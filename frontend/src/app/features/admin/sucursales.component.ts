import { Component, OnInit, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MAT_DIALOG_DATA, MatDialog, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AuthService } from '../../core/auth/auth.service';
import { Empresa, Sucursal } from '../../core/models/api.models';
import { AdminApi } from '../../core/services/api.services';
import { claseEstado, etiquetaEstado, txt } from '../../shared/ui';

interface SucursalDialogData { sucursal: Sucursal | null; empresas: Empresa[]; superadmin: boolean; }

@Component({
  selector: 'app-sucursal-dialog',
  standalone: true,
  imports: [ReactiveFormsModule, MatDialogModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatButtonModule],
  template: `
    <h2 mat-dialog-title>{{ data.sucursal ? 'Editar sucursal' : 'Nueva sucursal' }}</h2>
    <form [formGroup]="form" (ngSubmit)="guardar()">
      <mat-dialog-content>
        <div class="form-grid">
          @if (data.superadmin && !data.sucursal) {
            <mat-form-field appearance="outline" class="span-2"><mat-label>Empresa</mat-label>
              <mat-select formControlName="empresaId">@for (e of data.empresas; track e.id) { <mat-option [value]="e.id">{{ e.nombreComercial }} ({{ e.codigo }})</mat-option> }</mat-select>
              <mat-error>Requerida</mat-error></mat-form-field>
          }
          <mat-form-field appearance="outline"><mat-label>Código</mat-label><input matInput formControlName="codigo" maxlength="50" style="text-transform: uppercase"><mat-error>Requerido</mat-error></mat-form-field>
          <mat-form-field appearance="outline"><mat-label>Nombre</mat-label><input matInput formControlName="nombre" maxlength="200"><mat-error>Requerido</mat-error></mat-form-field>
          <mat-form-field appearance="outline"><mat-label>Ciudad</mat-label><input matInput formControlName="ciudad" maxlength="100"></mat-form-field>
          <mat-form-field appearance="outline"><mat-label>Provincia</mat-label><input matInput formControlName="provincia" maxlength="100"></mat-form-field>
          <mat-form-field appearance="outline"><mat-label>Teléfono</mat-label><input matInput formControlName="telefono" maxlength="20"></mat-form-field>
          <mat-form-field appearance="outline"><mat-label>Correo</mat-label><input matInput type="email" formControlName="correo" maxlength="100"><mat-error>Correo inválido</mat-error></mat-form-field>
          <mat-form-field appearance="outline" class="span-2"><mat-label>Dirección</mat-label><input matInput formControlName="direccion" maxlength="500"></mat-form-field>
        </div>
      </mat-dialog-content>
      <mat-dialog-actions align="end">
        <button mat-button type="button" mat-dialog-close>Cancelar</button>
        <button mat-flat-button color="primary" type="submit" [disabled]="guardando()">Guardar</button>
      </mat-dialog-actions>
    </form>`
})
export class SucursalDialogComponent {
  readonly data = inject<SucursalDialogData>(MAT_DIALOG_DATA);
  private readonly fb = inject(FormBuilder);
  private readonly api = inject(AdminApi);
  private readonly ref = inject(MatDialogRef<SucursalDialogComponent>);
  readonly guardando = signal(false);
  readonly form = this.fb.group({
    empresaId: [null as number | null], codigo: ['', Validators.required], nombre: ['', Validators.required],
    ciudad: [''], provincia: [''], telefono: [''], correo: ['', Validators.email], direccion: ['']
  });

  constructor() {
    const s = this.data.sucursal;
    if (s) this.form.patchValue({ ...s, empresaId: s.empresaId ?? null, correo: s.correo ?? '' });
    else if (this.data.superadmin) this.form.controls.empresaId.addValidators(Validators.required);
  }

  guardar() {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    const v = this.form.getRawValue();
    const req = {
      empresaId: this.data.superadmin ? v.empresaId : null, codigo: v.codigo!.trim().toUpperCase(), nombre: v.nombre!.trim(),
      ciudad: txt(v.ciudad), provincia: txt(v.provincia), telefono: txt(v.telefono), correo: txt(v.correo), direccion: txt(v.direccion)
    };
    this.guardando.set(true);
    const s = this.data.sucursal;
    (s ? this.api.actualizarSucursal(s.id, req) : this.api.crearSucursal(req)).subscribe({
      next: r => this.ref.close(r), error: () => this.guardando.set(false)
    });
  }
}

@Component({
  selector: 'app-sucursales',
  standalone: true,
  imports: [MatTableModule, MatButtonModule, MatIconModule],
  template: `
    <div class="page">
      <div class="page-header">
        <h1>Sucursales</h1>
        <button mat-flat-button color="primary" (click)="abrir(null)"><mat-icon>add</mat-icon> Nueva sucursal</button>
      </div>
      <div class="table-wrap">
        <table mat-table [dataSource]="sucursales()" class="full">
          @if (auth.hasRole('SUPERADMIN')) {
            <ng-container matColumnDef="empresa"><th mat-header-cell *matHeaderCellDef>Empresa</th><td mat-cell *matCellDef="let s">{{ nombreEmpresa(s.empresaId) }}</td></ng-container>
          }
          <ng-container matColumnDef="codigo"><th mat-header-cell *matHeaderCellDef>Código</th><td mat-cell *matCellDef="let s">{{ s.codigo }}</td></ng-container>
          <ng-container matColumnDef="nombre"><th mat-header-cell *matHeaderCellDef>Nombre</th><td mat-cell *matCellDef="let s"><strong>{{ s.nombre }}</strong><div class="muted">{{ s.direccion }}</div></td></ng-container>
          <ng-container matColumnDef="ciudad"><th mat-header-cell *matHeaderCellDef>Ciudad</th><td mat-cell *matCellDef="let s">{{ s.ciudad || '—' }}</td></ng-container>
          <ng-container matColumnDef="estado"><th mat-header-cell *matHeaderCellDef>Estado</th><td mat-cell *matCellDef="let s"><span class="chip" [class]="claseEstado(s.estado)">{{ etiqueta(s.estado) }}</span></td></ng-container>
          <ng-container matColumnDef="acciones"><th mat-header-cell *matHeaderCellDef></th><td mat-cell *matCellDef="let s" class="right"><button mat-icon-button (click)="abrir(s)" aria-label="Editar"><mat-icon>edit</mat-icon></button></td></ng-container>
          <tr mat-header-row *matHeaderRowDef="columnas"></tr><tr mat-row *matRowDef="let row; columns: columnas"></tr>
        </table>
        @if (sucursales().length === 0) { <div class="empty">No hay sucursales.</div> }
      </div>
    </div>`
})
export class SucursalesComponent implements OnInit {
  readonly auth = inject(AuthService);
  private readonly api = inject(AdminApi);
  private readonly dialog = inject(MatDialog);
  private readonly snack = inject(MatSnackBar);

  readonly columnas = this.auth.hasRole('SUPERADMIN') ? ['empresa', 'codigo', 'nombre', 'ciudad', 'estado', 'acciones'] : ['codigo', 'nombre', 'ciudad', 'estado', 'acciones'];
  readonly claseEstado = claseEstado;
  readonly etiqueta = etiquetaEstado;
  readonly sucursales = signal<Sucursal[]>([]);
  readonly empresas = signal<Empresa[]>([]);

  ngOnInit() {
    this.cargar();
    if (this.auth.hasRole('SUPERADMIN')) this.api.empresas().subscribe(e => this.empresas.set(e));
  }
  private cargar() { this.api.sucursales().subscribe(s => this.sucursales.set(s)); }
  nombreEmpresa(id?: number | null) { return this.empresas().find(e => e.id === id)?.nombreComercial ?? `#${id}`; }

  abrir(s: Sucursal | null) {
    this.dialog.open(SucursalDialogComponent, { width: '600px', data: { sucursal: s, empresas: this.empresas(), superadmin: this.auth.hasRole('SUPERADMIN') } })
      .afterClosed().subscribe(r => { if (r) { this.snack.open('Sucursal guardada'); this.cargar(); } });
  }
}
