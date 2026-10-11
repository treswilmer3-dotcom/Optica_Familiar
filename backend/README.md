# Backend – Óptica Familiar

.NET 10 · EF Core 10 · PostgreSQL 17 · JWT · Swagger · Serilog

## Capas
`Domain` ← `Application` (DTOs + interfaces) ← `Infrastructure` (EF Core, servicios, JWT, BCrypt, seed) ← `API` (controllers, composición)

## Configuración local (secretos fuera de git)
```bash
cd backend/OpticaFamiliar.API
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=optica_familiar;Username=<usuario>;Password=<clave>"
dotnet user-secrets set "Jwt:Key" "<mínimo 32 caracteres aleatorios>"
dotnet user-secrets set "Seed:AdminPassword" "<contraseña inicial del 'admin' de OPTICA-FAMILIAR>"
dotnet user-secrets set "Seed:SuperAdminPassword" "<contraseña inicial del 'superadmin' de la plataforma>"
```

## Ejecutar
```bash
dotnet run --project backend/OpticaFamiliar.API --urls http://localhost:5130
```
En `Development` aplica las migraciones y crea los datos iniciales (empresa `PLATAFORMA` con `superadmin`, empresa `OPTICA-FAMILIAR` con sucursal `MATRIZ`, roles, usuario `admin`, categorías).
Swagger: http://localhost:5130/swagger

## Multiempresa
Login: `POST /api/auth/login` con `codigoEmpresa`, `username` y `password`. La empresa viaja en el JWT y el `DbContext`
filtra todo por empresa (ver `docs/05-Modelo-Logico.md`, sección Multiempresa).

Alta de una empresa nueva (como `superadmin`): `POST /api/empresas` → `POST /api/sucursales` (con `empresaId`) →
`POST /api/usuarios` (con `empresaId`, rol `ADMIN`). A partir de ahí el administrador de la empresa gestiona lo suyo.

## Roles
`SUPERADMIN` (plataforma: empresas, sin datos clínicos/comerciales) · `ADMIN` (su empresa, todas las sucursales) ·
`VENDEDOR` (clientes, órdenes, ventas) · `OPTOMETRISTA` (clientes, exámenes visuales, recetas)

## Flujo Fase 1
`POST /api/auth/login` → `/api/clientes` → `/api/examenes-visuales` → `/api/recetas` → `/api/ordenes-trabajo` → `/api/ventas` → `/api/clientes/{id}/historial`
