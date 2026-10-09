ÓPTICA FAMILIAR
Arquitectura Oficial v1.0
Decisión Arquitectónica

Se utilizará una única base de datos PostgreSQL centralizada.
No existirán bases de datos independientes por sucursal.
La información estará segregada mediante:

Empresa
Sucursal

Modelo Organizacional
Plain Text 
Empresa
 └── Sucursal
 ├── Usuarios
 ├── Clientes
 ├── Inventario
 ├── Ventas
 ├── Compras
 ├── Reportes

Cliente Corporativo
Un cliente será único dentro de una empresa.
Podrá ser atendido en cualquier sucursal manteniendo:
Plain Text
Historial Clínico
Historial Comercial
Recetas
Exámenes
Ventas
Pagos

Inventario
Cada sucursal administrará su propio inventario.
Se permitirá:
Plain Text
Transferencias entre sucursales

Reportes
El Administrador General podrá consultar:
Ventas Consolidadas
Inventario Consolidado
Compras Consolidadas
Clientes Consolidados
Visión Comercial
El sistema será desarrollado para:
Óptica Familiar
Posteriormente será comercializado a terceros.
Por esta razón se adopta desde el inicio una arquitectura:
Multiempresa
Multisucursal
SaaS Ready

Restricciones
No se implementarán:
Microservicios
Mensajería distribuida
Arquitecturas complejas
Durante Fase 1.

Estado
APROBADO
CONGELADO PARA FASE 1
