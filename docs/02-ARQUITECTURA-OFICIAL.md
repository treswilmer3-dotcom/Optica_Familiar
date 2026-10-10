# ÓPTICA FAMILIAR

# Arquitectura Oficial v1.0

## Estado

APROBADO

Congelado para Fase 1

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

Fase 1

- Seguridad
- Clientes
- Optometría
- Venta

Fase 2

- Inventario
- Compras
- Proveedores

Fase 3

- Reportes
- Dashboard
- Facturación Electrónica

Fase 4

- Comercialización SaaS

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
