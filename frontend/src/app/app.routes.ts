import { Routes } from '@angular/router';
import { authGuard, roleGuard } from './core/auth/guards';
import { ShellComponent } from './layout/shell.component';

export const routes: Routes = [
  { path: 'login', loadComponent: () => import('./features/login/login.component').then(m => m.LoginComponent) },
  {
    path: '',
    component: ShellComponent,
    canActivate: [authGuard],
    children: [
      { path: '', pathMatch: 'full', redirectTo: 'inicio' },
      { path: 'inicio', loadComponent: () => import('./features/inicio/inicio.component').then(m => m.InicioComponent) },

      // Clientes + historial + optometría (el SUPERADMIN no accede a datos clínicos ni comerciales)
      {
        path: 'clientes',
        canActivate: [roleGuard('ADMIN', 'VENDEDOR', 'OPTOMETRISTA')],
        children: [
          { path: '', loadComponent: () => import('./features/clientes/clientes-lista.component').then(m => m.ClientesListaComponent) },
          { path: 'nuevo', canActivate: [roleGuard('ADMIN', 'VENDEDOR')], loadComponent: () => import('./features/clientes/cliente-form.component').then(m => m.ClienteFormComponent) },
          { path: ':id', loadComponent: () => import('./features/clientes/cliente-detalle.component').then(m => m.ClienteDetalleComponent) },
          { path: ':id/editar', canActivate: [roleGuard('ADMIN', 'VENDEDOR')], loadComponent: () => import('./features/clientes/cliente-form.component').then(m => m.ClienteFormComponent) },
          { path: ':id/examen', canActivate: [roleGuard('ADMIN', 'OPTOMETRISTA')], loadComponent: () => import('./features/optometria/examen-form.component').then(m => m.ExamenFormComponent) }
        ]
      },
      {
        path: 'examenes/:examenId/receta',
        canActivate: [roleGuard('ADMIN', 'OPTOMETRISTA')],
        loadComponent: () => import('./features/optometria/receta-form.component').then(m => m.RecetaFormComponent)
      },
      {
        path: 'ordenes',
        canActivate: [roleGuard('ADMIN', 'VENDEDOR', 'OPTOMETRISTA')],
        children: [
          { path: '', loadComponent: () => import('./features/ordenes/ordenes-lista.component').then(m => m.OrdenesListaComponent) },
          { path: 'nueva', canActivate: [roleGuard('ADMIN', 'VENDEDOR')], loadComponent: () => import('./features/ordenes/orden-form.component').then(m => m.OrdenFormComponent) }
        ]
      },
      {
        path: 'ventas',
        canActivate: [roleGuard('ADMIN', 'VENDEDOR')],
        children: [
          { path: '', loadComponent: () => import('./features/ventas/ventas-lista.component').then(m => m.VentasListaComponent) },
          { path: 'nueva', loadComponent: () => import('./features/ventas/venta-form.component').then(m => m.VentaFormComponent) },
          { path: ':id', loadComponent: () => import('./features/ventas/venta-detalle.component').then(m => m.VentaDetalleComponent) }
        ]
      },
      {
        path: 'productos',
        canActivate: [roleGuard('ADMIN', 'VENDEDOR', 'OPTOMETRISTA')],
        loadComponent: () => import('./features/productos/productos.component').then(m => m.ProductosComponent)
      },

      // Administración
      {
        path: 'admin',
        canActivate: [roleGuard('ADMIN', 'SUPERADMIN')],
        children: [
          { path: 'sucursales', loadComponent: () => import('./features/admin/sucursales.component').then(m => m.SucursalesComponent) },
          { path: 'usuarios', loadComponent: () => import('./features/admin/usuarios.component').then(m => m.UsuariosComponent) },
          { path: 'marca', canActivate: [roleGuard('ADMIN')], loadComponent: () => import('./features/admin/marca.component').then(m => m.MarcaComponent) }
        ]
      },
      {
        path: 'plataforma/empresas',
        canActivate: [roleGuard('SUPERADMIN')],
        loadComponent: () => import('./features/admin/empresas.component').then(m => m.EmpresasComponent)
      }
    ]
  },
  { path: '**', redirectTo: '' }
];
