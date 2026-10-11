import { Component, OnInit, computed, inject, input, signal } from '@angular/core';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatTabsModule } from '@angular/material/tabs';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { AuthService } from '../../core/auth/auth.service';
import { ExamenVisual, HistorialCliente, OrdenTrabajo } from '../../core/models/api.models';
import { ClientesApi } from '../../core/services/api.services';
import { claseEstado, confirmar, etiquetaEstado } from '../../shared/ui';

@Component({
  selector: 'app-cliente-detalle',
  standalone: true,
  imports: [RouterLink, DatePipe, CurrencyPipe, MatCardModule, MatTabsModule, MatButtonModule, MatIconModule, MatProgressBarModule],
  template: `
    <div class="page">
      @if (!h()) { <mat-progress-bar mode="indeterminate" /> }
      @if (h(); as h) {
        <div class="page-header">
          <div>
            <h1>{{ h.cliente.apellidos }} {{ h.cliente.nombres }}</h1>
            <div class="muted">{{ h.cliente.tipoIdentificacion }} {{ h.cliente.numeroIdentificacion }} · cliente desde {{ h.cliente.fechaRegistro | date: 'dd/MM/yyyy' }}</div>
          </div>
          <div class="toolbar-row">
            @if (auth.hasRole('ADMIN', 'OPTOMETRISTA')) {
              <a mat-flat-button color="primary" [routerLink]="['/clientes', h.cliente.id, 'examen']"><mat-icon>visibility</mat-icon> Nuevo examen</a>
            }
            @if (auth.hasRole('ADMIN', 'VENDEDOR')) {
              <a mat-flat-button color="primary" routerLink="/ventas/nueva" [queryParams]="{ clienteId: h.cliente.id }"><mat-icon>point_of_sale</mat-icon> Nueva venta</a>
              <a mat-stroked-button [routerLink]="['/clientes', h.cliente.id, 'editar']"><mat-icon>edit</mat-icon> Editar</a>
            }
            @if (auth.hasRole('ADMIN')) {
              <button mat-stroked-button color="warn" (click)="eliminar()"><mat-icon>delete</mat-icon> Dar de baja</button>
            }
          </div>
        </div>

        <mat-card>
          <mat-card-content><div class="row g-3">
            <div class="col-6 col-md-3"><div class="muted">Celular</div>{{ h.cliente.celular || '—' }}</div>
            <div class="col-6 col-md-3"><div class="muted">Teléfono</div>{{ h.cliente.telefono || '—' }}</div>
            <div class="col-6 col-md-3"><div class="muted">Correo</div>{{ h.cliente.correo || '—' }}</div>
            <div class="col-6 col-md-3"><div class="muted">Nacimiento</div>{{ h.cliente.fechaNacimiento ? (h.cliente.fechaNacimiento | date: 'dd/MM/yyyy') : '—' }}</div>
            <div class="col-12 col-md-6"><div class="muted">Dirección</div>{{ h.cliente.direccion || '—' }}</div>
            <div class="col-12 col-md-6"><div class="muted">Observaciones</div>{{ h.cliente.observaciones || '—' }}</div>
          </div></mat-card-content>
        </mat-card>

        <mat-tab-group animationDuration="0">
          <mat-tab [label]="'Exámenes (' + h.examenes.length + ')'">
            <div class="tab">
              @for (e of h.examenes; track e.id) {
                <mat-card>
                  <mat-card-header>
                    <mat-card-title>Examen del {{ e.fechaConsulta | date: 'dd/MM/yyyy HH:mm' }}</mat-card-title>
                    <mat-card-subtitle>Historia {{ e.numeroHistoria }}</mat-card-subtitle>
                  </mat-card-header>
                  <mat-card-content>
                    <div><span class="muted">Motivo:</span> {{ e.motivoConsulta || '—' }}</div>
                    <div><span class="muted">Diagnóstico:</span> {{ e.diagnostico || '—' }}</div>
                    @if (e.observaciones) { <div><span class="muted">Observaciones:</span> {{ e.observaciones }}</div> }
                    @if (e.recomendaciones) { <div><span class="muted">Recomendaciones:</span> {{ e.recomendaciones }}</div> }
                    @if (e.receta; as r) {
                      <table class="receta">
                        <tr><th></th><th>Esfera</th><th>Cilindro</th><th>Eje</th><th>Adición</th></tr>
                        <tr><th>OD</th><td>{{ r.odEsfera ?? '—' }}</td><td>{{ r.odCilindro ?? '—' }}</td><td>{{ r.odEje ?? '—' }}</td><td>{{ r.odAdicion ?? '—' }}</td></tr>
                        <tr><th>OI</th><td>{{ r.oiEsfera ?? '—' }}</td><td>{{ r.oiCilindro ?? '—' }}</td><td>{{ r.oiEje ?? '—' }}</td><td>{{ r.oiAdicion ?? '—' }}</td></tr>
                      </table>
                      <div class="muted">DP: {{ r.distanciaPupilar ?? '—' }} mm @if (r.observacion) { · {{ r.observacion }} }</div>
                    }
                  </mat-card-content>
                  <mat-card-actions>
                    @if (!e.receta && auth.hasRole('ADMIN', 'OPTOMETRISTA')) {
                      <a mat-button color="primary" [routerLink]="['/examenes', e.id, 'receta']"><mat-icon>receipt_long</mat-icon> Agregar receta</a>
                    }
                    @if (e.receta && !tieneOrden(e) && auth.hasRole('ADMIN', 'VENDEDOR')) {
                      <a mat-button color="primary" routerLink="/ordenes/nueva" [queryParams]="{ recetaId: e.receta.id, clienteId: h.cliente.id }"><mat-icon>build</mat-icon> Crear orden de trabajo</a>
                    }
                  </mat-card-actions>
                </mat-card>
              } @empty { <div class="empty">Aún no hay exámenes visuales.</div> }
            </div>
          </mat-tab>

          <mat-tab [label]="'Órdenes (' + h.ordenes.length + ')'">
            <div class="tab">
              @for (o of h.ordenes; track o.id) {
                <mat-card><mat-card-content class="fila-o">
                  <div><strong>{{ o.numeroOrden }}</strong><div class="muted">Ingreso {{ o.fechaIngreso | date: 'dd/MM/yyyy' }}@if (o.fechaEntregaEstimada) { · entrega est. {{ o.fechaEntregaEstimada | date: 'dd/MM/yyyy' }} }</div></div>
                  <span class="chip" [class]="claseEstado(o.estado)">{{ etiqueta(o.estado) }}</span>
                </mat-card-content></mat-card>
              } @empty { <div class="empty">Sin órdenes de trabajo.</div> }
            </div>
          </mat-tab>

          @if (auth.hasRole('ADMIN', 'VENDEDOR')) {
            <mat-tab [label]="'Ventas (' + h.ventas.length + ')'">
              <div class="tab">
                @for (v of h.ventas; track v.id) {
                  <a [routerLink]="['/ventas', v.id]" class="enlace"><mat-card><mat-card-content class="fila-o">
                    <div><strong>{{ v.numeroFactura }}</strong><div class="muted">{{ v.fechaVenta | date: 'dd/MM/yyyy HH:mm' }}</div></div>
                    <div class="right"><strong>{{ v.total | currency: auth.moneda() }}</strong><br><span class="chip" [class]="claseEstado(v.estado)">{{ etiqueta(v.estado) }}</span></div>
                  </mat-card-content></mat-card></a>
                } @empty { <div class="empty">Sin ventas.</div> }
              </div>
            </mat-tab>
          }
        </mat-tab-group>
      }
    </div>`,
  styles: [`
    .tab { padding: 16px 0; }
    .fila-o { display: flex; justify-content: space-between; align-items: center; }
    .enlace { text-decoration: none; color: inherit; }
    table.receta { border-collapse: collapse; margin: 10px 0 4px; }
    table.receta th, table.receta td { border: 1px solid #ddd; padding: 4px 14px; text-align: center; }
    table.receta th { background: #f0f4f8; font-weight: 500; }
  `]
})
export class ClienteDetalleComponent implements OnInit {
  readonly id = input.required<string>();
  readonly auth = inject(AuthService);
  private readonly api = inject(ClientesApi);
  private readonly router = inject(Router);
  private readonly dialog = inject(MatDialog);
  private readonly snack = inject(MatSnackBar);

  readonly h = signal<HistorialCliente | null>(null);
  readonly claseEstado = claseEstado;
  readonly etiqueta = etiquetaEstado;
  private readonly ordenesPorReceta = computed(() => new Set((this.h()?.ordenes ?? []).map((o: OrdenTrabajo) => o.recetaId)));

  ngOnInit() { this.api.historial(+this.id()).subscribe(h => this.h.set(h)); }

  tieneOrden(e: ExamenVisual) { return !!e.receta && this.ordenesPorReceta().has(e.receta.id); }

  eliminar() {
    confirmar(this.dialog, {
      titulo: 'Dar de baja al cliente', peligro: true, confirmar: 'Dar de baja',
      mensaje: 'El cliente dejará de aparecer en las búsquedas. Su historial clínico y comercial se conserva.'
    }).subscribe(ok => {
      if (ok) this.api.eliminar(+this.id()).subscribe(() => { this.snack.open('Cliente dado de baja'); this.router.navigate(['/clientes']); });
    });
  }
}
