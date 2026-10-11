import { Component, OnDestroy, OnInit, computed, inject, signal } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AuthService } from '../../core/auth/auth.service';
import { BrandService, MARCA_POR_DEFECTO } from '../../core/brand/brand.service';
import { asegurarContrasteConBlanco, esHex } from '../../core/brand/color';

const TIPOS = ['image/png', 'image/jpeg', 'image/webp'];
const MAX_ARCHIVO = 5 * 1024 * 1024;
const MAX_DATA_URL = 380_000; // el servidor acepta hasta 400 000 caracteres

/** Reduce la imagen a un cuadrado máximo de `lado` px y la devuelve como PNG (data URL). */
function reducir(archivo: File, lado: number): Promise<string> {
  return new Promise((ok, fallo) => {
    const url = URL.createObjectURL(archivo);
    const img = new Image();
    img.onload = () => {
      const k = Math.min(1, lado / Math.max(img.width, img.height));
      const c = document.createElement('canvas');
      c.width = Math.max(1, Math.round(img.width * k));
      c.height = Math.max(1, Math.round(img.height * k));
      c.getContext('2d')!.drawImage(img, 0, 0, c.width, c.height);
      URL.revokeObjectURL(url);
      ok(c.toDataURL('image/png'));
    };
    img.onerror = () => { URL.revokeObjectURL(url); fallo(new Error('imagen inválida')); };
    img.src = url;
  });
}

@Component({
  selector: 'app-marca',
  standalone: true,
  imports: [MatCardModule, MatButtonModule, MatIconModule],
  template: `
    <div class="page">
      <div class="page-header"><h1>Identidad visual</h1></div>
      <p class="muted">Personalice el logo y los colores de su empresa. Los cambios se ven al instante y se aplican a todos sus usuarios al guardar.</p>

      <div class="row g-3">
        <div class="col-12 col-lg-6">
          <mat-card><mat-card-content>
            <h3>Logo</h3>
            <div class="logo-fila">
              @if (logo(); as l) { <img class="logo-prev" [src]="l" alt="Logo actual"> } @else { <div class="logo-prev vacio"><mat-icon>image</mat-icon></div> }
              <div class="toolbar-row">
                <label class="mat-mdc-outlined-button mdc-button mat-mdc-button-base archivo">
                  <input type="file" accept="image/png,image/jpeg,image/webp" (change)="elegirArchivo($event)" hidden>
                  <mat-icon>upload</mat-icon> Subir logo
                </label>
                @if (logo()) { <button mat-button (click)="logo.set(null); actualizar()"><mat-icon>delete</mat-icon> Quitar</button> }
              </div>
            </div>
            <p class="muted ayuda">PNG, JPEG o WebP. Se recomienda una imagen cuadrada o circular con fondo transparente. Se reduce automáticamente.</p>

            <h3>Colores</h3>
            <div class="color-fila">
              <label>Color principal
                <span class="entrada"><input type="color" [value]="primarioValido()" (input)="primario.set($any($event.target).value); actualizar()"><code>{{ primarioValido() }}</code></span>
              </label>
              <label>Color de acento
                <span class="entrada"><input type="color" [value]="secundarioValido()" (input)="secundario.set($any($event.target).value); actualizar()"><code>{{ secundarioValido() }}</code></span>
              </label>
            </div>
            @if (ajustado()) { <p class="aviso">Para que el texto blanco sobre botones y menús sea legible, el color principal se aplicará como <strong>{{ efectivo() }}</strong>.</p> }

            <div class="toolbar-row acciones">
              <button mat-flat-button color="primary" (click)="guardar()" [disabled]="guardando()"><mat-icon>save</mat-icon> Guardar</button>
              <button mat-stroked-button (click)="colorPorDefecto()">Colores por defecto</button>
            </div>
          </mat-card-content></mat-card>
        </div>

        <div class="col-12 col-lg-6">
          <mat-card><mat-card-content>
            <h3>Vista previa</h3>
            <div class="previa">
              <div class="lateral">
                @if (logo(); as l) { <img [src]="l" alt=""> } @else { <div class="ini">{{ inicial() }}</div> }
                <span class="item activo"><mat-icon>home</mat-icon> Inicio</span>
                <span class="item"><mat-icon>groups</mat-icon> Clientes</span>
                <span class="item"><mat-icon>point_of_sale</mat-icon> Ventas</span>
              </div>
              <div class="contenido">
                <div class="botones"><button mat-flat-button color="primary">Guardar</button><button mat-stroked-button>Cancelar</button><span class="chip info">Etiqueta</span></div>
                <div class="muestra-acento">Color de acento</div>
              </div>
            </div>
          </mat-card-content></mat-card>
        </div>
      </div>
    </div>`,
  styles: [`
    h3 { margin: 4px 0 12px; font-weight: 500; }
    .logo-fila { display: flex; align-items: center; gap: 18px; flex-wrap: wrap; }
    .logo-prev { width: 96px; height: 96px; border-radius: 50%; object-fit: cover; border: 1px solid var(--borde); background: #fff; }
    .logo-prev.vacio { display: flex; align-items: center; justify-content: center; color: #9AA9B0; background: #F2F6F8; }
    .archivo { display: inline-flex; align-items: center; gap: 6px; cursor: pointer; height: 42px; padding: 0 16px; }
    .ayuda { font-size: .85rem; margin: 10px 0 22px; }
    .color-fila { display: flex; gap: 28px; flex-wrap: wrap; margin-bottom: 8px; }
    .color-fila label { display: flex; flex-direction: column; gap: 6px; color: var(--texto-suave); font-size: .85rem; }
    .entrada { display: inline-flex; align-items: center; gap: 10px; color: #1B2024; }
    input[type=color] { width: 54px; height: 42px; padding: 2px; border: 1px solid var(--borde); border-radius: 10px; background: #fff; cursor: pointer; }
    .aviso { background: var(--brand-light); color: var(--brand-dark); padding: 8px 12px; border-radius: 8px; font-size: .88rem; }
    .acciones { margin-top: 20px; }
    .previa { display: flex; min-height: 220px; border: 1px solid var(--borde); border-radius: var(--radio); overflow: hidden; }
    .lateral { width: 150px; background: var(--sidebar-bg); padding: 14px 10px; display: flex; flex-direction: column; gap: 6px; color: var(--sidebar-text); }
    .lateral img, .lateral .ini { width: 64px; height: 64px; border-radius: 50%; object-fit: cover; margin: 0 auto 8px; }
    .lateral .ini { display: flex; align-items: center; justify-content: center; font-size: 1.4rem; background: var(--brand-accent); color: var(--brand-on-accent); }
    .item { display: flex; align-items: center; gap: 8px; padding: 6px 8px; border-radius: 8px; font-size: .82rem; }
    .item mat-icon { font-size: 18px; width: 18px; height: 18px; color: inherit; }
    .item.activo { background: var(--brand); color: #fff; }
    .contenido { flex: 1; padding: 16px; background: var(--bg); display: flex; flex-direction: column; gap: 14px; }
    .botones { display: flex; gap: 10px; align-items: center; flex-wrap: wrap; }
    .muestra-acento { padding: 14px; border-radius: 10px; background: var(--brand-accent); color: var(--brand-on-accent); font-weight: 500; text-align: center; }
  `]
})
export class MarcaComponent implements OnInit, OnDestroy {
  private readonly auth = inject(AuthService);
  private readonly brand = inject(BrandService);
  private readonly snack = inject(MatSnackBar);

  private readonly inicial0 = this.brand.marca();
  readonly primario = signal(this.inicial0.colorPrimario ?? MARCA_POR_DEFECTO.colorPrimario);
  readonly secundario = signal(this.inicial0.colorSecundario ?? MARCA_POR_DEFECTO.colorSecundario);
  readonly logo = signal<string | null>(this.inicial0.logo ?? null);
  readonly guardando = signal(false);
  private guardado = false;

  readonly primarioValido = computed(() => (esHex(this.primario()) ? this.primario() : MARCA_POR_DEFECTO.colorPrimario).toUpperCase());
  readonly secundarioValido = computed(() => (esHex(this.secundario()) ? this.secundario() : MARCA_POR_DEFECTO.colorSecundario).toUpperCase());
  readonly efectivo = computed(() => asegurarContrasteConBlanco(this.primarioValido()));
  readonly ajustado = computed(() => this.efectivo() !== this.primarioValido());
  readonly inicial = computed(() => (this.auth.session()?.empresa ?? '?')[0]);

  /** El formulario parte de lo guardado en el servidor (no de la caché local, que puede ser de otra sesión). */
  ngOnInit() {
    this.brand.obtener().subscribe(m => {
      this.primario.set(m.colorPrimario ?? MARCA_POR_DEFECTO.colorPrimario);
      this.secundario.set(m.colorSecundario ?? MARCA_POR_DEFECTO.colorSecundario);
      this.logo.set(m.logo ?? null);
    });
  }

  actualizar() {
    this.brand.previsualizar({ colorPrimario: this.primarioValido(), colorSecundario: this.secundarioValido(), logo: this.logo() });
  }

  colorPorDefecto() {
    this.primario.set(MARCA_POR_DEFECTO.colorPrimario);
    this.secundario.set(MARCA_POR_DEFECTO.colorSecundario);
    this.actualizar();
  }

  async elegirArchivo(ev: Event) {
    const input = ev.target as HTMLInputElement;
    const archivo = input.files?.[0];
    input.value = '';
    if (!archivo) return;
    if (!TIPOS.includes(archivo.type)) { this.snack.open('El logo debe ser PNG, JPEG o WebP.'); return; }
    if (archivo.size > MAX_ARCHIVO) { this.snack.open('La imagen supera los 5 MB.'); return; }
    try {
      let data = await reducir(archivo, 320);
      if (data.length > MAX_DATA_URL) data = await reducir(archivo, 200);
      if (data.length > MAX_DATA_URL) { this.snack.open('La imagen es demasiado compleja; use una más simple.'); return; }
      this.logo.set(data);
      this.actualizar();
    } catch {
      this.snack.open('No se pudo leer la imagen.');
    }
  }

  guardar() {
    this.guardando.set(true);
    this.brand.guardar({ colorPrimario: this.primarioValido(), colorSecundario: this.secundarioValido(), logo: this.logo() }).subscribe({
      next: () => { this.guardado = true; this.guardando.set(false); this.snack.open('Identidad visual actualizada'); },
      error: () => this.guardando.set(false)
    });
  }

  /** Si se sale sin guardar, se descarta la vista previa. */
  ngOnDestroy() { if (!this.guardado) this.brand.restaurar(); }
}
