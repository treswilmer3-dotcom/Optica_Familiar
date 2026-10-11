import { Component, OnInit, inject, input, signal } from '@angular/core';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { AuthService } from '../../core/auth/auth.service';
import { Venta } from '../../core/models/api.models';
import { VentasApi } from '../../core/services/api.services';
import { claseEstado, confirmar, etiquetaEstado, txt } from '../../shared/ui';

@Component({
  selector: 'app-venta-detalle',
  standalone: true,
  imports: [CurrencyPipe, DatePipe, ReactiveFormsModule, RouterLink, MatCardModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatButtonModule, MatIconModule, MatProgressBarModule],
  template: `
    <div class="page">
      @if (!v()) { <mat-progress-bar mode="indeterminate" /> }
      @if (v(); as v) {
        <div class="page-header">
          <div>
            <h1>Factura {{ v.numeroFactura }} <span class="chip" [class]="claseEstado(v.estado)">{{ etiqueta(v.estado) }}</span></h1>
            <div class="muted">{{ v.fechaVenta | date: 'dd/MM/yyyy HH:mm' }} ·
              <a [routerLink]="['/clientes', v.clienteId]">{{ v.clienteNombre }}</a></div>
          </div>
          <div class="toolbar-row">
            <button mat-stroked-button (click)="imprimir()"><mat-icon>print</mat-icon> Imprimir</button>
            @if (auth.hasRole('ADMIN') && v.estado !== 'ANULADA') {
              <button mat-stroked-button color="warn" (click)="anular()"><mat-icon>block</mat-icon> Anular</button>
            }
          </div>
        </div>

        <mat-card><mat-card-content>
          <div class="table-wrap tabla-cards"><table class="detalle">
            <thead><tr><th>Producto</th><th class="right">Precio</th><th class="right">Cant.</th><th class="right">Desc.</th><th class="right">Subtotal</th></tr></thead>
            <tbody>
              @for (d of v.detalles; track d.id) {
                <tr><td data-label="Producto"><strong>{{ d.producto }}</strong></td><td class="right" data-label="Precio">{{ d.precioUnitario | currency: auth.moneda() }}</td><td class="right" data-label="Cantidad">{{ d.cantidad }}</td>
                  <td class="right" data-label="Descuento">{{ d.descuento | currency: auth.moneda() }}</td><td class="right" data-label="Subtotal">{{ d.subtotal | currency: auth.moneda() }}</td></tr>
              }
            </tbody>
          </table></div>
          <div class="totales">
            <div><span>Subtotal</span>{{ v.subtotal | currency: auth.moneda() }}</div>
            <div><span>IVA</span>{{ v.iva | currency: auth.moneda() }}</div>
            <div class="grande"><span>Total</span>{{ v.total | currency: auth.moneda() }}</div>
            <div><span>Pagado</span>{{ v.totalPagado | currency: auth.moneda() }}</div>
            <div><span>Saldo</span>{{ v.estado === 'ANULADA' ? '—' : ((v.total - v.totalPagado) | currency: auth.moneda()) }}</div>
          </div>
        </mat-card-content></mat-card>

        <mat-card>
          <mat-card-header><mat-card-title>Pagos</mat-card-title></mat-card-header>
          <mat-card-content>
            @for (p of v.pagos; track p.id) {
              <div class="pago-fila"><span>{{ p.fechaPago | date: 'dd/MM/yyyy HH:mm' }} · {{ p.metodoPago }}@if (p.referencia) { · {{ p.referencia }} }</span><strong>{{ p.valor | currency: auth.moneda() }}</strong></div>
            } @empty { <div class="muted">Sin pagos registrados.</div> }

            @if (v.estado === 'PENDIENTE') {
              <form [formGroup]="form" (ngSubmit)="pagar()" class="nuevo-pago">
                <mat-form-field appearance="outline" subscriptSizing="dynamic"><mat-label>Método</mat-label>
                  <mat-select formControlName="metodoPago"><mat-option value="EFECTIVO">Efectivo</mat-option><mat-option value="TARJETA">Tarjeta</mat-option>
                    <mat-option value="TRANSFERENCIA">Transferencia</mat-option><mat-option value="OTRO">Otro</mat-option></mat-select></mat-form-field>
                <mat-form-field appearance="outline" subscriptSizing="dynamic"><mat-label>Valor</mat-label>
                  <input matInput type="number" step="0.01" formControlName="valor"></mat-form-field>
                <mat-form-field appearance="outline" subscriptSizing="dynamic"><mat-label>Referencia</mat-label>
                  <input matInput formControlName="referencia" maxlength="100"></mat-form-field>
                <button mat-flat-button color="primary" type="submit" [disabled]="registrando()">Registrar pago</button>
              </form>
            }
          </mat-card-content>
        </mat-card>
        <a mat-button routerLink="/ventas"><mat-icon>arrow_back</mat-icon> Volver a ventas</a>
      }
    </div>`,
  styles: [`
    table.detalle { width: 100%; border-collapse: collapse; }
    table.detalle th, table.detalle td { padding: 6px 8px; border-bottom: 1px solid #eee; text-align: left; }
    table.detalle .right { text-align: right; }
    .totales { display: grid; grid-template-columns: repeat(auto-fit, minmax(130px, 1fr)); gap: 12px; margin-top: 16px; }
    .totales div { display: flex; flex-direction: column; } .totales span { color: rgba(0,0,0,.6); font-size: .85rem; }
    .grande { font-size: 1.3rem; font-weight: 500; }
    .pago-fila { display: flex; justify-content: space-between; padding: 6px 0; border-bottom: 1px solid #eee; }
    .nuevo-pago { display: flex; gap: 12px; flex-wrap: wrap; align-items: center; margin-top: 16px; }
    @media print { .toolbar-row, a, button, form { display: none !important; } }
  `]
})
export class VentaDetalleComponent implements OnInit {
  readonly id = input.required<string>();
  readonly auth = inject(AuthService);
  private readonly api = inject(VentasApi);
  private readonly fb = inject(FormBuilder);
  private readonly dialog = inject(MatDialog);
  private readonly snack = inject(MatSnackBar);

  readonly v = signal<Venta | null>(null);
  readonly registrando = signal(false);
  readonly claseEstado = claseEstado;
  readonly etiqueta = etiquetaEstado;
  readonly form = this.fb.group({ metodoPago: ['EFECTIVO', Validators.required], valor: [null as number | null, [Validators.required, Validators.min(0.01)]], referencia: [''] });

  ngOnInit() { this.api.obtener(+this.id()).subscribe(v => this.asignar(v)); }

  private asignar(v: Venta) {
    this.v.set(v);
    this.form.patchValue({ valor: Math.round((v.total - v.totalPagado) * 100) / 100 });
  }

  pagar() {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    const f = this.form.getRawValue();
    this.registrando.set(true);
    this.api.registrarPago(+this.id(), { metodoPago: f.metodoPago!, valor: f.valor!, referencia: txt(f.referencia) }).subscribe({
      next: v => { this.registrando.set(false); this.snack.open('Pago registrado'); this.asignar(v); },
      error: () => this.registrando.set(false)
    });
  }

  anular() {
    confirmar(this.dialog, { titulo: 'Anular venta', peligro: true, confirmar: 'Anular', mensaje: 'La venta quedará anulada y no podrá recibir más pagos. Esta acción no se puede deshacer.' })
      .subscribe(ok => ok && this.api.anular(+this.id()).subscribe(v => { this.snack.open('Venta anulada'); this.asignar(v); }));
  }

  imprimir() { window.print(); }
}
