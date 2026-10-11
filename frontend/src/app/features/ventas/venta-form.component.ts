import { Component, OnInit, computed, inject, input, signal } from '@angular/core';
import { CurrencyPipe } from '@angular/common';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar } from '@angular/material/snack-bar';
import { debounceTime, distinctUntilChanged, filter, switchMap } from 'rxjs';
import { AuthService } from '../../core/auth/auth.service';
import { Cliente, OrdenTrabajo, Producto } from '../../core/models/api.models';
import { ClientesApi, OrdenesApi, ProductosApi, VentasApi } from '../../core/services/api.services';

interface Linea { productoId: number; nombre: string; precio: number; cantidad: number; descuento: number; }
interface PagoLinea { metodoPago: string; valor: number; referencia: string; }

const redondear = (n: number) => Math.round((n + Number.EPSILON) * 100) / 100;

@Component({
  selector: 'app-venta-form',
  standalone: true,
  imports: [CurrencyPipe, ReactiveFormsModule, RouterLink, MatCardModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatAutocompleteModule, MatButtonModule, MatIconModule],
  template: `
    <div class="page">
      <div class="page-header"><h1>Nueva venta</h1></div>

      <mat-card>
        <mat-card-header><mat-card-title>Cliente</mat-card-title></mat-card-header>
        <mat-card-content>
          @if (cliente(); as c) {
            <div class="toolbar-row">
              <div><strong>{{ c.apellidos }} {{ c.nombres }}</strong><div class="muted">{{ c.numeroIdentificacion }}</div></div>
              @if (!clienteFijo()) { <button mat-button (click)="cliente.set(null)">Cambiar</button> }
            </div>
            @if (orden(); as o) { <div style="margin-top:8px"><span class="chip info">Orden {{ o.numeroOrden }} vinculada</span></div> }
          } @else {
            <mat-form-field appearance="outline" class="full">
              <mat-label>Buscar cliente por nombre o identificación</mat-label>
              <input matInput [formControl]="buscarCliente" [matAutocomplete]="acCliente">
              <mat-autocomplete #acCliente="matAutocomplete" (optionSelected)="elegirCliente($event.option.value)" [displayWith]="vacio">
                @for (c of clientesSugeridos(); track c.id) { <mat-option [value]="c">{{ c.apellidos }} {{ c.nombres }} · {{ c.numeroIdentificacion }}</mat-option> }
              </mat-autocomplete>
            </mat-form-field>
          }
        </mat-card-content>
      </mat-card>

      <mat-card>
        <mat-card-header><mat-card-title>Productos</mat-card-title></mat-card-header>
        <mat-card-content>
          <mat-form-field appearance="outline" class="full">
            <mat-label>Buscar producto por nombre o código</mat-label>
            <input matInput [formControl]="buscarProducto" [matAutocomplete]="acProd">
            <mat-autocomplete #acProd="matAutocomplete" (optionSelected)="agregar($event.option.value)" [displayWith]="vacio">
              @for (p of productosSugeridos(); track p.id) { <mat-option [value]="p">{{ p.codigo }} · {{ p.nombre }} — {{ p.precio | currency: auth.moneda() }}</mat-option> }
            </mat-autocomplete>
          </mat-form-field>

          @if (lineas().length) {
            <div class="table-wrap tabla-cards"><table class="lineas">
              <thead><tr><th>Producto</th><th class="right">Precio</th><th>Cant.</th><th>Descuento</th><th class="right">Subtotal</th><th></th></tr></thead>
              <tbody>
                @for (l of lineas(); track l.productoId; let i = $index) {
                  <tr>
                    <td data-label="Producto"><strong>{{ l.nombre }}</strong></td>
                    <td class="right" data-label="Precio">{{ l.precio | currency: auth.moneda() }}</td>
                    <td data-label="Cantidad"><input class="num" type="number" min="1" step="1" [value]="l.cantidad" (input)="cambiar(i, 'cantidad', $any($event.target).value)"></td>
                    <td data-label="Descuento"><input class="num" type="number" min="0" step="0.01" [value]="l.descuento" (input)="cambiar(i, 'descuento', $any($event.target).value)"></td>
                    <td class="right" data-label="Subtotal">{{ subtotalLinea(l) | currency: auth.moneda() }}</td>
                    <td data-label=""><button mat-icon-button (click)="quitar(i)" aria-label="Quitar"><mat-icon>close</mat-icon></button></td>
                  </tr>
                }
              </tbody>
            </table></div>
          } @else { <div class="empty">Agregue productos a la venta.</div> }
        </mat-card-content>
      </mat-card>

      <mat-card>
        <mat-card-header><mat-card-title>Pagos</mat-card-title></mat-card-header>
        <mat-card-content>
          @for (p of pagos(); track $index; let i = $index) {
            <div class="pago">
              <mat-form-field appearance="outline" subscriptSizing="dynamic">
                <mat-label>Método</mat-label>
                <mat-select [value]="p.metodoPago" (selectionChange)="cambiarPago(i, 'metodoPago', $event.value)">
                  <mat-option value="EFECTIVO">Efectivo</mat-option><mat-option value="TARJETA">Tarjeta</mat-option>
                  <mat-option value="TRANSFERENCIA">Transferencia</mat-option><mat-option value="OTRO">Otro</mat-option>
                </mat-select>
              </mat-form-field>
              <mat-form-field appearance="outline" subscriptSizing="dynamic">
                <mat-label>Valor</mat-label>
                <input matInput type="number" min="0.01" step="0.01" [value]="p.valor" (input)="cambiarPago(i, 'valor', $any($event.target).value)">
              </mat-form-field>
              <mat-form-field appearance="outline" subscriptSizing="dynamic">
                <mat-label>Referencia</mat-label>
                <input matInput [value]="p.referencia" maxlength="100" (input)="cambiarPago(i, 'referencia', $any($event.target).value)">
              </mat-form-field>
              <button mat-icon-button (click)="quitarPago(i)" aria-label="Quitar pago"><mat-icon>close</mat-icon></button>
            </div>
          }
          <div class="toolbar-row">
            <button mat-stroked-button (click)="agregarPago()" [disabled]="total() <= 0"><mat-icon>add</mat-icon> Agregar pago</button>
            <button mat-stroked-button (click)="pagarTodo()" [disabled]="total() <= 0 || saldo() <= 0">Pagar saldo completo</button>
          </div>
        </mat-card-content>
      </mat-card>

      <mat-card>
        <mat-card-content class="totales">
          <div><span>Subtotal</span><strong>{{ subtotal() | currency: auth.moneda() }}</strong></div>
          <div><span>Descuentos</span><strong>{{ descuentos() | currency: auth.moneda() }}</strong></div>
          <div><span>IVA ({{ auth.ivaPorcentaje() }}%)</span><strong>{{ iva() | currency: auth.moneda() }}</strong></div>
          <div class="grande"><span>Total</span><strong>{{ total() | currency: auth.moneda() }}</strong></div>
          <div><span>Pagado</span><strong>{{ pagado() | currency: auth.moneda() }}</strong></div>
          <div [class.alerta]="saldo() < 0"><span>Saldo</span><strong>{{ saldo() | currency: auth.moneda() }}</strong></div>
        </mat-card-content>
      </mat-card>

      <div class="toolbar-row">
        <button mat-flat-button color="primary" (click)="guardar()" [disabled]="guardando()">Registrar venta</button>
        <a mat-button routerLink="/ventas">Cancelar</a>
      </div>
    </div>`,
  styles: [`
    table.lineas { width: 100%; border-collapse: collapse; }
    table.lineas th, table.lineas td { padding: 6px 8px; border-bottom: 1px solid #eee; text-align: left; }
    table.lineas th.right, table.lineas td.right { text-align: right; }
    input.num { width: 84px; padding: 6px; border: 1px solid #bbb; border-radius: 4px; }
    .pago { display: flex; gap: 12px; align-items: center; flex-wrap: wrap; margin-bottom: 12px; }
    .totales { display: grid; grid-template-columns: repeat(auto-fit, minmax(150px, 1fr)); gap: 12px; }
    .totales div { display: flex; flex-direction: column; } .totales span { color: rgba(0,0,0,.6); font-size: .85rem; }
    .grande strong { font-size: 1.4rem; } .alerta strong { color: #b71c1c; }
  `]
})
export class VentaFormComponent implements OnInit {
  /** Query params ?clienteId=&ordenId= */
  readonly clienteId = input<string>();
  readonly ordenId = input<string>();

  readonly auth = inject(AuthService);
  private readonly clientesApi = inject(ClientesApi);
  private readonly productosApi = inject(ProductosApi);
  private readonly ordenesApi = inject(OrdenesApi);
  private readonly api = inject(VentasApi);
  private readonly router = inject(Router);
  private readonly snack = inject(MatSnackBar);

  readonly vacio = () => '';
  readonly buscarCliente = new FormControl<string | Cliente>('', { nonNullable: true });
  readonly buscarProducto = new FormControl<string | Producto>('', { nonNullable: true });
  readonly clientesSugeridos = signal<Cliente[]>([]);
  readonly productosSugeridos = signal<Producto[]>([]);
  readonly cliente = signal<Cliente | null>(null);
  readonly clienteFijo = signal(false);
  readonly orden = signal<OrdenTrabajo | null>(null);
  readonly lineas = signal<Linea[]>([]);
  readonly pagos = signal<PagoLinea[]>([]);
  readonly guardando = signal(false);

  // Cálculo de vista previa; el servidor recalcula con precios y IVA oficiales.
  readonly subtotal = computed(() => redondear(this.lineas().reduce((s, l) => s + this.subtotalLinea(l), 0)));
  readonly descuentos = computed(() => redondear(this.lineas().reduce((s, l) => s + l.descuento, 0)));
  readonly iva = computed(() => redondear(this.subtotal() * this.auth.ivaPorcentaje() / 100));
  readonly total = computed(() => redondear(this.subtotal() + this.iva()));
  readonly pagado = computed(() => redondear(this.pagos().reduce((s, p) => s + (+p.valor || 0), 0)));
  readonly saldo = computed(() => redondear(this.total() - this.pagado()));

  ngOnInit() {
    const cid = this.clienteId();
    if (cid) { this.clientesApi.obtener(+cid).subscribe(c => { this.cliente.set(c); this.clienteFijo.set(true); }); }
    const oid = this.ordenId();
    if (oid) this.ordenesApi.obtener(+oid).subscribe(o => this.orden.set(o));

    this.buscarCliente.valueChanges.pipe(
      debounceTime(250), distinctUntilChanged(), filter((v): v is string => typeof v === 'string' && v.trim().length >= 2),
      switchMap(v => this.clientesApi.listar(v.trim(), 1, 8))
    ).subscribe(r => this.clientesSugeridos.set(r));

    this.buscarProducto.valueChanges.pipe(
      debounceTime(250), distinctUntilChanged(), filter((v): v is string => typeof v === 'string'),
      switchMap(v => this.productosApi.listar(v.trim() || undefined))
    ).subscribe(r => this.productosSugeridos.set(r.slice(0, 15)));
    this.productosApi.listar().subscribe(r => this.productosSugeridos.set(r.slice(0, 15)));
  }

  elegirCliente(c: Cliente) { this.cliente.set(c); this.buscarCliente.setValue(''); }

  subtotalLinea(l: Linea) { return Math.max(redondear(l.precio * l.cantidad - l.descuento), 0); }

  agregar(p: Producto) {
    this.buscarProducto.setValue('');
    const i = this.lineas().findIndex(l => l.productoId === p.id);
    if (i >= 0) { this.cambiar(i, 'cantidad', String(this.lineas()[i].cantidad + 1)); return; }
    this.lineas.update(ls => [...ls, { productoId: p.id, nombre: p.nombre, precio: p.precio, cantidad: 1, descuento: 0 }]);
  }

  cambiar(i: number, campo: 'cantidad' | 'descuento', valor: string) {
    const n = Math.max(+valor || 0, 0);
    this.lineas.update(ls => ls.map((l, k) => k === i ? { ...l, [campo]: campo === 'cantidad' ? Math.floor(n) : n } : l));
  }
  quitar(i: number) { this.lineas.update(ls => ls.filter((_, k) => k !== i)); }

  agregarPago() { this.pagos.update(ps => [...ps, { metodoPago: 'EFECTIVO', valor: Math.max(this.saldo(), 0), referencia: '' }]); }
  pagarTodo() { this.pagos.update(ps => [...ps, { metodoPago: 'EFECTIVO', valor: this.saldo(), referencia: '' }]); }
  cambiarPago(i: number, campo: keyof PagoLinea, valor: string) {
    this.pagos.update(ps => ps.map((p, k) => k === i ? { ...p, [campo]: campo === 'valor' ? (+valor || 0) : valor } : p));
  }
  quitarPago(i: number) { this.pagos.update(ps => ps.filter((_, k) => k !== i)); }

  guardar() {
    const c = this.cliente();
    if (!c) { this.snack.open('Seleccione el cliente.'); return; }
    if (this.lineas().length === 0) { this.snack.open('Agregue al menos un producto.'); return; }
    if (this.lineas().some(l => l.cantidad < 1)) { this.snack.open('La cantidad debe ser al menos 1.'); return; }
    if (this.lineas().some(l => l.descuento > l.precio * l.cantidad)) { this.snack.open('Un descuento supera el valor de su línea.'); return; }
    if (this.pagos().some(p => !(p.valor > 0))) { this.snack.open('Todos los pagos deben tener un valor mayor a cero.'); return; }
    if (this.saldo() < 0) { this.snack.open('Los pagos superan el total de la venta.'); return; }

    this.guardando.set(true);
    this.api.crear({
      clienteId: c.id,
      ordenTrabajoId: this.orden()?.id ?? null,
      detalles: this.lineas().map(l => ({ productoId: l.productoId, cantidad: l.cantidad, precioUnitario: null, descuento: l.descuento })),
      pagos: this.pagos().map(p => ({ metodoPago: p.metodoPago, valor: p.valor, referencia: p.referencia.trim() || null }))
    }).subscribe({
      next: v => { this.snack.open(`Venta ${v.numeroFactura} registrada`); this.router.navigate(['/ventas', v.id]); },
      error: () => this.guardando.set(false)
    });
  }
}
