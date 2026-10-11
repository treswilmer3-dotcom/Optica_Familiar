import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { Rol } from '../models/api.models';
import { AuthService } from './auth.service';

export const authGuard: CanActivateFn = (_route, state) => {
  const auth = inject(AuthService);
  if (auth.sesionVigente) return true;
  auth.logout(false);
  return inject(Router).createUrlTree(['/login'], { queryParams: { returnUrl: state.url } });
};

/** Restringe una ruta a los roles indicados; si no cumple, vuelve al inicio. */
export const roleGuard = (...roles: Rol[]): CanActivateFn => () => {
  const auth = inject(AuthService);
  return auth.hasRole(...roles) ? true : inject(Router).createUrlTree(['/inicio']);
};
