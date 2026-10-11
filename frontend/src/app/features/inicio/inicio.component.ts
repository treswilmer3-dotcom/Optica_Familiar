import { Component, computed, inject } from '@angular/core';
import { DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { AuthService } from '../../core/auth/auth.service';
import { Rol } from '../../core/models/api.models';

interface Acceso { titulo: string; detalle: string; icono: string; ruta: string; roles: Rol[]; destacado?: boolean; }

const ACCESOS: Acceso[] = [
  { titulo: 'Nueva venta', detalle: 'Facturar productos y registrar pagos', icono: 'point_of_sale', ruta: '/ventas/nueva', roles: ['ADMIN', 'VENDEDOR'], destacado: true },
  { titulo: 'Nuevo cliente', detalle: 'Registrar un cliente', icono: 'person_add', ruta: '/clientes/nuevo', roles: ['ADMIN', 'VENDEDOR'], destacado: true },
  { titulo: 'Clientes', detalle: 'Buscar clientes y ver su historial', icono: 'groups', ruta: '/clientes', roles: ['ADMIN', 'VENDEDOR', 'OPTOMETRISTA'], destacado: true },
  { titulo: 'Órdenes de trabajo', detalle: 'Seguimiento de lentes en producción', icono: 'build', ruta: '/ordenes', roles: ['ADMIN', 'VENDEDOR', 'OPTOMETRISTA'] },
  { titulo: 'Ventas', detalle: 'Historial de facturas y saldos', icono: 'receipt_long', ruta: '/ventas', roles: ['ADMIN', 'VENDEDOR'] },
  { titulo: 'Productos', detalle: 'Catálogo de monturas, lentes y servicios', icono: 'inventory_2', ruta: '/productos', roles: ['ADMIN', 'VENDEDOR', 'OPTOMETRISTA'] },
  { titulo: 'Sucursales', detalle: 'Administrar locales', icono: 'store', ruta: '/admin/sucursales', roles: ['ADMIN', 'SUPERADMIN'] },
  { titulo: 'Usuarios', detalle: 'Usuarios, roles y contraseñas', icono: 'manage_accounts', ruta: '/admin/usuarios', roles: ['ADMIN', 'SUPERADMIN'] },
  { titulo: 'Identidad visual', detalle: 'Logo y colores de su empresa', icono: 'palette', ruta: '/admin/marca', roles: ['ADMIN'] },
  { titulo: 'Empresas', detalle: 'Alta y gestión de empresas', icono: 'business', ruta: '/plataforma/empresas', roles: ['SUPERADMIN'], destacado: true }
];

@Component({
  selector: 'app-inicio',
  standalone: true,
  imports: [RouterLink, DatePipe, MatIconModule],
  template: `
    <div class="page">
      <section class="saludo">
        <div>
          <h1>Hola, {{ auth.session()?.username }}</h1>
          <div class="muted">{{ auth.session()?.empresa }} · {{ hoy | date: 'EEEE d \\'de\\' MMMM yyyy' }}</div>
        </div>
      </section>
      <div class="rejilla">
        @for (a of accesos(); track a.ruta) {
          <a [routerLink]="a.ruta" class="acceso" [class.destacado]="a.destacado">
            <span class="icono"><mat-icon>{{ a.icono }}</mat-icon></span>
            <span class="texto"><strong>{{ a.titulo }}</strong><span>{{ a.detalle }}</span></span>
            <mat-icon class="flecha">chevron_right</mat-icon>
          </a>
        }
      </div>
    </div>`,
  styles: [`
    .saludo { margin-bottom: 20px; }
    .saludo h1 { margin: 0 0 2px; font-size: 1.6rem; font-weight: 500; text-transform: none; }
    .saludo .muted::first-letter { text-transform: uppercase; }
    .rejilla { display: grid; grid-template-columns: repeat(auto-fill, minmax(290px, 1fr)); gap: 14px; }
    .acceso { display: flex; align-items: center; gap: 14px; padding: 16px; background: #fff; border: 1px solid var(--borde); border-radius: var(--radio); text-decoration: none; color: inherit; transition: box-shadow .15s, transform .15s, border-color .15s; }
    .acceso:hover { box-shadow: 0 6px 18px rgba(16,24,40,.1); transform: translateY(-1px); border-color: var(--brand); }
    .icono { display: inline-flex; align-items: center; justify-content: center; flex: 0 0 48px; height: 48px; border-radius: 12px; background: var(--brand-light); color: var(--brand); }
    .destacado .icono { background: var(--brand); color: #fff; }
    .icono mat-icon { color: inherit; }
    .texto { display: flex; flex-direction: column; flex: 1; min-width: 0; }
    .texto span { color: var(--texto-suave); font-size: .85rem; }
    .flecha { color: #A5B2B8; }
  `]
})
export class InicioComponent {
  readonly auth = inject(AuthService);
  readonly hoy = new Date();
  readonly accesos = computed(() => ACCESOS.filter(a => this.auth.rol() && a.roles.includes(this.auth.rol()!)));
}
