# ÓPTICA FAMILIAR

# Referencia de la API (v1)

Base: `/api` · Formato: JSON · Autenticación: `Authorization: Bearer <token>` (excepto login).
Documentación interactiva: `/swagger` (solo en desarrollo). Errores en formato ProblemDetails
(`400` regla de negocio o validación, `401`, `403`, `404`, `409`, `429`).

Roles: **SA** = SUPERADMIN · **AD** = ADMIN · **VE** = VENDEDOR · **OP** = OPTOMETRISTA.
Todas las rutas (salvo las de `SA`) trabajan **solo con datos de la empresa del token**.

## Autenticación

| Método | Ruta | Roles | Descripción |
|---|---|---|---|
| POST | `/auth/login` | público | Body: `codigoEmpresa`, `username`, `password`. Devuelve token, rol, sucursal y empresa |

## Empresas, sucursales, usuarios, roles

| Método | Ruta | Roles | Descripción |
|---|---|---|---|
| GET | `/empresas/actual` | todos | Empresa del usuario (moneda, IVA, país…) |
| GET | `/empresas/actual/marca` | todos | Identidad visual: `colorPrimario`, `colorSecundario`, `logo` |
| PUT | `/empresas/actual/marca` | AD | Actualiza colores (`#RRGGBB`) y logo (ruta `/brand/...` o data URL png/jpeg/webp ≤ ~300 KB; no SVG) |
| GET/POST | `/empresas` | SA | Listar / crear (crea sus categorías base) |
| GET/PUT | `/empresas/{id}` | SA | Obtener / actualizar (incluye `estado`) |
| GET | `/sucursales`, `/sucursales/{id}` | todos | Sucursales de la empresa (SA: todas) |
| POST/PUT | `/sucursales`, `/sucursales/{id}` | AD, SA | Crear / editar (`empresaId` solo lo usa SA) |
| GET | `/roles` | AD, SA | Roles asignables (SUPERADMIN solo visible para SA) |
| GET/POST | `/usuarios` | AD, SA | Listar / crear (optometrista crea su perfil) |
| GET/PUT | `/usuarios/{id}` | AD, SA | Obtener / actualizar rol, sucursal y estado |
| PUT | `/usuarios/{id}/password` | todos | Cambiar la propia (pide la actual) o restablecer (AD/SA) |

## Clientes y optometría

| Método | Ruta | Roles | Descripción |
|---|---|---|---|
| GET | `/clientes?buscar&pagina&tamano` | AD, VE, OP | Búsqueda paginada (máx. 100) |
| GET | `/clientes/{id}`, `/clientes/{id}/historial` | AD, VE, OP | Ficha e historial (exámenes, órdenes, ventas) |
| POST/PUT | `/clientes`, `/clientes/{id}` | AD, VE | Alta / edición (identificación única por empresa) |
| DELETE | `/clientes/{id}` | AD | Baja lógica |
| GET | `/examenes-visuales/optometristas` | AD, VE, OP | Optometristas activos |
| GET | `/examenes-visuales/{id}`, `/examenes-visuales/cliente/{clienteId}` | AD, VE, OP | Consulta |
| POST | `/examenes-visuales` | AD, OP | Registra examen (crea paciente e historia si no existen) |
| GET | `/recetas/{id}` | AD, VE, OP | Consulta |
| POST | `/recetas` | AD, OP | Una receta por examen; rangos validados |

## Comercial

| Método | Ruta | Roles | Descripción |
|---|---|---|---|
| GET | `/productos?buscar`, `/productos/{id}`, `/productos/categorias` | AD, VE, OP | Catálogo de la empresa |
| POST | `/productos` | AD | Alta de producto |
| GET | `/ordenes-trabajo?estado`, `/ordenes-trabajo/{id}` | AD, VE, OP | Consulta |
| POST | `/ordenes-trabajo` | AD, VE | Desde una receta |
| PATCH | `/ordenes-trabajo/{id}/estado` | AD, VE | Solo avanza hacia adelante |
| GET | `/ventas?pagina&tamano`, `/ventas/{id}` | AD, VE | VE: solo su sucursal; AD: todas |
| POST | `/ventas` | AD, VE | Productos + pagos; el servidor calcula totales e IVA; puede vincular una orden |
| POST | `/ventas/{id}/pagos` | AD, VE | Abono; no puede superar el saldo |
| POST | `/ventas/{id}/anular` | AD | Anula la venta |

## Reglas de negocio destacadas

- Venta: descuento por línea ≤ valor de la línea; pagos ≤ total; estado `PAGADA` cuando el saldo es 0.
- Orden de trabajo: estados `CREADA → EN_PRODUCCION → EN_LABORATORIO → TERMINADA → ENTREGADA`, sin retroceso.
- Receta: eje obligatorio (0–180) si hay cilindro; esfera −30…30, cilindro −10…10, adición 0…5, DP 20…100.
