import { Component, computed, inject } from '@angular/core';
import { BreakpointObserver } from '@angular/cdk/layout';
import { toSignal } from '@angular/core/rxjs-interop';
import { Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatMenuModule } from '@angular/material/menu';
import { MatDialog } from '@angular/material/dialog';
import { map } from 'rxjs';
import { AuthService } from '../core/auth/auth.service';
import { BrandService } from '../core/brand/brand.service';
import { Rol } from '../core/models/api.models';
import { PasswordDialogComponent } from '../shared/password-dialog.component';

interface NavItem { etiqueta: string; icono: string; ruta: string; roles: Rol[]; grupo: 'operacion' | 'admin'; }

const NAV: NavItem[] = [
  { etiqueta: 'Inicio', icono: 'home', ruta: '/inicio', roles: ['ADMIN', 'VENDEDOR', 'OPTOMETRISTA', 'SUPERADMIN'], grupo: 'operacion' },
  { etiqueta: 'Clientes', icono: 'groups', ruta: '/clientes', roles: ['ADMIN', 'VENDEDOR', 'OPTOMETRISTA'], grupo: 'operacion' },
  { etiqueta: 'Órdenes de trabajo', icono: 'build', ruta: '/ordenes', roles: ['ADMIN', 'VENDEDOR', 'OPTOMETRISTA'], grupo: 'operacion' },
  { etiqueta: 'Ventas', icono: 'point_of_sale', ruta: '/ventas', roles: ['ADMIN', 'VENDEDOR'], grupo: 'operacion' },
  { etiqueta: 'Productos', icono: 'inventory_2', ruta: '/productos', roles: ['ADMIN', 'VENDEDOR', 'OPTOMETRISTA'], grupo: 'operacion' },
  { etiqueta: 'Sucursales', icono: 'store', ruta: '/admin/sucursales', roles: ['ADMIN', 'SUPERADMIN'], grupo: 'admin' },
  { etiqueta: 'Usuarios', icono: 'manage_accounts', ruta: '/admin/usuarios', roles: ['ADMIN', 'SUPERADMIN'], grupo: 'admin' },
  { etiqueta: 'Identidad visual', icono: 'palette', ruta: '/admin/marca', roles: ['ADMIN'], grupo: 'admin' },
  { etiqueta: 'Empresas', icono: 'business', ruta: '/plataforma/empresas', roles: ['SUPERADMIN'], grupo: 'admin' }
];

@Component({
  selector: 'app-shell',
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive, MatSidenavModule, MatToolbarModule, MatListModule, MatIconModule, MatButtonModule, MatMenuModule],
  template: `
    <mat-sidenav-container class="contenedor">
      <mat-sidenav #nav [mode]="movil() ? 'over' : 'side'" [opened]="!movil()" class="lateral">
        <div class="marca">
          @if (brand.logo(); as logo) {
            <img class="logo" [src]="logo" [alt]="auth.session()?.empresa ?? 'Logo'">
          } @else {
            <div class="logo inicial">{{ iniciales() }}</div>
          }
          <div class="nombre-empresa">{{ auth.session()?.empresa }}</div>
        </div>
        <nav class="menu" aria-label="Navegación principal">
          @for (item of operacion(); track item.ruta) {
            <a [routerLink]="item.ruta" routerLinkActive="activo" (click)="movil() && nav.close()">
              <mat-icon>{{ item.icono }}</mat-icon><span>{{ item.etiqueta }}</span>
            </a>
          }
          @if (admin().length) {
            <div class="titulo-grupo">Administración</div>
            @for (item of admin(); track item.ruta) {
              <a [routerLink]="item.ruta" routerLinkActive="activo" (click)="movil() && nav.close()">
                <mat-icon>{{ item.icono }}</mat-icon><span>{{ item.etiqueta }}</span>
              </a>
            }
          }
        </nav>
      </mat-sidenav>

      <mat-sidenav-content>
        <header class="barra">
          @if (movil()) { <button mat-icon-button (click)="nav.toggle()" aria-label="Abrir menú"><mat-icon>menu</mat-icon></button> }
          <div class="titulo">{{ auth.session()?.empresa }}</div>
          <span class="espacio"></span>
          <button class="usuario" mat-button [matMenuTriggerFor]="menu" aria-label="Menú de usuario">
            <span class="avatar">{{ inicialUsuario() }}</span>
            <span class="datos"><strong>{{ auth.session()?.username }}</strong><small>{{ etiquetaRol() }}</small></span>
            <mat-icon>expand_more</mat-icon>
          </button>
          <mat-menu #menu="matMenu" xPosition="before">
            <button mat-menu-item (click)="cambiarPassword()"><mat-icon>key</mat-icon><span>Cambiar contraseña</span></button>
            <button mat-menu-item (click)="salir()"><mat-icon>logout</mat-icon><span>Cerrar sesión</span></button>
          </mat-menu>
        </header>
        <main><router-outlet /></main>
      </mat-sidenav-content>
    </mat-sidenav-container>`,
  styles: [`
    .contenedor { height: 100vh; background: var(--bg); }
    .lateral { width: 264px; background: var(--sidebar-bg); color: var(--sidebar-text); border-right: 0; border-radius: 0; }
    .marca { display: flex; flex-direction: column; align-items: center; gap: 10px; padding: 22px 16px 14px; text-align: center; }
    .logo { width: 104px; height: 104px; border-radius: 50%; object-fit: cover; box-shadow: 0 0 0 3px rgba(255,255,255,.12); background: transparent; }
    .logo.inicial { display: flex; align-items: center; justify-content: center; font-size: 2rem; font-weight: 500; background: var(--brand-accent); color: var(--brand-on-accent); }
    .nombre-empresa { font-weight: 500; letter-spacing: .2px; }
    .menu { display: flex; flex-direction: column; gap: 2px; padding: 8px 12px 24px; }
    .menu a { display: flex; align-items: center; gap: 14px; min-height: 46px; padding: 0 14px; border-radius: 10px; color: var(--sidebar-text); text-decoration: none; transition: background .15s; }
    .menu a:hover { background: rgba(255,255,255,.07); }
    .menu a.activo { background: var(--brand); color: #fff; }
    .menu a mat-icon { color: inherit; opacity: .9; }
    .titulo-grupo { margin: 16px 14px 4px; font-size: .72rem; text-transform: uppercase; letter-spacing: .08em; color: rgba(220,229,233,.55); }
    .barra { position: sticky; top: 0; z-index: 5; display: flex; align-items: center; gap: 8px; height: 60px; padding: 0 16px; background: #fff; border-bottom: 1px solid var(--borde); }
    .titulo { font-weight: 500; font-size: 1.05rem; }
    .espacio { flex: 1; }
    .usuario { height: 46px; }
    .usuario .avatar { display: inline-flex; align-items: center; justify-content: center; width: 32px; height: 32px; margin-right: 10px; border-radius: 50%; background: var(--brand); color: #fff; font-weight: 500; text-transform: uppercase; }
    .usuario .datos { display: inline-flex; flex-direction: column; align-items: flex-start; line-height: 1.15; text-align: left; }
    .usuario small { color: var(--texto-suave); font-weight: 400; }
    main { min-height: calc(100vh - 60px); }
    @media (max-width: 520px) { .usuario .datos, .usuario mat-icon { display: none; } .usuario .avatar { margin-right: 0; } .titulo { max-width: 52vw; overflow: hidden; text-overflow: ellipsis; white-space: nowrap; } }
  `]
})
export class ShellComponent {
  readonly auth = inject(AuthService);
  readonly brand = inject(BrandService);
  private readonly router = inject(Router);
  private readonly bp = inject(BreakpointObserver);
  private readonly dialog = inject(MatDialog);

  readonly movil = toSignal(this.bp.observe('(max-width: 900px)').pipe(map(r => r.matches)), { initialValue: false });
  private readonly visibles = computed(() => NAV.filter(i => this.auth.rol() && i.roles.includes(this.auth.rol()!)));
  readonly operacion = computed(() => this.visibles().filter(i => i.grupo === 'operacion'));
  readonly admin = computed(() => this.visibles().filter(i => i.grupo === 'admin'));
  readonly iniciales = computed(() => (this.auth.session()?.empresa ?? '?').split(/\s+/).filter(Boolean).slice(0, 2).map(p => p[0]).join(''));
  readonly inicialUsuario = computed(() => (this.auth.session()?.username ?? '?')[0]);
  readonly etiquetaRol = computed(() => ({
    SUPERADMIN: 'Super administrador', ADMIN: 'Administrador', VENDEDOR: 'Vendedor', OPTOMETRISTA: 'Optometrista'
  } as Record<string, string>)[this.auth.rol() ?? ''] ?? '');

  cambiarPassword() {
    this.dialog.open(PasswordDialogComponent, { width: '420px', data: { usuarioId: this.auth.session()!.usuarioId, propio: true } });
  }

  salir() {
    this.auth.logout(false);
    this.router.navigate(['/login']);
  }
}
