import { HttpContextToken, HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { catchError, throwError } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ProblemDetails } from '../models/api.models';
import { AuthService } from './auth.service';

/** Úsese en una petición para que el interceptor no muestre el mensaje de error. */
export const SILENT = new HttpContextToken<boolean>(() => false);

export function mensajeDeError(e: HttpErrorResponse): string {
  if (e.status === 0) return 'No hay conexión con el servidor.';
  const p = e.error as ProblemDetails | null;
  if (p?.errors) {
    const primero = Object.values(p.errors).flat()[0];
    if (primero) return primero;
  }
  if (p?.detail) return p.detail;
  if (e.status === 403) return 'No tiene permiso para realizar esta acción.';
  if (e.status === 429) return 'Demasiados intentos. Espere un momento.';
  if (e.status >= 500) return 'Error interno del servidor. Intente nuevamente.';
  return p?.title ?? 'Ocurrió un error inesperado.';
}

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const auth = inject(AuthService);
  const router = inject(Router);
  const snack = inject(MatSnackBar);

  const esApi = req.url.startsWith(environment.apiUrl);
  const esLogin = req.url.endsWith('/auth/login');
  const token = auth.token;
  const conToken = esApi && token ? req.clone({ setHeaders: { Authorization: `Bearer ${token}` } }) : req;

  return next(conToken).pipe(
    catchError((e: HttpErrorResponse) => {
      if (e.status === 401 && !esLogin) {
        auth.logout(false);
        router.navigate(['/login'], { queryParams: { returnUrl: router.url } });
        snack.open('Su sesión expiró. Inicie sesión nuevamente.', 'Cerrar', { duration: 5000 });
      } else if (!req.context.get(SILENT) && !(esLogin && e.status === 401)) {
        snack.open(mensajeDeError(e), 'Cerrar', { duration: 6000 });
      }
      return throwError(() => e);
    })
  );
};
