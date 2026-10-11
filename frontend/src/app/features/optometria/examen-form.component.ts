import { Component, OnInit, inject, input, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AuthService } from '../../core/auth/auth.service';
import { Cliente, Optometrista } from '../../core/models/api.models';
import { ClientesApi, OptometriaApi } from '../../core/services/api.services';
import { txt } from '../../shared/ui';

@Component({
  selector: 'app-examen-form',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink, MatCardModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatCheckboxModule, MatButtonModule],
  template: `
    <div class="page">
      <div class="page-header">
        <div><h1>Nuevo examen visual</h1>
          @if (cliente(); as c) { <div class="muted">{{ c.apellidos }} {{ c.nombres }} · {{ c.numeroIdentificacion }}</div> }
        </div>
      </div>
      <mat-card><mat-card-content>
        <form [formGroup]="form" (ngSubmit)="guardar()">
          @if (esAdmin) {
            <mat-form-field appearance="outline" class="full">
              <mat-label>Optometrista</mat-label>
              <mat-select formControlName="optometristaId">
                @for (o of optometristas(); track o.id) { <mat-option [value]="o.id">{{ o.nombre }}</mat-option> }
              </mat-select>
              @if (form.controls.optometristaId.hasError('required')) { <mat-error>Seleccione el optometrista que realizó el examen</mat-error> }
            </mat-form-field>
          }
          <mat-form-field appearance="outline" class="full">
            <mat-label>Motivo de la consulta</mat-label>
            <textarea matInput rows="2" formControlName="motivoConsulta" maxlength="500"></textarea>
          </mat-form-field>
          <mat-form-field appearance="outline" class="full">
            <mat-label>Diagnóstico</mat-label>
            <textarea matInput rows="3" formControlName="diagnostico" maxlength="1000"></textarea>
          </mat-form-field>
          <mat-form-field appearance="outline" class="full">
            <mat-label>Observaciones</mat-label>
            <textarea matInput rows="2" formControlName="observaciones" maxlength="1000"></textarea>
          </mat-form-field>
          <mat-form-field appearance="outline" class="full">
            <mat-label>Recomendaciones</mat-label>
            <textarea matInput rows="2" formControlName="recomendaciones" maxlength="1000"></textarea>
          </mat-form-field>
          <mat-checkbox formControlName="continuarReceta">Registrar la receta a continuación</mat-checkbox>
          <div class="toolbar-row" style="margin-top: 16px">
            <button mat-flat-button color="primary" type="submit" [disabled]="guardando()">Guardar examen</button>
            <a mat-button [routerLink]="['/clientes', id()]">Cancelar</a>
          </div>
        </form>
      </mat-card-content></mat-card>
    </div>`
})
export class ExamenFormComponent implements OnInit {
  /** Id del cliente (parámetro de ruta). */
  readonly id = input.required<string>();

  private readonly fb = inject(FormBuilder);
  private readonly clientes = inject(ClientesApi);
  private readonly api = inject(OptometriaApi);
  private readonly auth = inject(AuthService);
  private readonly router = inject(Router);
  private readonly snack = inject(MatSnackBar);

  readonly esAdmin = this.auth.hasRole('ADMIN');
  readonly cliente = signal<Cliente | null>(null);
  readonly optometristas = signal<Optometrista[]>([]);
  readonly guardando = signal(false);

  readonly form = this.fb.group({
    optometristaId: [null as number | null],
    motivoConsulta: [''], diagnostico: [''], observaciones: [''], recomendaciones: [''],
    continuarReceta: [true]
  });

  ngOnInit() {
    this.clientes.obtener(+this.id()).subscribe(c => this.cliente.set(c));
    if (this.esAdmin) {
      this.form.controls.optometristaId.addValidators(Validators.required);
      this.api.optometristas().subscribe(o => this.optometristas.set(o));
    }
  }

  guardar() {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    const v = this.form.getRawValue();
    this.guardando.set(true);
    this.api.crearExamen({
      clienteId: +this.id(), optometristaId: this.esAdmin ? v.optometristaId : null,
      motivoConsulta: txt(v.motivoConsulta), diagnostico: txt(v.diagnostico),
      observaciones: txt(v.observaciones), recomendaciones: txt(v.recomendaciones)
    }).subscribe({
      next: e => {
        this.snack.open('Examen registrado');
        if (v.continuarReceta) this.router.navigate(['/examenes', e.id, 'receta']);
        else this.router.navigate(['/clientes', this.id()]);
      },
      error: () => this.guardando.set(false)
    });
  }
}
