# Frontend – Óptica Familiar

Angular 18 (componentes standalone) · Angular Material · Bootstrap 5 (solo grid y utilidades) · RxJS

## Desarrollo
```bash
cd frontend
npm install
npm start            # http://localhost:4200 (API en http://localhost:5130/api)
```
La URL de la API se define en `src/environments/`. En producción usa la ruta relativa `/api` (reverse proxy).

## Estructura
```
src/app/
  core/      modelos de la API, servicios HTTP, autenticación (JWT), interceptor y guardas por rol
  layout/    shell con menú lateral según rol
  shared/    diálogos y utilidades comunes
  features/  login, inicio, clientes, optometria, ordenes, ventas, productos, admin
```

## Notas
- Login con **código de empresa** + usuario + contraseña; el token y la empresa viajan en cada petición.
- El menú y las rutas se restringen por rol (`SUPERADMIN`, `ADMIN`, `VENDEDOR`, `OPTOMETRISTA`); la API es quien autoriza de verdad.
- Moneda e IVA se toman de la empresa autenticada (`GET /api/empresas/actual`).
- Los importes de la venta se previsualizan en el cliente, pero el servidor recalcula con precios e IVA oficiales.

## Comandos
```bash
npm run build        # build de producción en dist/
```
