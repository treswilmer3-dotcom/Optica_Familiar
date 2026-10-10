# MÓDULO DE TRANSFERENCIAS ENTRE SUCURSALES

## Objetivo

Permitir el traslado controlado de productos entre sucursales, manteniendo trazabilidad completa de inventario y auditoría de movimientos.

---

## transferencia

Representa la transferencia principal entre una sucursal origen y una sucursal destino.

```text
id

numero_transferencia

sucursal_origen_id
sucursal_destino_id

usuario_solicita_id
usuario_aprueba_id

fecha_solicitud
fecha_aprobacion
fecha_envio
fecha_recepcion

observaciones

estado
```

### Estados

```text
BORRADOR
SOLICITADA
APROBADA
EN_TRANSITO
RECIBIDA
CANCELADA
```

### Relaciones

```text
sucursal 1:N transferencia_origen

sucursal 1:N transferencia_destino

usuario 1:N transferencia
```

---

## transferencia_detalle

Detalle de productos incluidos en la transferencia.

```text
id

transferencia_id

producto_id

cantidad_solicitada

cantidad_aprobada

cantidad_enviada

cantidad_recibida

observacion
```

### Relaciones

```text
transferencia 1:N transferencia_detalle

producto 1:N transferencia_detalle
```

---

## flujo conceptual de transferencias

```text
Sucursal A

↓ Solicita

Transferencia

↓ Aprueba

Supervisor

↓ Envía

Bodega

↓ Recibe

Sucursal B

↓ Actualiza

Inventario
```

---

# MÓDULO DE CONFIGURACIÓN Y PARÁMETROS DEL SISTEMA

## Objetivo

Centralizar todas las configuraciones funcionales y operativas del sistema para evitar cambios mediante programación.

---

## configuracion_sistema

Configuraciones generales del sistema.

```text
id

codigo

nombre

descripcion

valor

tipo_dato

categoria

editable

estado
```

### Ejemplos

```text
PORCENTAJE_IVA

DIAS_GARANTIA

MONEDA

FORMATO_FACTURA

DIAS_CADUCIDAD_RECETA

PERMITE_STOCK_NEGATIVO
```

---

## empresa_configuracion

Configuraciones específicas de la empresa.

```text
id

empresa_id

logo

color_primario

color_secundario

correo_notificaciones

telefono_contacto

direccion_matriz

sitio_web

mensaje_factura
```

### Relaciones

```text
empresa 1:1 empresa_configuracion
```

---

## sucursal_configuracion

Configuraciones específicas por sucursal.

```text
id

sucursal_id

nombre_impresora

impresora_facturas

impresora_etiquetas

correo_sucursal

telefono_sucursal

direccion_sucursal
```

### Relaciones

```text
sucursal 1:1 sucursal_configuracion
```

---

## parametro_catalogo

Permite parametrizar listas configurables sin necesidad de cambios en código.

```text
id

codigo

nombre

categoria

descripcion

orden

activo
```

### Ejemplos

```text
TIPO_IDENTIFICACION

GENERO

METODO_PAGO

ESTADO_CITA

ESTADO_ORDEN_TRABAJO

TIPO_MOVIMIENTO_CAJA

TIPO_MOVIMIENTO_INVENTARIO
```

---

## numeracion_documento

Administra secuencias y consecutivos de documentos.

```text
id

sucursal_id

tipo_documento

serie

numero_actual

numero_final

reinicio_anual

estado
```

### Ejemplos

```text
FACTURA

NOTA_CREDITO

ORDEN_TRABAJO

COMPRA

TRANSFERENCIA
```

### Relaciones

```text
sucursal 1:N numeracion_documento
```

---

# ACTUALIZACIÓN DEL RESUMEN DE TABLAS

```text
empresa
empresa_configuracion

sucursal
sucursal_configuracion

rol
permiso
rol_permiso
usuario

cliente
paciente

optometrista
cita

historia_clinica
consulta
receta

categoria_producto
marca
producto

inventario
movimiento_inventario

transferencia
transferencia_detalle

proveedor
compra
compra_detalle

venta
venta_detalle

orden_trabajo

caja
movimiento_caja

pago

parametro_catalogo
configuracion_sistema
numeracion_documento

auditoria
```

---

# REGLAS DE NEGOCIO PRINCIPALES

## Multi-Sucursal

```text
Toda operación pertenece obligatoriamente a una sucursal.
```

---

## Inventario

```text
El stock se administra por sucursal.

Toda entrada o salida debe generar un movimiento de inventario.
```

---

## Ventas

```text
Una venta puede tener múltiples productos.

Una venta puede tener múltiples pagos.
```

---

## Pacientes

```text
Un paciente puede tener múltiples citas.

Un paciente puede tener múltiples consultas.

Una consulta genera una receta.
```

---

## Órdenes de Trabajo

```text
Una venta puede generar una orden de trabajo.

Una orden de trabajo puede requerir una receta válida.
```

---

## Transferencias

```text
No se podrán transferir existencias superiores al stock disponible.

Toda transferencia debe registrar:
- solicitud
- aprobación
- envío
- recepción
```

---

## Auditoría

```text
Todas las operaciones críticas deben quedar registradas.

INSERT
UPDATE
DELETE
LOGIN
LOGOUT
APROBACIONES
TRANSFERENCIAS
CIERRES DE CAJA
```

---

# MODELO LÓGICO COMPLETO - VERSIÓN 1.0

Módulos incluidos:

```text
01. Organización
02. Seguridad
03. Clientes
04. Pacientes
05. Agenda Médica
06. Historia Clínica
07. Recetas
08. Catálogo de Productos
09. Inventario
10. Transferencias entre Sucursales
11. Compras
12. Ventas
13. Órdenes de Trabajo
14. Caja
15. Pagos
16. Configuración y Parámetros
17. Auditor
