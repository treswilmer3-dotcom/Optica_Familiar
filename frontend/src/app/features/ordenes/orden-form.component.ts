import { Component, inject, input, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBar } from '@angular/material/snack-bar';
import { OrdenesApi } from '../../core/services/api.services';
import { fechaIso, txt } from '../../shared/ui';

@Component({
  selector: 'app-orden-form',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink, MatCardModule, MatFormFieldModule, MatInputModule, MatDatepickerModule, MatButtonModule],
  template: `
    <div class="page">
      <div class="page-header"><h1>Nueva orden de trabajo</h1></div>
      @if (!recetaId()) {
        <mat-card><mat-card-content>La orden se crea desde la receta del cliente: abra el cliente y use «Crear orden de trabajo».
          <div style="margin-top:12px"><a mat-flat-button color="primary" routerLink="/clientes">Ir a clientes</a></div></mat-card-content></mat-card>
      } @else {
        <mat-card><mat-card-content>
          <form [formGroup]="form" (ngSubmit)="guardar(false)">
            <mat-form-field appearance="outline" class="full">
              <mat-label>Fecha de entrega estimada</mat-label>
              <input matInput [matDatepicker]="dp" formControlName="fechaEntregaEstimada" [min]="hoy">
              <mat-datepicker-toggle matIconSuffix [for]="dp" /><mat-datepicker #dp />
            </mat-form-field>
            <mat-form-field appearance="outline" class="full">
              <mat-label>Observaciones (tipo de lente, tratamiento, montura…)</mat-label>
              <textarea matInput rows="3" formControlName="observaciones" maxlength="1000"></textarea>
            </mat-form-field>
            <div class="toolbar-row">
              <button mat-flat-button color="primary" type="submit" [disabled]="guardando()">Guardar orden</button>
              <button mat-stroked-button color="primary" type="button" [disabled]="guardando()" (click)="guardar(true)">Guardar y facturar</button>
              <a mat-button [routerLink]="clienteId() ? ['/clientes', clienteId()] : ['/ordenes']">Cancelar</a>
            </div>
          </form>
        </mat-card-content></mat-card>
      }
    </div>`
})
export class OrdenFormComponent {
  /** Query params ?recetaId=&clienteId= */
  readonly recetaId = input<string>();
  readonly clienteId = input<string>();

  private readonly fb = inject(FormBuilder);
  private readonly api = inject(OrdenesApi);
  private readonly router = inject(Router);
  private readonly snack = inject(MatSnackBar);

  readonly hoy = new Date();
  readonly guardando = signal(false);
  readonly form = this.fb.group({ fechaEntregaEstimada: [null as Date | null], observaciones: [''] });

  guardar(facturar: boolean) {
    const v = this.form.getRawValue();
    this.guardando.set(true);
    this.api.crear({ recetaId: +this.recetaId()!, fechaEntregaEstimada: fechaIso(v.fechaEntregaEstimada), observaciones: txt(v.observaciones) }).subscribe({
      next: o => {
        this.snack.open(`Orden ${o.numeroOrden} creada`);
        if (facturar) this.router.navigate(['/ventas/nueva'], { queryParams: { clienteId: this.clienteId(), ordenId: o.id } });
        else this.router.navigate(this.clienteId() ? ['/clientes', this.clienteId()] : ['/ordenes']);
      },
      error: () => this.guardando.set(false)
    });
  }
}
