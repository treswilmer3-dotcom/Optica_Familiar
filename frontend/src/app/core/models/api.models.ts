// Contratos de la API (ver backend/OpticaFamiliar.Application/DTOs)

export interface LoginRequest { codigoEmpresa: string; username: string; password: string; }
export interface LoginResponse {
  token: string; expiraEn: string; usuarioId: number; username: string; rol: string;
  sucursalId: number; empresaId: number; empresa?: string;
}

export type Rol = 'SUPERADMIN' | 'ADMIN' | 'VENDEDOR' | 'OPTOMETRISTA';

export interface Empresa {
  id: number; codigo: string; razonSocial: string; nombreComercial: string; identificacionFiscal: string;
  pais: string; moneda: string; zonaHoraria: string; idioma: string; ivaPorcentaje: number;
  direccion?: string | null; telefono?: string | null; correo?: string | null; sitioWeb?: string | null;
  estado?: string | null;
}
export type EmpresaRequest = Omit<Empresa, 'id'>;

/** Identidad visual de la empresa. `logo` es una ruta estática (/brand/...) o un data URL. */
export interface Marca { colorPrimario?: string | null; colorSecundario?: string | null; logo?: string | null; }

export interface Sucursal {
  id: number; empresaId?: number | null; codigo: string; nombre: string; direccion?: string | null;
  telefono?: string | null; correo?: string | null; ciudad?: string | null; provincia?: string | null; estado?: string | null;
}
export type SucursalRequest = Omit<Sucursal, 'id' | 'estado'>;

export interface RolDto { id: number; codigo: string; nombre: string; descripcion?: string | null; }

export interface Usuario {
  id: number; empresaId: number; username: string; rolId: number; rol: string; sucursalId: number;
  sucursal?: string | null; nombreCompleto?: string | null; estado?: string | null; ultimoAcceso?: string | null;
}
export interface UsuarioCreateRequest {
  empresaId?: number | null; username: string; password: string; rolId: number; sucursalId: number;
  nombres: string; apellidos: string; tipoIdentificacion?: string | null; numeroIdentificacion?: string | null;
  correo?: string | null; celular?: string | null;
}
export interface UsuarioUpdateRequest { rolId: number; sucursalId: number; estado: string; }
export interface CambiarPasswordRequest { passwordActual?: string | null; passwordNueva: string; }

export interface ClienteRequest {
  tipoIdentificacion?: string | null; numeroIdentificacion: string; nombres: string; apellidos: string;
  fechaNacimiento?: string | null; genero?: string | null; telefono?: string | null; celular?: string | null;
  correo?: string | null; direccion?: string | null; observaciones?: string | null;
}
export interface Cliente extends ClienteRequest { id: number; personaId: number; fechaRegistro: string; }

export interface ExamenVisualRequest {
  clienteId: number; optometristaId?: number | null; citaId?: number | null; motivoConsulta?: string | null;
  diagnostico?: string | null; observaciones?: string | null; recomendaciones?: string | null;
}
export interface Optometrista { id: number; nombre: string; numeroRegistro?: string | null; }
export interface RecetaRequest {
  consultaId: number; odEsfera?: number | null; odCilindro?: number | null; odEje?: number | null; odAdicion?: number | null;
  oiEsfera?: number | null; oiCilindro?: number | null; oiEje?: number | null; oiAdicion?: number | null;
  distanciaPupilar?: number | null; observacion?: string | null;
}
export interface Receta extends RecetaRequest { id: number; fechaEmision: string; }
export interface ExamenVisual {
  id: number; clienteId: number; clienteNombre?: string; historiaClinicaId: number; numeroHistoria?: string;
  optometristaId: number; citaId?: number | null; fechaConsulta: string; motivoConsulta?: string | null;
  diagnostico?: string | null; observaciones?: string | null; recomendaciones?: string | null; receta?: Receta | null;
}

export const ESTADOS_ORDEN = ['CREADA', 'EN_PRODUCCION', 'EN_LABORATORIO', 'TERMINADA', 'ENTREGADA'] as const;
export interface OrdenTrabajoRequest { recetaId: number; fechaEntregaEstimada?: string | null; observaciones?: string | null; }
export interface OrdenTrabajo {
  id: number; ventaId?: number | null; recetaId?: number | null; numeroOrden: string; fechaIngreso: string;
  fechaEntregaEstimada?: string | null; fechaEntregaReal?: string | null; estado: string; observaciones?: string | null;
}

export interface CategoriaProducto { id: number; codigo: string; nombre: string; }
export interface ProductoRequest {
  categoriaId: number; marcaId?: number | null; codigo: string; codigoBarras?: string | null; nombre: string;
  descripcion?: string | null; costo: number; precio: number; requiereFormula: boolean;
}
export interface Producto extends ProductoRequest { id: number; categoria?: string; estado?: string; }

export interface VentaDetalleRequest { productoId: number; cantidad: number; precioUnitario?: number | null; descuento: number; }
export interface PagoRequest { metodoPago: string; valor: number; referencia?: string | null; }
export interface VentaRequest { clienteId: number; ordenTrabajoId?: number | null; detalles: VentaDetalleRequest[]; pagos: PagoRequest[]; }
export interface VentaDetalle { id: number; productoId: number; producto?: string; cantidad: number; precioUnitario: number; descuento: number; subtotal: number; }
export interface Pago { id: number; metodoPago: string; valor: number; referencia?: string | null; fechaPago: string; }
export interface Venta {
  id: number; clienteId: number; clienteNombre?: string; usuarioId: number; sucursalId: number; numeroFactura: string;
  fechaVenta: string; subtotal: number; descuento: number; iva: number; total: number; totalPagado: number; estado?: string;
  detalles: VentaDetalle[]; pagos: Pago[];
}

export interface HistorialCliente { cliente: Cliente; examenes: ExamenVisual[]; ordenes: OrdenTrabajo[]; ventas: Venta[]; }

/** ProblemDetails (RFC 9457) devuelto por la API. */
export interface ProblemDetails { title?: string; detail?: string; status?: number; errors?: Record<string, string[]>; }
