import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import * as M from '../models/api.models';

const base = environment.apiUrl;

function params(obj: Record<string, string | number | null | undefined>): HttpParams {
  let p = new HttpParams();
  for (const [k, v] of Object.entries(obj)) if (v !== null && v !== undefined && v !== '') p = p.set(k, String(v));
  return p;
}

@Injectable({ providedIn: 'root' })
export class ClientesApi {
  private http = inject(HttpClient);
  listar(buscar: string, pagina: number, tamano: number): Observable<M.Cliente[]> {
    return this.http.get<M.Cliente[]>(`${base}/clientes`, { params: params({ buscar, pagina, tamano }) });
  }
  obtener(id: number) { return this.http.get<M.Cliente>(`${base}/clientes/${id}`); }
  historial(id: number) { return this.http.get<M.HistorialCliente>(`${base}/clientes/${id}/historial`); }
  crear(r: M.ClienteRequest) { return this.http.post<M.Cliente>(`${base}/clientes`, r); }
  actualizar(id: number, r: M.ClienteRequest) { return this.http.put<M.Cliente>(`${base}/clientes/${id}`, r); }
  eliminar(id: number) { return this.http.delete<void>(`${base}/clientes/${id}`); }
}

@Injectable({ providedIn: 'root' })
export class OptometriaApi {
  private http = inject(HttpClient);
  crearExamen(r: M.ExamenVisualRequest) { return this.http.post<M.ExamenVisual>(`${base}/examenes-visuales`, r); }
  optometristas() { return this.http.get<M.Optometrista[]>(`${base}/examenes-visuales/optometristas`); }
  obtenerExamen(id: number) { return this.http.get<M.ExamenVisual>(`${base}/examenes-visuales/${id}`); }
  crearReceta(r: M.RecetaRequest) { return this.http.post<M.Receta>(`${base}/recetas`, r); }
}

@Injectable({ providedIn: 'root' })
export class OrdenesApi {
  private http = inject(HttpClient);
  listar(estado?: string) { return this.http.get<M.OrdenTrabajo[]>(`${base}/ordenes-trabajo`, { params: params({ estado }) }); }
  obtener(id: number) { return this.http.get<M.OrdenTrabajo>(`${base}/ordenes-trabajo/${id}`); }
  crear(r: M.OrdenTrabajoRequest) { return this.http.post<M.OrdenTrabajo>(`${base}/ordenes-trabajo`, r); }
  cambiarEstado(id: number, estado: string) { return this.http.patch<M.OrdenTrabajo>(`${base}/ordenes-trabajo/${id}/estado`, { estado }); }
}

@Injectable({ providedIn: 'root' })
export class VentasApi {
  private http = inject(HttpClient);
  listar(pagina: number, tamano: number) { return this.http.get<M.Venta[]>(`${base}/ventas`, { params: params({ pagina, tamano }) }); }
  obtener(id: number) { return this.http.get<M.Venta>(`${base}/ventas/${id}`); }
  crear(r: M.VentaRequest) { return this.http.post<M.Venta>(`${base}/ventas`, r); }
  registrarPago(id: number, r: M.PagoRequest) { return this.http.post<M.Venta>(`${base}/ventas/${id}/pagos`, r); }
  anular(id: number) { return this.http.post<M.Venta>(`${base}/ventas/${id}/anular`, {}); }
}

@Injectable({ providedIn: 'root' })
export class ProductosApi {
  private http = inject(HttpClient);
  listar(buscar?: string) { return this.http.get<M.Producto[]>(`${base}/productos`, { params: params({ buscar }) }); }
  categorias() { return this.http.get<M.CategoriaProducto[]>(`${base}/productos/categorias`); }
  crear(r: M.ProductoRequest) { return this.http.post<M.Producto>(`${base}/productos`, r); }
}

@Injectable({ providedIn: 'root' })
export class AdminApi {
  private http = inject(HttpClient);
  empresas() { return this.http.get<M.Empresa[]>(`${base}/empresas`); }
  crearEmpresa(r: M.EmpresaRequest) { return this.http.post<M.Empresa>(`${base}/empresas`, r); }
  actualizarEmpresa(id: number, r: M.EmpresaRequest) { return this.http.put<M.Empresa>(`${base}/empresas/${id}`, r); }

  sucursales() { return this.http.get<M.Sucursal[]>(`${base}/sucursales`); }
  crearSucursal(r: M.SucursalRequest) { return this.http.post<M.Sucursal>(`${base}/sucursales`, r); }
  actualizarSucursal(id: number, r: M.SucursalRequest) { return this.http.put<M.Sucursal>(`${base}/sucursales/${id}`, r); }

  roles() { return this.http.get<M.RolDto[]>(`${base}/roles`); }
  usuarios() { return this.http.get<M.Usuario[]>(`${base}/usuarios`); }
  crearUsuario(r: M.UsuarioCreateRequest) { return this.http.post<M.Usuario>(`${base}/usuarios`, r); }
  actualizarUsuario(id: number, r: M.UsuarioUpdateRequest) { return this.http.put<M.Usuario>(`${base}/usuarios/${id}`, r); }
  cambiarPassword(id: number, r: M.CambiarPasswordRequest) { return this.http.put<void>(`${base}/usuarios/${id}/password`, r); }
}
