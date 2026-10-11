# ÓPTICA FAMILIAR

# Fase 1 de Desarrollo

## Estado

COMPLETADA

Validada de extremo a extremo (API, base de datos y frontend) con datos de demostración.

---

# Objetivo

Implementar el núcleo operativo funcional.

---

# Resultado por módulo

## Módulo Seguridad — completado

- Empresa y Sucursal (alta y edición; la empresa nueva se crea con sus categorías base)
- Rol (catálogo del sistema) y Usuario (alta, edición, activación, cambio y restablecimiento de contraseña)
- Login por código de empresa con JWT

## Módulo Clientes — completado

- Cliente (alta, edición, búsqueda paginada, baja lógica) e historial clínico y comercial

## Módulo Optometría — completado

- Examen Visual (consulta e historia clínica) y Receta, con validación de rangos y eje obligatorio si hay cilindro

## Módulo Comercial — completado

- Orden de Trabajo (estados `CREADA → EN_PRODUCCION → EN_LABORATORIO → TERMINADA → ENTREGADA`)
- Venta y Detalle de Venta, pagos parciales o mixtos, anulación
- Catálogo mínimo de productos (necesario para facturar)

---

# Flujo Mínimo Operativo (validado)

Login → Cliente → Examen Visual → Receta → Orden de Trabajo → Venta → Entrega → Historial del cliente

---

# Alcance respecto a lo planificado

| Previsto | Resultado |
|---|---|
| Empresa, Sucursal, Rol, Usuario, Login, JWT | Hecho |
| Cliente | Hecho |
| Examen Visual, Receta | Hecho |
| Orden de Trabajo, Venta, Detalle de Venta | Hecho |
| Historial del cliente | Hecho |
| Multiempresa (originalmente prevista para Fase 4) | **Adelantada a Fase 1** por requerimiento comercial |
| Rol SUPERADMIN y parámetros regionales por empresa | Añadido |
| Catálogo mínimo de productos | Añadido (necesario para vender) |

---

# Fuera de Alcance (siguen pendientes)

- Inventario completo (el stock no se descuenta al vender)
- Compras y Proveedores
- Agenda de citas
- Dashboard y reportes
- Facturación Electrónica
- Multiidioma
- Aplicación Móvil (la web es responsive)

---

# Objetivo de Salida — cumplido

El sistema permite:

- Iniciar sesión
- Registrar clientes
- Registrar exámenes visuales
- Registrar recetas
- Registrar órdenes de trabajo
- Registrar ventas
- Generar historial del cliente

---

# Pendientes técnicos conocidos

- Auditoría de operaciones (modelada, no implementada)
- Bloqueo de cuenta por intentos fallidos
- Invalidar tokens vigentes al desactivar usuarios o empresas
- Validación de cédula/RUC ecuatoriano (diferida por el objetivo multipaís)
- Pruebas automatizadas unitarias y de integración (hoy: `scripts/smoke-test.sh`)
- Empaquetado con Docker (siguiente fase)
