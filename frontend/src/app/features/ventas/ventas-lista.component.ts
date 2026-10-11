import { Component, OnInit, inject, signal } from '@angular/core';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { AuthService } from '../../core/auth/auth.service';
import { Venta } from '../../core/models/api.models';
import { VentasApi } from '../../core/services/api.services';
import { claseEstado, etiquetaEstado } from '../../shared/ui';

const TAMANO = 15;

@Component({
  selector: 'app-ventas-lista',
  standalone: true,
  imports: [CurrencyPipe, DatePipe, RouterLink, MatTableModule, MatButtonModule, MatIconModule, MatProgressBarModule],
  template: `
    <div class="page">
      <div class="page-header">
        <h1>Ventas</h1>
        <a mat-flat-button color="primary" routerLink="/ventas/nueva"><mat-icon>add</mat-icon> Nueva venta</a>
      </div>
      @if (cargando()) { <mat-progress-bar mode="indeterminate" /> }
      <div class="table-wrap">
        <table mat-table [dataSource]="ventas()" class="full">
          <ng-container matColumnDef="factura"><th mat-header-cell *matHeaderCellDef>Factura</th><td mat-cell *matCellDef="let v"><strong>{{ v.numeroFactura }}</strong></td></ng-container>
          <ng-container matColumnDef="fecha"><th mat-header-cell *matHeaderCellDef>Fecha</th><td mat-cell *matCellDef="let v">{{ v.fechaVenta | date: 'dd/MM/yyyy HH:mm' }}</td></ng-container>
          <ng-container matColumnDef="cliente"><th mat-header-cell *matHeaderCellDef>Cliente</th><td mat-cell *matCellDef="let v">{{ v.clienteNombre }}</td></ng-container>
          <ng-container matColumnDef="total"><th mat-header-cell *matHeaderCellDef class="right">Total</th><td mat-cell *matCellDef="let v" class="right">{{ v.total | currency: auth.moneda() }}</td></ng-container>
          <ng-container matColumnDef="saldo"><th mat-header-cell *matHeaderCellDef class="right">Saldo</th>
            <td mat-cell *matCellDef="let v" class="right">{{ v.estado === 'ANULADA' ? '—' : ((v.total - v.totalPagado) | currency: auth.moneda()) }}</td></ng-container>
          <ng-container matColumnDef="estado"><th mat-header-cell *matHeaderCellDef>Estado</th>
            <td mat-cell *matCellDef="let v"><span class="chip" [class]="claseEstado(v.estado)">{{ etiqueta(v.estado) }}</span></td></ng-container>
          <tr mat-header-row *matHeaderRowDef="columnas"></tr>
          <tr mat-row *matRowDef="let row; columns: columnas" class="fila" (click)="abrir(row)"></tr>
        </table>
        @if (!cargando() && ventas().length === 0) { <div class="empty">Aún no hay ventas.</div> }
      </div>
      <div class="paginacion">
        <button mat-button [disabled]="pagina() === 1" (click)="ir(pagina() - 1)"><mat-icon>chevron_left</mat-icon> Anterior</button>
        <span class="muted">Página {{ pagina() }}</span>
        <button mat-button [disabled]="!hayMas()" (click)="ir(pagina() + 1)">Siguiente <mat-icon>chevron_right</mat-icon></button>
      </div>
    </div>`,
  styles: [`.fila { cursor: pointer; } .fila:hover { background: rgba(0,0,0,.04); } .paginacion { display: flex; justify-content: center; align-items: center; gap: 12px; margin-top: 12px; }`]
})
export class VentasListaComponent implements OnInit {
  readonly auth = inject(AuthService);
  private readonly api = inject(VentasApi);
  private readonly router = inject(Router);

  readonly columnas = ['factura', 'fecha', 'cliente', 'total', 'saldo', 'estado'];
  readonly claseEstado = claseEstado;
  readonly etiqueta = etiquetaEstado;
  readonly ventas = signal<Venta[]>([]);
  readonly cargando = signal(false);
  readonly pagina = signal(1);
  readonly hayMas = signal(false);

  ngOnInit() { this.cargar(); }
  ir(p: number) { this.pagina.set(p); this.cargar(); }
  abrir(v: Venta) { this.router.navigate(['/ventas', v.id]); }

  private cargar() {
    this.cargando.set(true);
    this.api.listar(this.pagina(), TAMANO).subscribe({
      next: r => { this.ventas.set(r); this.hayMas.set(r.length === TAMANO); this.cargando.set(false); },
      error: () => this.cargando.set(false)
    });
  }
}
