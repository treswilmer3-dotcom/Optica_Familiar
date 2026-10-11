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
import { PasswordDialogComponent } from '../shared/password-dialog.component';
import { AuthService } from '../core/auth/auth.service';
import { Rol } from '../core/models/api.models';

interface NavItem { etiqueta: string; icono: string; ruta: string; roles: Rol[]; }

const NAV: NavItem[] = [
  { etiqueta: 'Inicio', icono: 'home', ruta: '/inicio', roles: ['ADMIN', 'VENDEDOR', 'OPTOMETRISTA', 'SUPERADMIN'] },
  { etiqueta: 'Clientes', icono: 'groups', ruta: '/clientes', roles: ['ADMIN', 'VENDEDOR', 'OPTOMETRISTA'] },
  { etiqueta: 'Órdenes de trabajo', icono: 'build', ruta: '/ordenes', roles: ['ADMIN', 'VENDEDOR', 'OPTOMETRISTA'] },
  { etiqueta: 'Ventas', icono: 'point_of_sale', ruta: '/ventas', roles: ['ADMIN', 'VENDEDOR'] },
  { etiqueta: 'Productos', icono: 'inventory_2', ruta: '/productos', roles: ['ADMIN', 'VENDEDOR', 'OPTOMETRISTA'] },
  { etiqueta: 'Sucursales', icono: 'store', ruta: '/admin/sucursales', roles: ['ADMIN', 'SUPERADMIN'] },
  { etiqueta: 'Usuarios', icono: 'manage_accounts', ruta: '/admin/usuarios', roles: ['ADMIN', 'SUPERADMIN'] },
  { etiqueta: 'Empresas', icono: 'business', ruta: '/plataforma/empresas', roles: ['SUPERADMIN'] }
];

@Component({
  selector: 'app-shell',
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive, MatSidenavModule, MatToolbarModule, MatListModule, MatIconModule, MatButtonModule, MatMenuModule],
  template: `
    <mat-sidenav-container class="contenedor">
      <mat-sidenav #nav [mode]="movil() ? 'over' : 'side'" [opened]="!movil()" class="lateral">
        <div class="marca"><mat-icon>visibility</mat-icon><span>Óptica Familiar</span></div>
        <mat-nav-list>
          @for (item of items(); track item.ruta) {
            <a mat-list-item [routerLink]="item.ruta" routerLinkActive="activo" (click)="movil() && nav.close()">
              <mat-icon matListItemIcon>{{ item.icono }}</mat-icon>
              <span matListItemTitle>{{ item.etiqueta }}</span>
            </a>
          }
        </mat-nav-list>
      </mat-sidenav>

      <mat-sidenav-content>
        <mat-toolbar color="primary">
          @if (movil()) { <button mat-icon-button (click)="nav.toggle()" aria-label="Menú"><mat-icon>menu</mat-icon></button> }
          <span class="empresa">{{ auth.session()?.empresa }}</span>
          <span class="espacio"></span>
          <button mat-button [matMenuTriggerFor]="menu" class="usuario">
            <mat-icon>account_circle</mat-icon>
            <span class="nombre">{{ auth.session()?.username }}</span>
          </button>
          <mat-menu #menu="matMenu">
            <div class="menu-info"><strong>{{ auth.session()?.username }}</strong><br><small>{{ etiquetaRol() }}</small></div>
            <button mat-menu-item (click)="cambiarPassword()"><mat-icon>key</mat-icon><span>Cambiar contraseña</span></button>
            <button mat-menu-item (click)="salir()"><mat-icon>logout</mat-icon><span>Cerrar sesión</span></button>
          </mat-menu>
        </mat-toolbar>
        <main><router-outlet /></main>
      </mat-sidenav-content>
    </mat-sidenav-container>`,
  styles: [`
    .contenedor { height: 100vh; }
    .lateral { width: 250px; }
    .marca { display: flex; align-items: center; gap: 8px; padding: 18px 16px; font-size: 1.15rem; font-weight: 500; color: #1565c0; }
    .espacio { flex: 1; }
    .empresa { margin-left: 8px; font-weight: 500; }
    .nombre { margin-left: 6px; }
    .menu-info { padding: 8px 16px; }
    a.activo { background: rgba(21, 101, 192, .12); }
    main { min-height: calc(100vh - 64px); }
  `]
})
export class ShellComponent {
  readonly auth = inject(AuthService);
  private readonly router = inject(Router);
  private readonly bp = inject(BreakpointObserver);
  private readonly dialog = inject(MatDialog);

  readonly movil = toSignal(this.bp.observe('(max-width: 900px)').pipe(map(r => r.matches)), { initialValue: false });
  readonly items = computed(() => NAV.filter(i => this.auth.rol() && i.roles.includes(this.auth.rol()!)));
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
