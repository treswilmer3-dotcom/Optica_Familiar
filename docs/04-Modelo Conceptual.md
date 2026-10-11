# Sistema de Gestión Integral para Ópticas
## Modelo Conceptual

### Objetivo

Definir las entidades principales del negocio y sus relaciones para un Sistema de Gestión Integral de Ópticas Multi-Sucursal.

---

### Nota de versión (v1.1)

El modelo es **multiempresa**: cada entidad que pertenece a una óptica lleva su empresa (ver `05-Modelo-Logico.md`,
sección Multiempresa). Los únicos elementos globales son la propia Empresa y el catálogo de Roles y Permisos.
Estado de implementación por dominio: `02-ARQUITECTURA-OFICIAL.md`.

---

# Alcance del Sistema

El sistema permitirá gestionar:

- Seguridad y control de accesos.
- Administración de sucursales.
- Gestión de clientes y pacientes.
- Agenda médica.
- Historia clínica.
- Recetas oftalmológicas.
- Inventario.
- Compras.
- Ventas.
- Órdenes de trabajo.
- Caja.
- Pagos.
- Auditoría.
- Reportes.

---

# 1. Organización

## Empresa

Representa la organización propietaria de las ópticas.

### Relación

```text
Empresa
 └── 1:N Sucursal
```

## Sucursal

Representa cada establecimiento físico de atención.

Funciones:

- Ventas
- Inventario
- Caja
- Atención médica
- Reportes

---

# 2. Seguridad

## Usuario

Representa una persona con acceso al sistema.

Ejemplos:

- Administrador
- Supervisor
- Optometrista
- Cajero
- Vendedor
- Bodeguero

## Rol

Agrupa permisos para ejecutar acciones dentro del sistema.

Ejemplos:

- Administrador General
- Administrador Sucursal
- Optometrista
- Cajero
- Vendedor

## Permiso

Define operaciones autorizadas.

Ejemplos:

- Crear ventas
- Modificar ventas
- Eliminar ventas
- Gestionar usuarios
- Ver reportes
- Registrar compras

### Relaciones

```text
Rol
 └── N:M Permiso

Usuario
 └── N:1 Rol

Usuario
 └── N:1 Sucursal
```

---

# 3. Clientes y Pacientes

## Cliente

Persona que realiza compras o utiliza servicios.

Información relevante:

- Identificación
- Nombres
- Apellidos
- Teléfono
- Correo electrónico
- Dirección

## Paciente

Persona que posee historial clínico.

Información relevante:

- Antecedentes
- Alergias
- Patologías
- Observaciones

### Relación

```text
Cliente
 └── 1:1 Paciente
```

---

# 4. Agenda Médica

## Optometrista

Profesional encargado de realizar consultas visuales.

## Cita

Permite gestionar la agenda de atención.

Estados:

- Programada
- Confirmada
- Atendida
- Cancelada
- No Asistió

### Relaciones

```text
Paciente
 └── 1:N Cita

Optometrista
 └── 1:N Cita

Sucursal
 └── 1:N Cita
```

---

# 5. Historia Clínica

## Historia Clínica

Expediente médico completo del paciente.

## Consulta

Registro de una atención realizada.

Información:

- Motivo de consulta
- Diagnóstico
- Observaciones
- Recomendaciones

## Receta

Prescripción visual emitida por el optometrista.

Información:

- Ojo Derecho (OD)
- Ojo Izquierdo (OI)
- Esfera
- Cilindro
- Eje
- Adición
- Distancia Pupilar

### Relaciones

```text
Paciente
 └── 1:N Historia Clínica

Historia Clínica
 └── 1:N Consulta

Consulta
 └── 1:1 Receta
```

---

# 6. Catálogo de Productos

## Categoría

Clasifica los productos.

Ejemplos:

- Monturas
- Lentes
- Accesorios
- Líquidos
- Estuches

## Marca

Identifica fabricantes o líneas comerciales.

Ejemplos:

- Ray-Ban
- Oakley
- Vogue
- Police

## Producto

Artículo disponible para comercialización.

### Relaciones

```text
Categoría
 └── 1:N Producto

Marca
 └── 1:N Producto
```

---

# 7. Inventario

## Inventario

Controla existencias por sucursal.

## Movimiento de Inventario

Registra cambios en stock.

Tipos:

- Ingreso
- Salida
- Transferencia
- Ajuste

### Relaciones

```text
Sucursal
 └── 1:N Inventario

Producto
 └── 1:N Inventario

Inventario
 └── 1:N MovimientoInventario
```

---

# 8. Compras

## Proveedor

Persona o empresa que suministra productos.

## Compra

Documento principal de adquisición.

## Detalle de Compra

Detalle de productos adquiridos.

### Relaciones

```text
Proveedor
 └── 1:N Compra

Compra
 └── 1:N DetalleCompra

Producto
 └── 1:N DetalleCompra
```

---

# 9. Ventas

## Venta

Documento comercial generado por una transacción.

## Detalle de Venta

Productos vendidos dentro de una venta.

### Relaciones

```text
Cliente
 └── 1:N Venta

Usuario
 └── 1:N Venta

Sucursal
 └── 1:N Venta

Venta
 └── 1:N DetalleVenta

Producto
 └── 1:N DetalleVenta
```

---

# 10. Órdenes de Trabajo

## Orden de Trabajo

Controla la fabricación o preparación de lentes.

Estados:

- Creada
- En Producción
- En Laboratorio
- Terminada
- Entregada

### Relaciones

```text
Venta
 └── 1:1 OrdenTrabajo

Receta
 └── 1:N OrdenTrabajo
```

---

# 11. Caja

## Caja

Representa la caja operativa de cada sucursal.

## Movimiento de Caja

Registra operaciones monetarias.

Tipos:

- Apertura
- Ingreso
- Egreso
- Cierre

### Relaciones

```text
Sucursal
 └── 1:N Caja

Caja
 └── 1:N MovimientoCaja
```

---

# 12. Pagos

## Pago

Representa los pagos asociados a una venta.

Métodos:

- Efectivo
- Tarjeta
- Transferencia
- Cheque
- Crédito

### Relaciones

```text
Venta
 └── 1:N Pago
```

---

# 13. Auditoría

## Auditoría

Registra cambios realizados en el sistema.

Información registrada:

- Usuario
- Fecha
- Hora
- Acción
- Entidad afectada
- Valor anterior
- Valor nuevo
- Dirección IP

### Relaciones

```text
Usuario
 └── 1:N Auditoría
```

---

# 14. Reportes

## Reporte

Permite obtener información para análisis y toma de decisiones.

Tipos:

- Reporte de ventas
- Reporte de compras
- Reporte de inventario
- Reporte de clientes
- Reporte de pacientes
- Reporte de caja
- Reporte de sucursales
- Reporte de auditoría
- Reporte de rentabilidad

---

# Resumen de Módulos

```text
01. Seguridad
02. Organización y Sucursales
03. Clientes
04. Pacientes
05. Agenda Médica
06. Historia Clínica
07. Recetas
08. Catálogo de Productos
09. Inventario
10. Compras
11. Ventas
12. Órdenes de Trabajo
13. Caja
14. Pagos
15. Auditoría
16. Reportes
```

---

# Próxima Fase

Modelo Lógico:

- Entidades
- Atributos
- Llaves Primarias (PK)
- Llaves Foráneas (FK)
- Cardinalidades
- Restricciones
- Índices
- Reglas de Integridad

Base para implementación en PostgreSQL 17 y Entity Framework Core 10.
