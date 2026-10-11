import { Component, OnInit, inject, signal } from '@angular/core';
import { CurrencyPipe } from '@angular/common';
import { FormBuilder, FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDialog, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { debounceTime, distinctUntilChanged } from 'rxjs';
import { AuthService } from '../../core/auth/auth.service';
import { CategoriaProducto, Producto } from '../../core/models/api.models';
import { ProductosApi } from '../../core/services/api.services';
import { txt } from '../../shared/ui';

@Component({
  selector: 'app-producto-dialog',
  standalone: true,
  imports: [ReactiveFormsModule, MatDialogModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatCheckboxModule, MatButtonModule],
  template: `
    <h2 mat-dialog-title>Nuevo producto</h2>
    <form [formGroup]="form" (ngSubmit)="guardar()">
      <mat-dialog-content>
        <div class="form-grid">
          <mat-form-field appearance="outline"><mat-label>Categoría</mat-label>
            <mat-select formControlName="categoriaId">@for (c of categorias(); track c.id) { <mat-option [value]="c.id">{{ c.nombre }}</mat-option> }</mat-select>
            <mat-error>Requerido</mat-error></mat-form-field>
          <mat-form-field appearance="outline"><mat-label>Código</mat-label><input matInput formControlName="codigo" maxlength="50"><mat-error>Requerido</mat-error></mat-form-field>
          <mat-form-field appearance="outline" class="span-2"><mat-label>Nombre</mat-label><input matInput formControlName="nombre" maxlength="200"><mat-error>Requerido</mat-error></mat-form-field>
          <mat-form-field appearance="outline"><mat-label>Costo</mat-label><input matInput type="number" step="0.01" formControlName="costo"></mat-form-field>
          <mat-form-field appearance="outline"><mat-label>Precio de venta</mat-label><input matInput type="number" step="0.01" formControlName="precio"><mat-error>Mayor o igual a 0</mat-error></mat-form-field>
          <mat-form-field appearance="outline" class="span-2"><mat-label>Descripción</mat-label><input matInput formControlName="descripcion" maxlength="1000"></mat-form-field>
        </div>
        <mat-checkbox formControlName="requiereFormula">Requiere fórmula (receta)</mat-checkbox>
      </mat-dialog-content>
      <mat-dialog-actions align="end">
        <button mat-button type="button" mat-dialog-close>Cancelar</button>
        <button mat-flat-button color="primary" type="submit" [disabled]="guardando()">Guardar</button>
      </mat-dialog-actions>
    </form>`
})
export class ProductoDialogComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly api = inject(ProductosApi);
  private readonly ref = inject(MatDialogRef<ProductoDialogComponent>);
  readonly categorias = signal<CategoriaProducto[]>([]);
  readonly guardando = signal(false);
  readonly form = this.fb.group({
    categoriaId: [null as number | null, Validators.required], codigo: ['', Validators.required], nombre: ['', Validators.required],
    costo: [0, [Validators.required, Validators.min(0)]], precio: [0, [Validators.required, Validators.min(0)]],
    descripcion: [''], requiereFormula: [false]
  });
  ngOnInit() { this.api.categorias().subscribe(c => this.categorias.set(c)); }
  guardar() {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    const v = this.form.getRawValue();
    this.guardando.set(true);
    this.api.crear({ categoriaId: v.categoriaId!, codigo: v.codigo!.trim(), nombre: v.nombre!.trim(), costo: v.costo!, precio: v.precio!, descripcion: txt(v.descripcion), requiereFormula: !!v.requiereFormula })
      .subscribe({ next: p => this.ref.close(p), error: () => this.guardando.set(false) });
  }
}

@Component({
  selector: 'app-productos',
  standalone: true,
  imports: [CurrencyPipe, ReactiveFormsModule, MatTableModule, MatButtonModule, MatIconModule, MatFormFieldModule, MatInputModule, MatProgressBarModule],
  template: `
    <div class="page">
      <div class="page-header">
        <h1>Productos</h1>
        @if (auth.hasRole('ADMIN')) { <button mat-flat-button color="primary" (click)="nuevo()"><mat-icon>add</mat-icon> Nuevo producto</button> }
      </div>
      <mat-form-field appearance="outline" class="full">
        <mat-label>Buscar por nombre o código</mat-label><mat-icon matPrefix>search</mat-icon>
        <input matInput [formControl]="buscar" autocomplete="off">
      </mat-form-field>
      @if (cargando()) { <mat-progress-bar mode="indeterminate" /> }
      <div class="table-wrap">
        <table mat-table [dataSource]="productos()" class="full">
          <ng-container matColumnDef="codigo"><th mat-header-cell *matHeaderCellDef>Código</th><td mat-cell *matCellDef="let p">{{ p.codigo }}</td></ng-container>
          <ng-container matColumnDef="nombre"><th mat-header-cell *matHeaderCellDef>Producto</th><td mat-cell *matCellDef="let p"><strong>{{ p.nombre }}</strong><div class="muted">{{ p.descripcion }}</div></td></ng-container>
          <ng-container matColumnDef="categoria"><th mat-header-cell *matHeaderCellDef>Categoría</th><td mat-cell *matCellDef="let p">{{ p.categoria }}</td></ng-container>
          <ng-container matColumnDef="precio"><th mat-header-cell *matHeaderCellDef class="right">Precio</th><td mat-cell *matCellDef="let p" class="right">{{ p.precio | currency: auth.moneda() }}</td></ng-container>
          <tr mat-header-row *matHeaderRowDef="columnas"></tr><tr mat-row *matRowDef="let row; columns: columnas"></tr>
        </table>
        @if (!cargando() && productos().length === 0) { <div class="empty">No hay productos.</div> }
      </div>
    </div>`
})
export class ProductosComponent implements OnInit {
  readonly auth = inject(AuthService);
  private readonly api = inject(ProductosApi);
  private readonly dialog = inject(MatDialog);
  private readonly snack = inject(MatSnackBar);
  readonly columnas = ['codigo', 'nombre', 'categoria', 'precio'];
  readonly buscar = new FormControl('', { nonNullable: true });
  readonly productos = signal<Producto[]>([]);
  readonly cargando = signal(false);

  ngOnInit() {
    this.cargar();
    this.buscar.valueChanges.pipe(debounceTime(300), distinctUntilChanged()).subscribe(() => this.cargar());
  }
  private cargar() {
    this.cargando.set(true);
    this.api.listar(this.buscar.value.trim() || undefined).subscribe({ next: r => { this.productos.set(r); this.cargando.set(false); }, error: () => this.cargando.set(false) });
  }
  nuevo() { this.dialog.open(ProductoDialogComponent, { width: '560px' }).afterClosed().subscribe(p => { if (p) { this.snack.open('Producto creado'); this.cargar(); } }); }
}
