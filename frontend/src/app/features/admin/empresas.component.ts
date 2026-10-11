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
import { Empresa } from '../../core/models/api.models';
import { AdminApi } from '../../core/services/api.services';
import { claseEstado, etiquetaEstado, txt } from '../../shared/ui';

@Component({
  selector: 'app-empresa-dialog',
  standalone: true,
  imports: [ReactiveFormsModule, MatDialogModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatButtonModule],
  template: `
    <h2 mat-dialog-title>{{ data ? 'Editar empresa' : 'Nueva empresa' }}</h2>
    <form [formGroup]="form" (ngSubmit)="guardar()">
      <mat-dialog-content>
        <div class="form-grid">
          <mat-form-field appearance="outline"><mat-label>Código de empresa (para el login)</mat-label>
            <input matInput formControlName="codigo" maxlength="50" style="text-transform: uppercase"><mat-error>Requerido</mat-error></mat-form-field>
          <mat-form-field appearance="outline"><mat-label>Nombre comercial</mat-label><input matInput formControlName="nombreComercial" maxlength="200"><mat-error>Requerido</mat-error></mat-form-field>
          <mat-form-field appearance="outline"><mat-label>Razón social</mat-label><input matInput formControlName="razonSocial" maxlength="200"><mat-error>Requerida</mat-error></mat-form-field>
          <mat-form-field appearance="outline"><mat-label>Identificación fiscal (RUC)</mat-label><input matInput formControlName="identificacionFiscal" maxlength="30"><mat-error>Requerida</mat-error></mat-form-field>
          <mat-form-field appearance="outline"><mat-label>País (ISO, ej. EC)</mat-label><input matInput formControlName="pais" maxlength="2" style="text-transform: uppercase"><mat-error>2 letras</mat-error></mat-form-field>
          <mat-form-field appearance="outline"><mat-label>Moneda (ISO, ej. USD)</mat-label><input matInput formControlName="moneda" maxlength="3" style="text-transform: uppercase"><mat-error>3 letras</mat-error></mat-form-field>
          <mat-form-field appearance="outline"><mat-label>Zona horaria</mat-label><input matInput formControlName="zonaHoraria" maxlength="50"><mat-error>Requerida</mat-error></mat-form-field>
          <mat-form-field appearance="outline"><mat-label>Idioma</mat-label><input matInput formControlName="idioma" maxlength="5"><mat-error>Requerido</mat-error></mat-form-field>
          <mat-form-field appearance="outline"><mat-label>IVA / impuesto (%)</mat-label><input matInput type="number" step="0.01" formControlName="ivaPorcentaje"><mat-error>0 a 100</mat-error></mat-form-field>
          <mat-form-field appearance="outline"><mat-label>Teléfono</mat-label><input matInput formControlName="telefono" maxlength="20"></mat-form-field>
          <mat-form-field appearance="outline"><mat-label>Correo</mat-label><input matInput type="email" formControlName="correo" maxlength="100"><mat-error>Correo inválido</mat-error></mat-form-field>
          @if (data) {
            <mat-form-field appearance="outline"><mat-label>Estado</mat-label>
              <mat-select formControlName="estado"><mat-option value="ACTIVO">Activa</mat-option><mat-option value="INACTIVO">Inactiva (sin acceso)</mat-option></mat-select></mat-form-field>
          }
          <mat-form-field appearance="outline" class="span-2"><mat-label>Dirección</mat-label><input matInput formControlName="direccion" maxlength="500"></mat-form-field>
        </div>
      </mat-dialog-content>
      <mat-dialog-actions align="end">
        <button mat-button type="button" mat-dialog-close>Cancelar</button>
        <button mat-flat-button color="primary" type="submit" [disabled]="guardando()">Guardar</button>
      </mat-dialog-actions>
    </form>`
})
export class EmpresaDialogComponent {
  readonly data = inject<Empresa | null>(MAT_DIALOG_DATA);
  private readonly fb = inject(FormBuilder);
  private readonly api = inject(AdminApi);
  private readonly ref = inject(MatDialogRef<EmpresaDialogComponent>);
  readonly guardando = signal(false);
  readonly form = this.fb.group({
    codigo: ['', Validators.required], nombreComercial: ['', Validators.required], razonSocial: ['', Validators.required],
    identificacionFiscal: ['', Validators.required], pais: ['EC', [Validators.required, Validators.minLength(2), Validators.maxLength(2)]],
    moneda: ['USD', [Validators.required, Validators.minLength(3), Validators.maxLength(3)]],
    zonaHoraria: ['America/Guayaquil', Validators.required], idioma: ['es', Validators.required],
    ivaPorcentaje: [15, [Validators.required, Validators.min(0), Validators.max(100)]],
    telefono: [''], correo: ['', Validators.email], direccion: [''], estado: ['ACTIVO']
  });

  constructor() {
    const e = this.data;
    if (e) this.form.patchValue({ ...e, telefono: e.telefono ?? '', correo: e.correo ?? '', direccion: e.direccion ?? '', estado: e.estado ?? 'ACTIVO' });
  }

  guardar() {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    const v = this.form.getRawValue();
    const req = {
      codigo: v.codigo!.trim().toUpperCase(), nombreComercial: v.nombreComercial!.trim(), razonSocial: v.razonSocial!.trim(),
      identificacionFiscal: v.identificacionFiscal!.trim(), pais: v.pais!.trim().toUpperCase(), moneda: v.moneda!.trim().toUpperCase(),
      zonaHoraria: v.zonaHoraria!.trim(), idioma: v.idioma!.trim().toLowerCase(), ivaPorcentaje: v.ivaPorcentaje!,
      telefono: txt(v.telefono), correo: txt(v.correo), direccion: txt(v.direccion), sitioWeb: this.data?.sitioWeb ?? null,
      estado: this.data ? v.estado : null
    };
    this.guardando.set(true);
    (this.data ? this.api.actualizarEmpresa(this.data.id, req) : this.api.crearEmpresa(req)).subscribe({
      next: r => this.ref.close(r), error: () => this.guardando.set(false)
    });
  }
}

@Component({
  selector: 'app-empresas',
  standalone: true,
  imports: [MatTableModule, MatButtonModule, MatIconModule],
  template: `
    <div class="page">
      <div class="page-header">
        <h1>Empresas</h1>
        <button mat-flat-button color="primary" (click)="abrir(null)"><mat-icon>add_business</mat-icon> Nueva empresa</button>
      </div>
      <p class="muted">Tras crear una empresa, cree su primera sucursal (menú Sucursales) y su usuario administrador (menú Usuarios).</p>
      <div class="table-wrap">
        <table mat-table [dataSource]="empresas()" class="full">
          <ng-container matColumnDef="codigo"><th mat-header-cell *matHeaderCellDef>Código</th><td mat-cell *matCellDef="let e">{{ e.codigo }}</td></ng-container>
          <ng-container matColumnDef="nombre"><th mat-header-cell *matHeaderCellDef>Empresa</th><td mat-cell *matCellDef="let e"><strong>{{ e.nombreComercial }}</strong><div class="muted">{{ e.razonSocial }}</div></td></ng-container>
          <ng-container matColumnDef="fiscal"><th mat-header-cell *matHeaderCellDef>Identificación fiscal</th><td mat-cell *matCellDef="let e">{{ e.pais }} · {{ e.identificacionFiscal }}</td></ng-container>
          <ng-container matColumnDef="moneda"><th mat-header-cell *matHeaderCellDef>Moneda / IVA</th><td mat-cell *matCellDef="let e">{{ e.moneda }} · {{ e.ivaPorcentaje }}%</td></ng-container>
          <ng-container matColumnDef="estado"><th mat-header-cell *matHeaderCellDef>Estado</th><td mat-cell *matCellDef="let e"><span class="chip" [class]="claseEstado(e.estado)">{{ etiqueta(e.estado) }}</span></td></ng-container>
          <ng-container matColumnDef="acciones"><th mat-header-cell *matHeaderCellDef></th><td mat-cell *matCellDef="let e" class="right"><button mat-icon-button (click)="abrir(e)" aria-label="Editar"><mat-icon>edit</mat-icon></button></td></ng-container>
          <tr mat-header-row *matHeaderRowDef="columnas"></tr><tr mat-row *matRowDef="let row; columns: columnas"></tr>
        </table>
      </div>
    </div>`
})
export class EmpresasComponent implements OnInit {
  private readonly api = inject(AdminApi);
  private readonly dialog = inject(MatDialog);
  private readonly snack = inject(MatSnackBar);
  readonly columnas = ['codigo', 'nombre', 'fiscal', 'moneda', 'estado', 'acciones'];
  readonly claseEstado = claseEstado;
  readonly etiqueta = etiquetaEstado;
  readonly empresas = signal<Empresa[]>([]);

  ngOnInit() { this.cargar(); }
  private cargar() { this.api.empresas().subscribe(e => this.empresas.set(e)); }

  abrir(e: Empresa | null) {
    this.dialog.open(EmpresaDialogComponent, { width: '680px', data: e }).afterClosed().subscribe(r => { if (r) { this.snack.open('Empresa guardada'); this.cargar(); } });
  }
}
