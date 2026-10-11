import { Component, computed, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { AuthService } from '../../core/auth/auth.service';
import { Rol } from '../../core/models/api.models';

interface Acceso { titulo: string; detalle: string; icono: string; ruta: string; roles: Rol[]; }

const ACCESOS: Acceso[] = [
  { titulo: 'Nuevo cliente', detalle: 'Registrar un cliente', icono: 'person_add', ruta: '/clientes/nuevo', roles: ['ADMIN', 'VENDEDOR'] },
  { titulo: 'Clientes', detalle: 'Buscar clientes y ver su historial', icono: 'groups', ruta: '/clientes', roles: ['ADMIN', 'VENDEDOR', 'OPTOMETRISTA'] },
  { titulo: 'Nueva venta', detalle: 'Facturar productos y registrar pagos', icono: 'point_of_sale', ruta: '/ventas/nueva', roles: ['ADMIN', 'VENDEDOR'] },
  { titulo: 'Órdenes de trabajo', detalle: 'Seguimiento de lentes en producción', icono: 'build', ruta: '/ordenes', roles: ['ADMIN', 'VENDEDOR', 'OPTOMETRISTA'] },
  { titulo: 'Sucursales', detalle: 'Administrar locales', icono: 'store', ruta: '/admin/sucursales', roles: ['ADMIN', 'SUPERADMIN'] },
  { titulo: 'Usuarios', detalle: 'Administrar usuarios y roles', icono: 'manage_accounts', ruta: '/admin/usuarios', roles: ['ADMIN', 'SUPERADMIN'] },
  { titulo: 'Empresas', detalle: 'Alta y gestión de empresas', icono: 'business', ruta: '/plataforma/empresas', roles: ['SUPERADMIN'] }
];

@Component({
  selector: 'app-inicio',
  standalone: true,
  imports: [RouterLink, MatCardModule, MatIconModule],
  template: `
    <div class="page">
      <div class="page-header"><h1>Hola, {{ auth.session()?.username }}</h1></div>
      <div class="row g-3">
        @for (a of accesos(); track a.ruta) {
          <div class="col-12 col-sm-6 col-lg-4">
            <a [routerLink]="a.ruta" class="enlace">
              <mat-card class="acceso">
                <mat-icon>{{ a.icono }}</mat-icon>
                <div><strong>{{ a.titulo }}</strong><div class="muted">{{ a.detalle }}</div></div>
              </mat-card>
            </a>
          </div>
        }
      </div>
    </div>`,
  styles: [`
    .enlace { text-decoration: none; color: inherit; }
    .acceso { display: flex; flex-direction: row; align-items: center; gap: 16px; padding: 20px; margin: 0; transition: box-shadow .15s; }
    .acceso:hover { box-shadow: 0 4px 14px rgba(0,0,0,.15); }
    mat-icon { font-size: 36px; width: 36px; height: 36px; color: #1565c0; }
  `]
})
export class InicioComponent {
  readonly auth = inject(AuthService);
  readonly accesos = computed(() => ACCESOS.filter(a => this.auth.rol() && a.roles.includes(this.auth.rol()!)));
}
