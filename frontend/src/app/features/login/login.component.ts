import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { AuthService } from '../../core/auth/auth.service';

const CODIGO_KEY = 'of_codigo_empresa';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, MatCardModule, MatFormFieldModule, MatInputModule, MatButtonModule, MatIconModule, MatProgressSpinnerModule],
  template: `
    <div class="fondo">
      <mat-card class="tarjeta">
        <div class="logo"><mat-icon>visibility</mat-icon></div>
        <h1>Óptica Familiar</h1>
        <p class="muted">Ingrese con su código de empresa</p>
        <form [formGroup]="form" (ngSubmit)="entrar()">
          <mat-form-field appearance="outline" class="full">
            <mat-label>Código de empresa</mat-label>
            <input matInput formControlName="codigoEmpresa" autocomplete="organization" autocapitalize="characters">
            @if (form.controls.codigoEmpresa.hasError('required')) { <mat-error>Requerido</mat-error> }
          </mat-form-field>
          <mat-form-field appearance="outline" class="full">
            <mat-label>Usuario</mat-label>
            <input matInput formControlName="username" autocomplete="username">
            @if (form.controls.username.hasError('required')) { <mat-error>Requerido</mat-error> }
          </mat-form-field>
          <mat-form-field appearance="outline" class="full">
            <mat-label>Contraseña</mat-label>
            <input matInput [type]="ver() ? 'text' : 'password'" formControlName="password" autocomplete="current-password">
            <button mat-icon-button matSuffix type="button" (click)="ver.set(!ver())" [attr.aria-label]="ver() ? 'Ocultar' : 'Mostrar'">
              <mat-icon>{{ ver() ? 'visibility_off' : 'visibility' }}</mat-icon>
            </button>
            @if (form.controls.password.hasError('required')) { <mat-error>Requerido</mat-error> }
          </mat-form-field>
          @if (error()) { <p class="error">{{ error() }}</p> }
          <button mat-flat-button color="primary" class="full" type="submit" [disabled]="cargando()">
            @if (cargando()) { <mat-spinner diameter="20" /> } @else { Ingresar }
          </button>
        </form>
      </mat-card>
    </div>`,
  styles: [`
    .fondo { min-height: 100vh; display: flex; align-items: center; justify-content: center; background: linear-gradient(135deg, #e3f2fd, #f5f6fa); padding: 16px; }
    .tarjeta { width: 100%; max-width: 400px; padding: 28px 24px; text-align: center; }
    .logo mat-icon { font-size: 48px; width: 48px; height: 48px; color: #1565c0; }
    h1 { margin: 4px 0 0; font-weight: 500; }
    p { margin: 4px 0 20px; }
    form { text-align: left; }
    .error { color: #b71c1c; margin: 0 0 12px; text-align: center; }
    button[type=submit] { height: 44px; }
  `]
})
export class LoginComponent {
  private readonly fb = inject(FormBuilder);
  private readonly auth = inject(AuthService);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);

  readonly ver = signal(false);
  readonly cargando = signal(false);
  readonly error = signal('');

  readonly form = this.fb.nonNullable.group({
    codigoEmpresa: [localStorage.getItem(CODIGO_KEY) ?? '', Validators.required],
    username: ['', Validators.required],
    password: ['', Validators.required]
  });

  constructor() {
    if (this.auth.sesionVigente) this.router.navigate(['/inicio']);
  }

  entrar() {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    this.cargando.set(true);
    this.error.set('');
    const v = this.form.getRawValue();
    this.auth.login({ ...v, codigoEmpresa: v.codigoEmpresa.trim().toUpperCase() }).subscribe({
      next: () => {
        localStorage.setItem(CODIGO_KEY, v.codigoEmpresa.trim().toUpperCase());
        const destino = this.route.snapshot.queryParamMap.get('returnUrl');
        this.router.navigateByUrl(destino && destino.startsWith('/') && !destino.startsWith('//') ? destino : '/inicio');
      },
      error: e => {
        this.cargando.set(false);
        this.error.set(e.status === 401 ? 'Empresa, usuario o contraseña incorrectos.' : e.status === 429 ? 'Demasiados intentos. Espere un minuto.' : 'No se pudo iniciar sesión.');
      }
    });
  }
}
