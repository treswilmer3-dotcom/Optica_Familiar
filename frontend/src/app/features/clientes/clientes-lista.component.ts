import { Component, OnInit, inject, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { debounceTime, distinctUntilChanged } from 'rxjs';
import { AuthService } from '../../core/auth/auth.service';
import { Cliente } from '../../core/models/api.models';
import { ClientesApi } from '../../core/services/api.services';

const TAMANO = 15;

@Component({
  selector: 'app-clientes-lista',
  standalone: true,
  imports: [DatePipe, ReactiveFormsModule, RouterLink, MatTableModule, MatButtonModule, MatIconModule, MatFormFieldModule, MatInputModule, MatProgressBarModule],
  template: `
    <div class="page">
      <div class="page-header">
        <h1>Clientes</h1>
        @if (auth.hasRole('ADMIN', 'VENDEDOR')) {
          <a mat-flat-button color="primary" routerLink="/clientes/nuevo"><mat-icon>person_add</mat-icon> Nuevo cliente</a>
        }
      </div>

      <mat-form-field appearance="outline" class="full">
        <mat-label>Buscar por nombre, apellido o identificación</mat-label>
        <mat-icon matPrefix>search</mat-icon>
        <input matInput [formControl]="buscar" autocomplete="off">
      </mat-form-field>

      @if (cargando()) { <mat-progress-bar mode="indeterminate" /> }
      <div class="table-wrap">
        <table mat-table [dataSource]="clientes()" class="full">
          <ng-container matColumnDef="identificacion">
            <th mat-header-cell *matHeaderCellDef>Identificación</th>
            <td mat-cell *matCellDef="let c">{{ c.numeroIdentificacion }}</td>
          </ng-container>
          <ng-container matColumnDef="nombre">
            <th mat-header-cell *matHeaderCellDef>Cliente</th>
            <td mat-cell *matCellDef="let c"><strong>{{ c.apellidos }} {{ c.nombres }}</strong></td>
          </ng-container>
          <ng-container matColumnDef="contacto">
            <th mat-header-cell *matHeaderCellDef>Contacto</th>
            <td mat-cell *matCellDef="let c">{{ c.celular || c.telefono || '—' }}<div class="muted">{{ c.correo }}</div></td>
          </ng-container>
          <ng-container matColumnDef="registro">
            <th mat-header-cell *matHeaderCellDef>Registro</th>
            <td mat-cell *matCellDef="let c">{{ c.fechaRegistro | date: 'dd/MM/yyyy' }}</td>
          </ng-container>
          <tr mat-header-row *matHeaderRowDef="columnas"></tr>
          <tr mat-row *matRowDef="let row; columns: columnas" class="fila" (click)="abrir(row)"></tr>
        </table>
        @if (!cargando() && clientes().length === 0) { <div class="empty">No se encontraron clientes.</div> }
      </div>

      <div class="paginacion">
        <button mat-button [disabled]="pagina() === 1" (click)="ir(pagina() - 1)"><mat-icon>chevron_left</mat-icon> Anterior</button>
        <span class="muted">Página {{ pagina() }}</span>
        <button mat-button [disabled]="!hayMas()" (click)="ir(pagina() + 1)">Siguiente <mat-icon>chevron_right</mat-icon></button>
      </div>
    </div>`,
  styles: [`
    .fila { cursor: pointer; } .fila:hover { background: rgba(0,0,0,.04); }
    .paginacion { display: flex; justify-content: center; align-items: center; gap: 12px; margin-top: 12px; }
  `]
})
export class ClientesListaComponent implements OnInit {
  readonly auth = inject(AuthService);
  private readonly api = inject(ClientesApi);
  private readonly router = inject(Router);

  readonly columnas = ['identificacion', 'nombre', 'contacto', 'registro'];
  readonly buscar = new FormControl('', { nonNullable: true });
  readonly clientes = signal<Cliente[]>([]);
  readonly cargando = signal(false);
  readonly pagina = signal(1);
  readonly hayMas = signal(false);

  ngOnInit() {
    this.cargar();
    this.buscar.valueChanges.pipe(debounceTime(300), distinctUntilChanged()).subscribe(() => this.ir(1));
  }

  ir(p: number) { this.pagina.set(p); this.cargar(); }

  private cargar() {
    this.cargando.set(true);
    this.api.listar(this.buscar.value.trim(), this.pagina(), TAMANO).subscribe({
      next: r => { this.clientes.set(r); this.hayMas.set(r.length === TAMANO); this.cargando.set(false); },
      error: () => this.cargando.set(false)
    });
  }

  abrir(c: Cliente) { this.router.navigate(['/clientes', c.id]); }
}
