# Modelo Lógico
## Sistema de Gestión Integral para Ópticas

### Versión
1.0

### Objetivo

Definir las entidades, atributos, relaciones y reglas de negocio que servirán como base para la implementación en PostgreSQL 17 y Entity Framework Core 10.

---

# MÓDULO ORGANIZACIÓN

## empresa

```text
id
codigo
razon_social
nombre_comercial
identificacion_fiscal   (RUC en Ecuador)
pais                    (ISO 3166-1 alfa-2, p. ej. EC)
moneda                  (ISO 4217, p. ej. USD)
zona_horaria            (IANA, p. ej. America/Guayaquil)
idioma
iva_porcentaje
direccion
telefono
correo
sitio_web
estado
fecha_creacion
fecha_modificacion
```

---

## empresa_configuracion

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

---

## sucursal

```text
id
empresa_id

codigo
nombre

direccion
telefono
correo

ciudad
provincia

estado

fecha_creacion
fecha_modificacion
```

---

## sucursal_configuracion

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

---

# MÓDULO SEGURIDAD

## rol

```text
id

codigo
nombre
descripcion

estado

fecha_creacion
```

---

## permiso

```text
id

codigo
nombre

descripcion

modulo
```

---

## rol_permiso

```text
rol_id
permiso_id
```

---

# MÓDULO PERSONAS

## persona

```text
id

tipo_identificacion
numero_identificacion

nombres
apellidos

fecha_nacimiento

genero

telefono
celular

correo

direccion

estado

fecha_creacion
fecha_modificacion
```

---

## cliente

```text
id

persona_id

fecha_registro

observaciones
```

---

## paciente

```text
id

persona_id

ocupacion
empresa

alergias

antecedentes_medicos

observaciones_generales

estado
```

---

## usuario

```text
id

persona_id

rol_id
sucursal_id

username

password_hash

ultimo_acceso

estado

fecha_creacion
fecha_modificacion
```

---

## optometrista

```text
id

persona_id

usuario_id

numero_registro

especialidad

firma

estado
```

---

# MÓDULO AGENDA MÉDICA

## cita

```text
id

paciente_id
optometrista_id
sucursal_id

fecha

hora_inicio
hora_fin

motivo

estado

observaciones

fecha_registro
```

### Estados

```text
PROGRAMADA
CONFIRMADA
ATENDIDA
CANCELADA
NO_ASISTIO
```

---

# MÓDULO HISTORIA CLÍNICA

## historia_clinica

```text
id

paciente_id

numero_historia

fecha_apertura

estado
```

---

## consulta

```text
id

historia_clinica_id

optometrista_id

cita_id

fecha_consulta

motivo_consulta

diagnostico

observaciones

recomendaciones
```

---

## receta

```text
id

consulta_id

od_esfera
od_cilindro
od_eje
od_adicion

oi_esfera
oi_cilindro
oi_eje
oi_adicion

distancia_pupilar

observacion

fecha_emision
```

---

# MÓDULO PRODUCTOS

## categoria_producto

```text
id

codigo

nombre
descripcion

estado
```

---

## marca

```text
id

codigo

nombre
descripcion

estado
```

---

## producto

```text
id

categoria_id
marca_id

codigo

codigo_barras

nombre

descripcion

costo

precio

stock_minimo

stock_maximo

requiere_formula

estado

fecha_creacion
```

---

# MÓDULO INVENTARIO

## inventario

```text
id

sucursal_id

producto_id

stock_actual

stock_reservado

ultima_actualizacion
```

---

## movimiento_inventario

```text
id

inventario_id

usuario_id

tipo_movimiento

cantidad

stock_anterior

stock_nuevo

descripcion

fecha_movimiento
```

### Tipos

```text
INGRESO
SALIDA
TRANSFERENCIA
AJUSTE
```

---

# MÓDULO TRANSFERENCIAS

## transferencia

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

---

## transferencia_detalle

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

---

# MÓDULO PROVEEDORES

## proveedor

```text
id

ruc

razon_social

nombre_comercial

telefono

correo

direccion

contacto

estado
```

---

# MÓDULO COMPRAS

## compra

```text
id

proveedor_id

sucursal_id

numero_documento

fecha_compra

subtotal

iva

total

estado
```

---

## compra_detalle

```text
id

compra_id

producto_id

cantidad

costo_unitario

subtotal
```

---

# MÓDULO VENTAS

## venta

```text
id

cliente_id

usuario_id

sucursal_id

numero_factura

fecha_venta

subtotal

descuento

iva

total

estado
```

---

## venta_detalle

```text
id

venta_id

producto_id

cantidad

precio_unitario

descuento

subtotal
```

---

# MÓDULO ÓRDENES DE TRABAJO

## orden_trabajo

```text
id

venta_id

receta_id

numero_orden

fecha_ingreso

fecha_entrega_estimada

fecha_entrega_real

estado

observaciones
```

### Estados

```text
CREADA
EN_PRODUCCION
EN_LABORATORIO
TERMINADA
ENTREGADA
```

---

# MÓDULO CAJA

## caja

```text
id

sucursal_id

codigo

nombre

estado
```

---

## movimiento_caja

```text
id

caja_id

usuario_id

tipo_movimiento

valor

concepto

fecha_movimiento
```

### Tipos

```text
APERTURA
INGRESO
EGRESO
CIERRE
```

---

# MÓDULO PAGOS

## pago

```text
id

venta_id

metodo_pago

valor

referencia

fecha_pago
```

### Métodos

```text
EFECTIVO
TARJETA
TRANSFERENCIA
CHEQUE
CREDITO
```

---

# MÓDULO CONFIGURACIÓN DEL SISTEMA

## configuracion_sistema

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

---

## parametro_catalogo

```text
id

codigo

nombre

categoria

descripcion

orden

activo
```

---

## numeracion_documento

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

---

# MÓDULO AUDITORÍA

## auditoria

```text
id

usuario_id

tabla

registro_id

accion

valor_anterior

valor_nuevo

ip

fecha_evento
```

### Acciones

```text
INSERT
UPDATE
DELETE
LOGIN
LOGOUT
APROBACION
TRANSFERENCIA
CIERRE_CAJA
```

---

# RELACIONES PRINCIPALES

```text
empresa 1:N sucursal

empresa 1:1 empresa_configuracion

sucursal 1:1 sucursal_configuracion

rol N:M permiso

rol 1:N usuario

persona 1:1 cliente

persona 1:1 paciente

persona 1:1 usuario

persona 1:1 optometrista

sucursal 1:N usuario

paciente 1:N cita

optometrista 1:N cita

paciente 1:N historia_clinica

historia_clinica 1:N consulta

consulta 1:1 receta

categoria_producto 1:N producto

marca 1:N producto

producto 1:N inventario

sucursal 1:N inventario

inventario 1:N movimiento_inventario

proveedor 1:N compra

compra 1:N compra_detalle

producto 1:N compra_detalle

cliente 1:N venta

venta 1:N venta_detalle

producto 1:N venta_detalle

venta 1:N pago

venta 1:1 orden_trabajo

receta 1:N orden_trabajo

caja 1:N movimiento_caja

transferencia 1:N transferencia_detalle

producto 1:N transferencia_detalle

usuario 1:N auditoria
```

---

# RESUMEN DE ENTIDADES

```text
empresa
empresa_configuracion

sucursal
sucursal_configuracion

rol
permiso
rol_permiso

persona
cliente
paciente
usuario
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

configuración_sistema
parametro_catalogo
numeracion_documento

auditoria
```

---

# MULTIEMPRESA (v1.1)

Una sola base de datos y un solo esquema. Toda tabla que pertenece a una empresa lleva
`empresa_id` (FK a `empresa`, `ON DELETE RESTRICT`), aunque pueda deducirse por la sucursal
o por el padre. Así el aislamiento se aplica de forma uniforme y no depende de joins.

## Tablas con `empresa_id`

```text
sucursal, sucursal_configuracion, empresa_configuracion
usuario, persona, cliente, paciente, optometrista
cita, historia_clinica, consulta, receta
orden_trabajo, venta, venta_detalle, pago
categoria_producto, marca, producto
inventario, movimiento_inventario
transferencia, transferencia_detalle
proveedor, compra, compra_detalle
caja, movimiento_caja
configuracion_sistema, parametro_catalogo, numeracion_documento
auditoria
```

## Tablas globales (sin `empresa_id`)

```text
empresa
rol, permiso, rol_permiso      (catálogo de roles del sistema)
```

## Unicidad (siempre dentro de la empresa)

```text
usuario              (empresa_id, username)
persona              (empresa_id, numero_identificacion)  -- cuando no es nulo
producto             (empresa_id, codigo)
sucursal             (empresa_id, codigo)
orden_trabajo        (empresa_id, numero_orden)
historia_clinica     (empresa_id, numero_historia)
venta                (sucursal_id, numero_factura)
numeracion_documento (sucursal_id, tipo_documento)
empresa              (codigo)  y  (pais, identificacion_fiscal)
```

## Reglas

- La empresa activa sale del claim `empresa_id` del JWT. El `DbContext` aplica un filtro global
  por empresa y la asigna al insertar; sin contexto de empresa las consultas no devuelven nada.
- Una misma persona real puede ser cliente de dos empresas: son registros independientes.
- Login: `codigo_empresa + username + password`.
- Roles: `SUPERADMIN` (operador de la plataforma: gestiona empresas, sin acceso a datos clínicos
  ni comerciales), `ADMIN` (su empresa y todas sus sucursales), `VENDEDOR`, `OPTOMETRISTA`.
- Cliente, historia clínica y catálogo son de la empresa (compartidos entre sus sucursales);
  las ventas se limitan a la sucursal del usuario salvo para `ADMIN`.
- Numeración por sucursal; la serie incluye el código de sucursal (`OT-MATRIZ`, `HC-MATRIZ`).
