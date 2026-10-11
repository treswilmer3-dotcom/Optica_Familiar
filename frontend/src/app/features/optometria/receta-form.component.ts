import { Component, OnInit, inject, input, signal } from '@angular/core';
import { AbstractControl, FormBuilder, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ExamenVisual } from '../../core/models/api.models';
import { OptometriaApi } from '../../core/services/api.services';
import { num, txt } from '../../shared/ui';

/** Convención óptica: si hay cilindro, el eje es obligatorio (por ojo). */
function cilindroRequiereEje(g: AbstractControl): ValidationErrors | null {
  const v = g.value;
  const err: ValidationErrors = {};
  if (num(v.odCilindro) !== null && num(v.odCilindro) !== 0 && num(v.odEje) === null) err['odEje'] = true;
  if (num(v.oiCilindro) !== null && num(v.oiCilindro) !== 0 && num(v.oiEje) === null) err['oiEje'] = true;
  return Object.keys(err).length ? err : null;
}

@Component({
  selector: 'app-receta-form',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink, MatCardModule, MatFormFieldModule, MatInputModule, MatButtonModule],
  template: `
    <div class="page">
      <div class="page-header">
        <div><h1>Receta</h1>
          @if (examen(); as e) { <div class="muted">{{ e.clienteNombre }} · Historia {{ e.numeroHistoria }}</div> }
        </div>
      </div>
      <mat-card><mat-card-content>
        <form [formGroup]="form" (ngSubmit)="guardar()">
          @for (ojo of ojos; track ojo.pref) {
            <h3>{{ ojo.titulo }}</h3>
            <div class="row g-2">
              <div class="col-6 col-md-3"><mat-form-field appearance="outline" class="full"><mat-label>Esfera</mat-label>
                <input matInput type="number" step="0.25" [formControlName]="ojo.pref + 'Esfera'"><mat-error>-30 a 30</mat-error></mat-form-field></div>
              <div class="col-6 col-md-3"><mat-form-field appearance="outline" class="full"><mat-label>Cilindro</mat-label>
                <input matInput type="number" step="0.25" [formControlName]="ojo.pref + 'Cilindro'"><mat-error>-10 a 10</mat-error></mat-form-field></div>
              <div class="col-6 col-md-3"><mat-form-field appearance="outline" class="full"><mat-label>Eje (°)</mat-label>
                <input matInput type="number" step="1" [formControlName]="ojo.pref + 'Eje'"><mat-error>0 a 180</mat-error>
                @if (form.errors?.[ojo.pref + 'Eje']) { <mat-hint class="falta">Requerido si hay cilindro</mat-hint> }</mat-form-field></div>
              <div class="col-6 col-md-3"><mat-form-field appearance="outline" class="full"><mat-label>Adición</mat-label>
                <input matInput type="number" step="0.25" [formControlName]="ojo.pref + 'Adicion'"><mat-error>0 a 5</mat-error></mat-form-field></div>
            </div>
          }
          <div class="row g-2">
            <div class="col-12 col-md-3"><mat-form-field appearance="outline" class="full"><mat-label>Distancia pupilar (mm)</mat-label>
              <input matInput type="number" step="0.5" formControlName="distanciaPupilar"><mat-error>20 a 100</mat-error></mat-form-field></div>
            <div class="col-12 col-md-9"><mat-form-field appearance="outline" class="full"><mat-label>Observación</mat-label>
              <input matInput formControlName="observacion" maxlength="1000"></mat-form-field></div>
          </div>
          <div class="toolbar-row">
            <button mat-flat-button color="primary" type="submit" [disabled]="guardando()">Guardar receta</button>
            @if (examen(); as e) { <a mat-button [routerLink]="['/clientes', e.clienteId]">Cancelar</a> }
          </div>
        </form>
      </mat-card-content></mat-card>
    </div>`,
  styles: [`h3 { margin: 8px 0 12px; font-weight: 500; } .falta { color: #b71c1c; }`]
})
export class RecetaFormComponent implements OnInit {
  /** Id del examen visual (parámetro de ruta). */
  readonly examenId = input.required<string>();

  private readonly fb = inject(FormBuilder);
  private readonly api = inject(OptometriaApi);
  private readonly router = inject(Router);
  private readonly snack = inject(MatSnackBar);

  readonly ojos = [{ pref: 'od', titulo: 'Ojo derecho (OD)' }, { pref: 'oi', titulo: 'Ojo izquierdo (OI)' }];
  readonly examen = signal<ExamenVisual | null>(null);
  readonly guardando = signal(false);

  readonly form = this.fb.group({
    odEsfera: [null as number | null, [Validators.min(-30), Validators.max(30)]],
    odCilindro: [null as number | null, [Validators.min(-10), Validators.max(10)]],
    odEje: [null as number | null, [Validators.min(0), Validators.max(180)]],
    odAdicion: [null as number | null, [Validators.min(0), Validators.max(5)]],
    oiEsfera: [null as number | null, [Validators.min(-30), Validators.max(30)]],
    oiCilindro: [null as number | null, [Validators.min(-10), Validators.max(10)]],
    oiEje: [null as number | null, [Validators.min(0), Validators.max(180)]],
    oiAdicion: [null as number | null, [Validators.min(0), Validators.max(5)]],
    distanciaPupilar: [null as number | null, [Validators.min(20), Validators.max(100)]],
    observacion: ['']
  }, { validators: cilindroRequiereEje });

  ngOnInit() {
    this.api.obtenerExamen(+this.examenId()).subscribe(e => {
      this.examen.set(e);
      if (e.receta) this.router.navigate(['/clientes', e.clienteId]);
    });
  }

  guardar() {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    const v = this.form.getRawValue();
    const hayValores = [v.odEsfera, v.odCilindro, v.oiEsfera, v.oiCilindro, v.odAdicion, v.oiAdicion].some(x => num(x) !== null);
    if (!hayValores) { this.snack.open('Ingrese al menos un valor de graduación.'); return; }
    this.guardando.set(true);
    this.api.crearReceta({
      consultaId: +this.examenId(),
      odEsfera: num(v.odEsfera), odCilindro: num(v.odCilindro), odEje: num(v.odEje), odAdicion: num(v.odAdicion),
      oiEsfera: num(v.oiEsfera), oiCilindro: num(v.oiCilindro), oiEje: num(v.oiEje), oiAdicion: num(v.oiAdicion),
      distanciaPupilar: num(v.distanciaPupilar), observacion: txt(v.observacion)
    }).subscribe({
      next: () => { this.snack.open('Receta registrada'); this.router.navigate(['/clientes', this.examen()!.clienteId]); },
      error: () => this.guardando.set(false)
    });
  }
}
