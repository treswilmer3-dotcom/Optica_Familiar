import { Component, OnInit, inject, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AuthService } from '../../core/auth/auth.service';
import { ESTADOS_ORDEN, OrdenTrabajo } from '../../core/models/api.models';
import { OrdenesApi } from '../../core/services/api.services';
import { claseEstado, confirmar, etiquetaEstado } from '../../shared/ui';

@Component({
  selector: 'app-ordenes-lista',
  standalone: true,
  imports: [DatePipe, MatTableModule, MatButtonModule, MatIconModule, MatFormFieldModule, MatSelectModule, MatProgressBarModule],
  template: `
    <div class="page">
      <div class="page-header">
        <h1>Órdenes de trabajo</h1>
        <mat-form-field appearance="outline" subscriptSizing="dynamic">
          <mat-label>Estado</mat-label>
          <mat-select [value]="filtro()" (selectionChange)="filtro.set($event.value); cargar()">
            <mat-option value="">Todos</mat-option>
            @for (e of estados; track e) { <mat-option [value]="e">{{ etiqueta(e) }}</mat-option> }
          </mat-select>
        </mat-form-field>
      </div>
      @if (cargando()) { <mat-progress-bar mode="indeterminate" /> }
      <div class="table-wrap tabla-cards">
        <table mat-table [dataSource]="ordenes()" class="full">
          <ng-container matColumnDef="numero"><th mat-header-cell *matHeaderCellDef>Orden</th><td mat-cell *matCellDef="let o" data-label="Orden"><strong>{{ o.numeroOrden }}</strong></td></ng-container>
          <ng-container matColumnDef="ingreso"><th mat-header-cell *matHeaderCellDef>Ingreso</th><td mat-cell *matCellDef="let o" data-label="Ingreso">{{ o.fechaIngreso | date: 'dd/MM/yyyy' }}</td></ng-container>
          <ng-container matColumnDef="entrega"><th mat-header-cell *matHeaderCellDef>Entrega estimada</th>
            <td mat-cell *matCellDef="let o" data-label="Entrega estimada">{{ o.fechaEntregaReal ? (o.fechaEntregaReal | date: 'dd/MM/yyyy') : (o.fechaEntregaEstimada ? (o.fechaEntregaEstimada | date: 'dd/MM/yyyy') : '—') }}</td></ng-container>
          <ng-container matColumnDef="venta"><th mat-header-cell *matHeaderCellDef>Venta</th><td mat-cell *matCellDef="let o" data-label="Venta">{{ o.ventaId ? '#' + o.ventaId : 'Sin facturar' }}</td></ng-container>
          <ng-container matColumnDef="estado"><th mat-header-cell *matHeaderCellDef>Estado</th>
            <td mat-cell *matCellDef="let o" data-label="Estado"><span class="chip" [class]="claseEstado(o.estado)">{{ etiqueta(o.estado) }}</span></td></ng-container>
          <ng-container matColumnDef="acciones"><th mat-header-cell *matHeaderCellDef></th>
            <td mat-cell *matCellDef="let o" data-label="" class="right">
              @if (puedeAvanzar && siguiente(o); as s) {
                <button mat-stroked-button color="primary" (click)="avanzar(o, s)">Pasar a {{ etiqueta(s) }}</button>
              }
            </td></ng-container>
          <tr mat-header-row *matHeaderRowDef="columnas"></tr>
          <tr mat-row *matRowDef="let row; columns: columnas"></tr>
        </table>
        @if (!cargando() && ordenes().length === 0) { <div class="empty">No hay órdenes de trabajo.</div> }
      </div>
    </div>`
})
export class OrdenesListaComponent implements OnInit {
  private readonly api = inject(OrdenesApi);
  private readonly auth = inject(AuthService);
  private readonly dialog = inject(MatDialog);
  private readonly snack = inject(MatSnackBar);

  readonly estados = ESTADOS_ORDEN;
  readonly columnas = ['numero', 'ingreso', 'entrega', 'venta', 'estado', 'acciones'];
  readonly puedeAvanzar = this.auth.hasRole('ADMIN', 'VENDEDOR');
  readonly claseEstado = claseEstado;
  readonly etiqueta = etiquetaEstado;
  readonly ordenes = signal<OrdenTrabajo[]>([]);
  readonly cargando = signal(false);
  readonly filtro = signal('');

  ngOnInit() { this.cargar(); }

  cargar() {
    this.cargando.set(true);
    this.api.listar(this.filtro() || undefined).subscribe({
      next: r => { this.ordenes.set(r); this.cargando.set(false); },
      error: () => this.cargando.set(false)
    });
  }

  siguiente(o: OrdenTrabajo): string | null {
    const i = ESTADOS_ORDEN.indexOf(o.estado as typeof ESTADOS_ORDEN[number]);
    return i >= 0 && i < ESTADOS_ORDEN.length - 1 ? ESTADOS_ORDEN[i + 1] : null;
  }

  avanzar(o: OrdenTrabajo, estado: string) {
    const entrega = estado === 'ENTREGADA';
    const accion = () => this.api.cambiarEstado(o.id, estado).subscribe(() => { this.snack.open(`Orden ${o.numeroOrden}: ${etiquetaEstado(estado)}`); this.cargar(); });
    if (!entrega) { accion(); return; }
    confirmar(this.dialog, { titulo: 'Entregar al cliente', mensaje: `Se registrará la entrega de la orden ${o.numeroOrden}. Esta acción no se puede deshacer.`, confirmar: 'Entregar' })
      .subscribe(ok => ok && accion());
  }
}
