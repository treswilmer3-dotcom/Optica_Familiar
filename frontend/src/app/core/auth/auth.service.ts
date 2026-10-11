import { Injectable, computed, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, catchError, of, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Empresa, LoginRequest, LoginResponse, Rol } from '../models/api.models';

const STORAGE_KEY = 'of_session';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly router = inject(Router);

  private readonly _session = signal<LoginResponse | null>(this.restore());
  private readonly _empresa = signal<Empresa | null>(null);

  readonly session = this._session.asReadonly();
  readonly empresa = this._empresa.asReadonly();
  readonly isLoggedIn = computed(() => this._session() !== null);
  readonly rol = computed(() => this._session()?.rol as Rol | undefined);
  readonly moneda = computed(() => this._empresa()?.moneda ?? 'USD');
  readonly ivaPorcentaje = computed(() => this._empresa()?.ivaPorcentaje ?? 0);

  constructor() {
    // Se difiere: el interceptor HTTP inyecta este servicio y no puede existir una petición durante su construcción.
    if (this._session()) queueMicrotask(() => this.cargarEmpresa());
  }

  get token(): string | null {
    return this._session()?.token ?? null;
  }

  login(req: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${environment.apiUrl}/auth/login`, req).pipe(
      tap(res => {
        localStorage.setItem(STORAGE_KEY, JSON.stringify(res));
        this._session.set(res);
        this.cargarEmpresa();
      })
    );
  }

  logout(redirect = true): void {
    localStorage.removeItem(STORAGE_KEY);
    this._session.set(null);
    this._empresa.set(null);
    if (redirect) this.router.navigate(['/login']);
  }

  hasRole(...roles: Rol[]): boolean {
    const r = this.rol();
    return !!r && roles.includes(r);
  }

  /** True si hay sesión y el token no ha vencido. */
  get sesionVigente(): boolean {
    const s = this._session();
    return !!s && new Date(s.expiraEn).getTime() > Date.now();
  }

  private cargarEmpresa(): void {
    this.http.get<Empresa>(`${environment.apiUrl}/empresas/actual`).pipe(catchError(() => of(null)))
      .subscribe(e => this._empresa.set(e));
  }

  private restore(): LoginResponse | null {
    try {
      const raw = localStorage.getItem(STORAGE_KEY);
      if (!raw) return null;
      const s = JSON.parse(raw) as LoginResponse;
      if (new Date(s.expiraEn).getTime() <= Date.now()) {
        localStorage.removeItem(STORAGE_KEY);
        return null;
      }
      return s;
    } catch {
      localStorage.removeItem(STORAGE_KEY);
      return null;
    }
  }
}
