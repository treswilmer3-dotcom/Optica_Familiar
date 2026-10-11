import { Component, OnInit, inject, input, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Cliente, ClienteRequest } from '../../core/models/api.models';
import { ClientesApi } from '../../core/services/api.services';
import { fechaIso, txt } from '../../shared/ui';

@Component({
  selector: 'app-cliente-form',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink, MatCardModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatDatepickerModule, MatButtonModule],
  template: `
    <div class="page">
      <div class="page-header"><h1>{{ id() ? 'Editar cliente' : 'Nuevo cliente' }}</h1></div>
      <mat-card>
        <mat-card-content>
          <form [formGroup]="form" (ngSubmit)="guardar()">
            <div class="form-grid">
              <mat-form-field appearance="outline">
                <mat-label>Tipo de identificación</mat-label>
                <mat-select formControlName="tipoIdentificacion">
                  <mat-option value="CEDULA">Cédula</mat-option>
                  <mat-option value="RUC">RUC</mat-option>
                  <mat-option value="PASAPORTE">Pasaporte</mat-option>
                  <mat-option value="OTRO">Otro</mat-option>
                </mat-select>
              </mat-form-field>
              <mat-form-field appearance="outline">
                <mat-label>Número de identificación</mat-label>
                <input matInput formControlName="numeroIdentificacion" maxlength="20">
                @if (f.numeroIdentificacion.hasError('required')) { <mat-error>Requerido</mat-error> }
              </mat-form-field>
              <mat-form-field appearance="outline">
                <mat-label>Nombres</mat-label>
                <input matInput formControlName="nombres" maxlength="100">
                @if (f.nombres.hasError('required')) { <mat-error>Requerido</mat-error> }
              </mat-form-field>
              <mat-form-field appearance="outline">
                <mat-label>Apellidos</mat-label>
                <input matInput formControlName="apellidos" maxlength="100">
                @if (f.apellidos.hasError('required')) { <mat-error>Requerido</mat-error> }
              </mat-form-field>
              <mat-form-field appearance="outline">
                <mat-label>Fecha de nacimiento</mat-label>
                <input matInput [matDatepicker]="dp" formControlName="fechaNacimiento" [max]="hoy">
                <mat-datepicker-toggle matIconSuffix [for]="dp" />
                <mat-datepicker #dp />
              </mat-form-field>
              <mat-form-field appearance="outline">
                <mat-label>Género</mat-label>
                <mat-select formControlName="genero">
                  <mat-option [value]="null">—</mat-option>
                  <mat-option value="F">Femenino</mat-option>
                  <mat-option value="M">Masculino</mat-option>
                  <mat-option value="OTRO">Otro</mat-option>
                </mat-select>
              </mat-form-field>
              <mat-form-field appearance="outline">
                <mat-label>Celular</mat-label>
                <input matInput formControlName="celular" maxlength="20" inputmode="tel">
              </mat-form-field>
              <mat-form-field appearance="outline">
                <mat-label>Teléfono</mat-label>
                <input matInput formControlName="telefono" maxlength="20" inputmode="tel">
              </mat-form-field>
              <mat-form-field appearance="outline">
                <mat-label>Correo</mat-label>
                <input matInput type="email" formControlName="correo" maxlength="100">
                @if (f.correo.hasError('email')) { <mat-error>Correo inválido</mat-error> }
              </mat-form-field>
              <mat-form-field appearance="outline" class="span-2">
                <mat-label>Dirección</mat-label>
                <input matInput formControlName="direccion" maxlength="500">
              </mat-form-field>
              <mat-form-field appearance="outline" class="span-2">
                <mat-label>Observaciones</mat-label>
                <textarea matInput rows="2" formControlName="observaciones" maxlength="1000"></textarea>
              </mat-form-field>
            </div>
            <div class="toolbar-row">
              <button mat-flat-button color="primary" type="submit" [disabled]="guardando()">Guardar</button>
              <a mat-button [routerLink]="id() ? ['/clientes', id()] : ['/clientes']">Cancelar</a>
            </div>
          </form>
        </mat-card-content>
      </mat-card>
    </div>`
})
export class ClienteFormComponent implements OnInit {
  /** Parámetro de ruta :id (ausente al crear). */
  readonly id = input<string>();

  private readonly fb = inject(FormBuilder);
  private readonly api = inject(ClientesApi);
  private readonly router = inject(Router);
  private readonly snack = inject(MatSnackBar);

  readonly hoy = new Date();
  readonly guardando = signal(false);
  readonly form = this.fb.group({
    tipoIdentificacion: ['CEDULA' as string | null],
    numeroIdentificacion: ['', Validators.required],
    nombres: ['', Validators.required],
    apellidos: ['', Validators.required],
    fechaNacimiento: [null as Date | null],
    genero: [null as string | null],
    celular: [''], telefono: [''],
    correo: ['', Validators.email],
    direccion: [''], observaciones: ['']
  });
  get f() { return this.form.controls; }

  ngOnInit() {
    const id = this.id();
    if (id) this.api.obtener(+id).subscribe(c => this.cargar(c));
  }

  private cargar(c: Cliente) {
    this.form.patchValue({ ...c, fechaNacimiento: c.fechaNacimiento ? new Date(c.fechaNacimiento) : null });
  }

  guardar() {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    const v = this.form.getRawValue();
    const req: ClienteRequest = {
      tipoIdentificacion: v.tipoIdentificacion, numeroIdentificacion: v.numeroIdentificacion!.trim(),
      nombres: v.nombres!.trim(), apellidos: v.apellidos!.trim(), fechaNacimiento: fechaIso(v.fechaNacimiento),
      genero: v.genero, celular: txt(v.celular), telefono: txt(v.telefono), correo: txt(v.correo),
      direccion: txt(v.direccion), observaciones: txt(v.observaciones)
    };
    this.guardando.set(true);
    const id = this.id();
    (id ? this.api.actualizar(+id, req) : this.api.crear(req)).subscribe({
      next: c => { this.snack.open('Cliente guardado'); this.router.navigate(['/clientes', c.id]); },
      error: () => this.guardando.set(false)
    });
  }
}
