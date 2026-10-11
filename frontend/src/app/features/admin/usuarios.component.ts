import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
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
import { Empresa, RolDto, Sucursal, Usuario } from '../../core/models/api.models';
import { AdminApi } from '../../core/services/api.services';
import { PasswordDialogComponent } from '../../shared/password-dialog.component';
import { claseEstado, etiquetaEstado, txt } from '../../shared/ui';

interface UsuarioDialogData { usuario: Usuario | null; roles: RolDto[]; sucursales: Sucursal[]; empresas: Empresa[]; superadmin: boolean; }

@Component({
  selector: 'app-usuario-dialog',
  standalone: true,
  imports: [ReactiveFormsModule, MatDialogModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatButtonModule],
  template: `
    <h2 mat-dialog-title>{{ data.usuario ? 'Editar usuario ' + data.usuario.username : 'Nuevo usuario' }}</h2>
    <form [formGroup]="form" (ngSubmit)="guardar()">
      <mat-dialog-content>
        <div class="form-grid">
          @if (data.superadmin && !data.usuario) {
            <mat-form-field appearance="outline" class="span-2"><mat-label>Empresa</mat-label>
              <mat-select formControlName="empresaId">@for (e of data.empresas; track e.id) { <mat-option [value]="e.id">{{ e.nombreComercial }} ({{ e.codigo }})</mat-option> }</mat-select>
              <mat-error>Requerida</mat-error></mat-form-field>
          }
          @if (!data.usuario) {
            <mat-form-field appearance="outline"><mat-label>Usuario</mat-label><input matInput formControlName="username" maxlength="50" autocomplete="off"><mat-error>Requerido</mat-error></mat-form-field>
            <mat-form-field appearance="outline"><mat-label>Contraseña (mín. 8)</mat-label><input matInput type="password" formControlName="password" autocomplete="new-password"><mat-error>Mínimo 8 caracteres</mat-error></mat-form-field>
            <mat-form-field appearance="outline"><mat-label>Nombres</mat-label><input matInput formControlName="nombres" maxlength="100"><mat-error>Requerido</mat-error></mat-form-field>
            <mat-form-field appearance="outline"><mat-label>Apellidos</mat-label><input matInput formControlName="apellidos" maxlength="100"><mat-error>Requerido</mat-error></mat-form-field>
            <mat-form-field appearance="outline"><mat-label>Identificación</mat-label><input matInput formControlName="numeroIdentificacion" maxlength="20"></mat-form-field>
            <mat-form-field appearance="outline"><mat-label>Celular</mat-label><input matInput formControlName="celular" maxlength="20"></mat-form-field>
            <mat-form-field appearance="outline" class="span-2"><mat-label>Correo</mat-label><input matInput type="email" formControlName="correo" maxlength="100"><mat-error>Correo inválido</mat-error></mat-form-field>
          }
          <mat-form-field appearance="outline"><mat-label>Rol</mat-label>
            <mat-select formControlName="rolId">@for (r of data.roles; track r.id) { <mat-option [value]="r.id">{{ r.nombre }}</mat-option> }</mat-select><mat-error>Requerido</mat-error></mat-form-field>
          <mat-form-field appearance="outline"><mat-label>Sucursal</mat-label>
            <mat-select formControlName="sucursalId">@for (s of sucursalesDisponibles(); track s.id) { <mat-option [value]="s.id">{{ s.nombre }}</mat-option> }</mat-select><mat-error>Requerida</mat-error></mat-form-field>
          @if (data.usuario) {
            <mat-form-field appearance="outline"><mat-label>Estado</mat-label>
              <mat-select formControlName="estado"><mat-option value="ACTIVO">Activo</mat-option><mat-option value="INACTIVO">Inactivo</mat-option></mat-select></mat-form-field>
          }
        </div>
      </mat-dialog-content>
      <mat-dialog-actions align="end">
        <button mat-button type="button" mat-dialog-close>Cancelar</button>
        <button mat-flat-button color="primary" type="submit" [disabled]="guardando()">Guardar</button>
      </mat-dialog-actions>
    </form>`
})
export class UsuarioDialogComponent {
  readonly data = inject<UsuarioDialogData>(MAT_DIALOG_DATA);
  private readonly fb = inject(FormBuilder);
  private readonly api = inject(AdminApi);
  private readonly ref = inject(MatDialogRef<UsuarioDialogComponent>);
  readonly guardando = signal(false);

  readonly form = this.fb.group({
    empresaId: [null as number | null], username: [''], password: [''], nombres: [''], apellidos: [''],
    numeroIdentificacion: [''], celular: [''], correo: ['', Validators.email],
    rolId: [null as number | null, Validators.required], sucursalId: [null as number | null, Validators.required], estado: ['ACTIVO']
  });

  private readonly empresaSel = signal<number | null>(null);
  readonly sucursalesDisponibles = computed(() => {
    const u = this.data.usuario;
    const empresa = u ? u.empresaId : this.data.superadmin ? this.empresaSel() : null;
    return this.data.sucursales.filter(s => empresa === null || s.empresaId === empresa);
  });

  constructor() {
    const u = this.data.usuario;
    if (u) {
      this.form.patchValue({ rolId: u.rolId, sucursalId: u.sucursalId, estado: u.estado ?? 'ACTIVO' });
    } else {
      const c = this.form.controls;
      [c.username, c.nombres, c.apellidos].forEach(x => x.addValidators(Validators.required));
      c.password.addValidators([Validators.required, Validators.minLength(8)]);
      if (this.data.superadmin) {
        c.empresaId.addValidators(Validators.required);
        c.empresaId.valueChanges.subscribe(v => { this.empresaSel.set(v); c.sucursalId.reset(); });
      }
    }
  }

  guardar() {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    const v = this.form.getRawValue();
    this.guardando.set(true);
    const u = this.data.usuario;
    const obs = u
      ? this.api.actualizarUsuario(u.id, { rolId: v.rolId!, sucursalId: v.sucursalId!, estado: v.estado! })
      : this.api.crearUsuario({
          empresaId: this.data.superadmin ? v.empresaId : null, username: v.username!.trim(), password: v.password!,
          rolId: v.rolId!, sucursalId: v.sucursalId!, nombres: v.nombres!.trim(), apellidos: v.apellidos!.trim(),
          numeroIdentificacion: txt(v.numeroIdentificacion), celular: txt(v.celular), correo: txt(v.correo)
        });
    obs.subscribe({ next: r => this.ref.close(r), error: () => this.guardando.set(false) });
  }
}

@Component({
  selector: 'app-usuarios',
  standalone: true,
  imports: [DatePipe, MatTableModule, MatButtonModule, MatIconModule],
  template: `
    <div class="page">
      <div class="page-header">
        <h1>Usuarios</h1>
        <button mat-flat-button color="primary" (click)="abrir(null)"><mat-icon>person_add</mat-icon> Nuevo usuario</button>
      </div>
      <div class="table-wrap tabla-cards">
        <table mat-table [dataSource]="usuarios()" class="full">
          @if (auth.hasRole('SUPERADMIN')) {
            <ng-container matColumnDef="empresa"><th mat-header-cell *matHeaderCellDef>Empresa</th><td mat-cell *matCellDef="let u" data-label="Empresa">{{ nombreEmpresa(u.empresaId) }}</td></ng-container>
          }
          <ng-container matColumnDef="usuario"><th mat-header-cell *matHeaderCellDef>Usuario</th><td mat-cell *matCellDef="let u" data-label="Usuario"><strong>{{ u.username }}</strong><div class="muted">{{ u.nombreCompleto }}</div></td></ng-container>
          <ng-container matColumnDef="rol"><th mat-header-cell *matHeaderCellDef>Rol</th><td mat-cell *matCellDef="let u" data-label="Rol">{{ u.rol }}</td></ng-container>
          <ng-container matColumnDef="sucursal"><th mat-header-cell *matHeaderCellDef>Sucursal</th><td mat-cell *matCellDef="let u" data-label="Sucursal">{{ u.sucursal }}</td></ng-container>
          <ng-container matColumnDef="acceso"><th mat-header-cell *matHeaderCellDef>Último acceso</th><td mat-cell *matCellDef="let u" data-label="Último acceso">{{ u.ultimoAcceso ? (u.ultimoAcceso | date: 'dd/MM/yyyy HH:mm') : 'Nunca' }}</td></ng-container>
          <ng-container matColumnDef="estado"><th mat-header-cell *matHeaderCellDef>Estado</th><td mat-cell *matCellDef="let u" data-label="Estado"><span class="chip" [class]="claseEstado(u.estado)">{{ etiqueta(u.estado) }}</span></td></ng-container>
          <ng-container matColumnDef="acciones"><th mat-header-cell *matHeaderCellDef></th>
            <td mat-cell *matCellDef="let u" data-label="" class="right">
              <button mat-icon-button (click)="abrir(u)" aria-label="Editar"><mat-icon>edit</mat-icon></button>
              @if (u.id !== auth.session()?.usuarioId) { <button mat-icon-button (click)="clave(u)" aria-label="Restablecer contraseña"><mat-icon>key</mat-icon></button> }
            </td></ng-container>
          <tr mat-header-row *matHeaderRowDef="columnas"></tr><tr mat-row *matRowDef="let row; columns: columnas"></tr>
        </table>
        @if (usuarios().length === 0) { <div class="empty">No hay usuarios.</div> }
      </div>
    </div>`
})
export class UsuariosComponent implements OnInit {
  readonly auth = inject(AuthService);
  private readonly api = inject(AdminApi);
  private readonly dialog = inject(MatDialog);
  private readonly snack = inject(MatSnackBar);

  readonly columnas = this.auth.hasRole('SUPERADMIN') ? ['empresa', 'usuario', 'rol', 'sucursal', 'acceso', 'estado', 'acciones'] : ['usuario', 'rol', 'sucursal', 'acceso', 'estado', 'acciones'];
  readonly claseEstado = claseEstado;
  readonly etiqueta = etiquetaEstado;
  readonly usuarios = signal<Usuario[]>([]);
  readonly roles = signal<RolDto[]>([]);
  readonly sucursales = signal<Sucursal[]>([]);
  readonly empresas = signal<Empresa[]>([]);

  ngOnInit() {
    this.cargar();
    this.api.roles().subscribe(r => this.roles.set(r));
    this.api.sucursales().subscribe(s => this.sucursales.set(s));
    if (this.auth.hasRole('SUPERADMIN')) this.api.empresas().subscribe(e => this.empresas.set(e));
  }
  private cargar() { this.api.usuarios().subscribe(u => this.usuarios.set(u)); }
  nombreEmpresa(id: number) { return this.empresas().find(e => e.id === id)?.nombreComercial ?? `#${id}`; }

  abrir(u: Usuario | null) {
    this.dialog.open(UsuarioDialogComponent, { width: '600px', data: { usuario: u, roles: this.roles(), sucursales: this.sucursales(), empresas: this.empresas(), superadmin: this.auth.hasRole('SUPERADMIN') } })
      .afterClosed().subscribe(r => { if (r) { this.snack.open('Usuario guardado'); this.cargar(); } });
  }

  clave(u: Usuario) { this.dialog.open(PasswordDialogComponent, { width: '420px', data: { usuarioId: u.id, propio: false, nombre: u.username } }); }
}
