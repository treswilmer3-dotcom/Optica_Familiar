import { Injectable, computed, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, of, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Marca } from '../models/api.models';
import { aclarar, asegurarContrasteConBlanco, esHex, mezclar, oscurecer, textoSobre } from './color';
import { TOKENS_PRIMARIO, TOKENS_PRIMARIO_CLARO, TOKENS_PRIMARIO_POR_SELECTOR } from './material-tokens';

export const MARCA_POR_DEFECTO = { colorPrimario: '#1F7391', colorSecundario: '#4494AC', logo: '/brand/optica-familiar.png' };
const CACHE = 'of_marca';

/**
 * Identidad visual de la empresa: aplica los colores como variables CSS (incluidas las de Angular Material)
 * y expone el logo. Se recuerda la última marca para que la pantalla de ingreso conserve la identidad.
 */
@Injectable({ providedIn: 'root' })
export class BrandService {
  private readonly http = inject(HttpClient);

  private readonly _marca = signal<Marca>(this.leerCache() ?? { ...MARCA_POR_DEFECTO });
  readonly marca = this._marca.asReadonly();
  readonly logo = computed(() => this._marca().logo ?? null);

  constructor() {
    this.aplicar(this._marca());
  }

  /** Descarga la marca de la empresa autenticada y la aplica. */
  cargar() {
    return this.http.get<Marca>(`${environment.apiUrl}/empresas/actual/marca`).pipe(
      catchError(() => of(null)),
      tap(m => { if (m) this.fijar(m); })
    ).subscribe();
  }

  /** Lee la marca guardada en el servidor sin modificar la vigente (para formularios de edición). */
  obtener() {
    return this.http.get<Marca>(`${environment.apiUrl}/empresas/actual/marca`);
  }

  guardar(m: Marca) {
    return this.http.put<Marca>(`${environment.apiUrl}/empresas/actual/marca`, m).pipe(tap(r => this.fijar(r)));
  }

  /** Aplica una marca sin guardarla (vista previa). */
  previsualizar(m: Marca) { this.aplicar(m); }

  /** Descarta una vista previa y vuelve a la marca vigente. */
  restaurar() { this.aplicar(this._marca()); }

  private fijar(m: Marca) {
    // Sin ningún dato de marca (empresa nueva) se usan los colores de la plataforma, sin logo.
    const marca: Marca = { colorPrimario: m.colorPrimario ?? null, colorSecundario: m.colorSecundario ?? null, logo: m.logo ?? null };
    this._marca.set(marca);
    try { localStorage.setItem(CACHE, JSON.stringify(marca)); } catch { /* almacenamiento no disponible */ }
    this.aplicar(marca);
  }

  private aplicar(m: Marca) {
    const root = document.documentElement.style;
    const base = esHex(m.colorPrimario) ? m.colorPrimario : MARCA_POR_DEFECTO.colorPrimario;
    const primario = asegurarContrasteConBlanco(base);
    const acento = esHex(m.colorSecundario) ? m.colorSecundario.toUpperCase() : aclarar(primario, 0.25);
    const claro = aclarar(primario, 0.88);

    root.setProperty('--brand', primario);
    root.setProperty('--brand-dark', oscurecer(primario, 0.22));
    root.setProperty('--brand-light', claro);
    root.setProperty('--brand-accent', acento);
    root.setProperty('--brand-on-accent', textoSobre(acento));
    root.setProperty('--sidebar-bg', mezclar('#1B2024', primario, 0.1));
    TOKENS_PRIMARIO.forEach(t => root.setProperty(t, primario));
    TOKENS_PRIMARIO_CLARO.forEach(t => root.setProperty(t, claro));
    this.hojaPorSelector(primario);
    document.querySelector('meta[name="theme-color"]')?.setAttribute('content', primario);
  }

  /** Material define algunas variables en reglas con clase (.mat-primary…); se replican con el color de la marca. */
  private hojaPorSelector(primario: string) {
    let hoja = document.getElementById('brand-scoped') as HTMLStyleElement | null;
    if (!hoja) {
      hoja = document.createElement('style');
      hoja.id = 'brand-scoped';
      document.head.appendChild(hoja);
    }
    hoja.textContent = Object.entries(TOKENS_PRIMARIO_POR_SELECTOR)
      .map(([sel, props]) => `${sel}{${props.map(p => `${p}:${primario}`).join(';')}}`).join('\n');
  }

  private leerCache(): Marca | null {
    try { const raw = localStorage.getItem(CACHE); return raw ? JSON.parse(raw) as Marca : null; } catch { return null; }
  }
}
