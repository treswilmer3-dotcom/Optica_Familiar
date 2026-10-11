# ÓPTICA FAMILIAR

# Arquitectura Oficial v1.1

## Estado

APROBADO · Fase 1 implementada (ver sección «Estado de implementación»)

---

# Decisión Arquitectónica

Se utilizará una única base de datos PostgreSQL centralizada.

No existirán bases de datos independientes por sucursal.

La información estará segregada mediante:

- Empresa
- Sucursal

---

# Modelo Organizacional

Empresa
└── Sucursal
├── Usuarios
├── Clientes
├── Inventario
├── Ventas
├── Compras
└── Reportes

---

# Arquitectura de Datos

Modelo Multiempresa

Una misma plataforma podrá operar múltiples ópticas.

Cada óptica será considerada una Empresa.

Modelo Multisucursal

Cada Empresa podrá tener una o más sucursales.

---

# Cliente Corporativo

Un cliente será único dentro de una empresa.

El cliente podrá ser atendido en cualquier sucursal conservando:

- Historial Clínico
- Historial Comercial
- Exámenes Visuales
- Recetas
- Ventas
- Pagos

---

# Inventario

El inventario será independiente por sucursal.

Se permitirá:

- Transferencias entre sucursales

Las transferencias actualizarán automáticamente:

- Inventario Origen
- Inventario Destino

---

# Reportes

El Administrador General podrá visualizar:

- Ventas Consolidadas
- Inventario Consolidado
- Compras Consolidadas
- Clientes Consolidados
- Indicadores Corporativos

---

# Visión Comercial

Óptica Familiar será el primer cliente del sistema.

La plataforma será diseñada para comercialización futura.

Arquitectura objetivo:

- Multiempresa
- Multisucursal
- SaaS Ready

---

# Restricciones de la Fase 1

No se implementarán:

- Microservicios
- Event Bus
- Kafka
- RabbitMQ
- Kubernetes
- Arquitecturas Distribuidas

---

# Dominios Funcionales

- Seguridad
- Administración
- Clientes
- Optometría
- Recetas
- Ventas
- Inventario
- Compras
- Caja
- Reportes
- Auditoría

---

# Flujo Principal del Negocio

Cliente
↓
Examen Visual
↓
Receta
↓
Orden de Trabajo
↓
Venta
↓
Entrega de Lentes

Este flujo constituye el núcleo funcional del negocio.

---

# Entidades Iniciales

- Empresa
- Sucursal
- Usuario
- Rol
- Cliente
- ExamenVisual
- Receta
- OrdenTrabajo
- Venta
- DetalleVenta

---

# Roadmap

Fase 1 — **Completada**

- Seguridad, multiempresa, clientes, optometría, ventas y órdenes de trabajo
- Frontend funcional con control por roles

Fase 1.5 — Afinamiento y entrega (siguiente)

- Identidad visual: logo y colores por empresa (`empresa_configuracion` ya modela logo y colores)
- Ajustes de interfaz para computador y celular
- Cambio de credenciales iniciales y carga de datos reales
- Empaquetado con Docker / Docker Compose y despliegue desde registro de contenedores

Fase 2

- Inventario por sucursal, transferencias, compras y proveedores
- Agenda de citas

Fase 3

- Reportes y dashboard
- Facturación electrónica (SRI Ecuador)

Fase 4

- Comercialización SaaS (planes, alta autoservicio de empresas, otros países)

---

# Estructura de la solución

```text
backend/
  OpticaFamiliar.Domain          Entidades (36) y reglas básicas. Sin dependencias.
  OpticaFamiliar.Application     DTOs, interfaces de servicios, constantes (roles, estados), excepciones.
  OpticaFamiliar.Infrastructure  EF Core (AppDbContext + migraciones), implementación de servicios,
                                 JWT, BCrypt, numeración de documentos y datos iniciales (seed).
  OpticaFamiliar.API             Controllers REST, JWT/CORS/Swagger/Serilog, composición de dependencias.
frontend/                        Angular 18 (core · layout · shared · features)
scripts/                         dev.sh (entorno local) y smoke-test.sh (verificación)
docs/                            Documentación
```

Dependencias: `API → Application, Infrastructure` · `Infrastructure → Application, Domain` · `Application → Domain`.
La capa Application solo define contratos; las reglas de negocio viven en los servicios de Infrastructure.

---

# Multiempresa (implementada)

Una sola base de datos y un solo esquema. Toda tabla que pertenece a una empresa lleva `empresa_id`
(FK a `empresa`). El aislamiento es automático y falla cerrado:

1. El JWT contiene `empresa_id`, `sucursal_id` y `role`.
2. `AppDbContext` aplica un **filtro global** `EmpresaId == empresa del usuario` a todas las entidades de la empresa.
   Sin empresa en el contexto (login, seed) las consultas no devuelven nada.
3. Al insertar, la empresa se asigna sola; escribir datos de otra empresa o cambiar la empresa de un registro lanza error.
4. Las únicas operaciones que cruzan empresas (login por código de empresa, gestión de empresas, sucursales y usuarios
   por el `SUPERADMIN`) usan `IgnoreQueryFilters()` de forma explícita y acotada.

Reglas:

- Login: **código de empresa + usuario + contraseña**. El usuario es único dentro de su empresa.
- Una persona puede ser cliente de dos empresas: son registros independientes (cada empresa es dueña de sus datos).
- Catálogo de productos, categorías y numeración son por empresa; el inventario será por sucursal.
- Parámetros regionales por empresa: país, moneda, zona horaria, idioma e IVA (hoy Ecuador, USD, 15 %).
- Roles globales: `SUPERADMIN` (plataforma; gestiona empresas, **sin acceso a datos clínicos ni comerciales**),
  `ADMIN` (su empresa y todas sus sucursales), `VENDEDOR`, `OPTOMETRISTA`.
- Visibilidad: clientes e historial son de la empresa (cliente corporativo entre sucursales);
  las ventas se limitan a la sucursal del usuario, salvo para `ADMIN`.

Detalle de tablas y unicidades en `05-Modelo-Logico.md`.

---

# Seguridad

- Autenticación JWT (8 h por defecto), contraseñas con BCrypt, respuesta uniforme ante credenciales inválidas.
- Autorización por rol en cada endpoint; el frontend solo oculta opciones, la API es quien autoriza.
- Límite de intentos de login por IP, CORS por lista blanca, errores sin detalles internos (ProblemDetails).
- Secretos (cadena de conexión, clave JWT, contraseñas iniciales) fuera de git.
- Pendientes: auditoría de operaciones, bloqueo de cuenta por intentos fallidos, invalidación de tokens al desactivar
  una empresa o usuario (hoy el token vigente sigue válido hasta vencer).

---

# Estado de implementación

| Dominio | Estado |
|---|---|
| Organización (empresa, sucursal) y multiempresa | Implementado |
| Seguridad (login, usuarios, roles, JWT) | Implementado |
| Clientes e historial | Implementado |
| Optometría (examen visual, receta) | Implementado |
| Comercial (productos, orden de trabajo, venta, pagos, anulación) | Implementado |
| Agenda médica (citas) | Modelado, sin servicios (fuera de Fase 1) |
| Inventario, transferencias, compras, proveedores | Modelado, sin servicios (Fase 2) |
| Caja | Modelado, sin servicios |
| Reportes, dashboard, facturación electrónica (SRI) | Pendiente (Fase 3) |
| Auditoría | Modelada, sin implementar |

---

# Decisiones tomadas durante la Fase 1

- El «Examen Visual» se implementa sobre `consulta` + `historia_clinica` + `paciente` (el paciente se crea automáticamente
  a partir del cliente al registrar su primer examen).
- Baja lógica de clientes (`persona.estado = INACTIVO`) para conservar el historial clínico y comercial.
- Venta: el servidor recalcula precios, descuentos e IVA; el cliente solo envía productos, cantidades y pagos.
- Numeración correlativa por sucursal y tipo de documento (`numeracion_documento`); la serie incluye el código de
  sucursal (`OT-QUITO-000000001`) para ser única dentro de la empresa.
- Los datos de demostración (`DEMO-ANDINA`, `DEMO-SIERRA`) están separados de las empresas reales (ver `08-DATOS-DE-PRUEBA.md`).

---

# Decisión Final

La arquitectura aprobada para el proyecto es:

- PostgreSQL Centralizado
- Multiempresa
- Multisucursal
- Monolito Modular
- .NET 10
- Angular 18

Esta decisión queda congelada para el inicio del desarrollo.
