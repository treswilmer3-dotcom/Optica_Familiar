import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AdminApi } from '../core/services/api.services';

export interface PasswordDialogData { usuarioId: number; propio: boolean; nombre?: string; }

@Component({
  selector: 'app-password-dialog',
  standalone: true,
  imports: [ReactiveFormsModule, MatDialogModule, MatFormFieldModule, MatInputModule, MatButtonModule],
  template: `
    <h2 mat-dialog-title>{{ data.propio ? 'Cambiar mi contraseña' : 'Restablecer contraseña' }}</h2>
    <form [formGroup]="form" (ngSubmit)="guardar()">
      <mat-dialog-content>
        @if (!data.propio) { <p class="muted">Usuario: <strong>{{ data.nombre }}</strong></p> }
        @if (data.propio) {
          <mat-form-field appearance="outline" class="full"><mat-label>Contraseña actual</mat-label>
            <input matInput type="password" formControlName="actual" autocomplete="current-password"><mat-error>Requerida</mat-error></mat-form-field>
        }
        <mat-form-field appearance="outline" class="full"><mat-label>Nueva contraseña (mínimo 8 caracteres)</mat-label>
          <input matInput type="password" formControlName="nueva" autocomplete="new-password"><mat-error>Mínimo 8 caracteres</mat-error></mat-form-field>
        <mat-form-field appearance="outline" class="full"><mat-label>Repetir nueva contraseña</mat-label>
          <input matInput type="password" formControlName="repetir" autocomplete="new-password">
          @if (form.hasError('distintas')) { <mat-error>Las contraseñas no coinciden</mat-error> }</mat-form-field>
      </mat-dialog-content>
      <mat-dialog-actions align="end">
        <button mat-button type="button" mat-dialog-close>Cancelar</button>
        <button mat-flat-button color="primary" type="submit" [disabled]="guardando()">Guardar</button>
      </mat-dialog-actions>
    </form>`
})
export class PasswordDialogComponent {
  readonly data = inject<PasswordDialogData>(MAT_DIALOG_DATA);
  private readonly fb = inject(FormBuilder);
  private readonly api = inject(AdminApi);
  private readonly ref = inject(MatDialogRef<PasswordDialogComponent>);
  private readonly snack = inject(MatSnackBar);
  readonly guardando = signal(false);

  readonly form = this.fb.group({
    actual: [''], nueva: ['', [Validators.required, Validators.minLength(8)]], repetir: ['']
  }, { validators: g => g.value.nueva !== g.value.repetir ? { distintas: true } : null });

  constructor() {
    if (this.data.propio) this.form.controls.actual.addValidators(Validators.required);
  }

  guardar() {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    const v = this.form.getRawValue();
    this.guardando.set(true);
    this.api.cambiarPassword(this.data.usuarioId, { passwordActual: this.data.propio ? v.actual : null, passwordNueva: v.nueva! }).subscribe({
      next: () => { this.snack.open('Contraseña actualizada'); this.ref.close(true); },
      error: () => this.guardando.set(false)
    });
  }
}
