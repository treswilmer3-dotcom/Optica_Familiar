# Backend – Óptica Familiar

.NET 10 · EF Core 10 · PostgreSQL 17 · JWT · Swagger · Serilog

## Capas
`Domain` ← `Application` (DTOs + interfaces) ← `Infrastructure` (EF Core, servicios, JWT, BCrypt, seed) ← `API` (controllers, composición)

## Configuración local (secretos fuera de git)
```bash
cd backend/OpticaFamiliar.API
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=optica_familiar;Username=<usuario>;Password=<clave>"
dotnet user-secrets set "Jwt:Key" "<mínimo 32 caracteres aleatorios>"
dotnet user-secrets set "Seed:AdminPassword" "<contraseña inicial de 'admin'>"
```

## Ejecutar
```bash
dotnet run --project backend/OpticaFamiliar.API --urls http://localhost:5130
```
En `Development` aplica las migraciones y crea los datos iniciales (empresa, sucursal, roles, usuario `admin`, categorías).
Swagger: http://localhost:5130/swagger

## Roles
`ADMIN` (todo) · `VENDEDOR` (clientes, órdenes, ventas) · `OPTOMETRISTA` (clientes, exámenes visuales, recetas)

## Flujo Fase 1
`POST /api/auth/login` → `/api/clientes` → `/api/examenes-visuales` → `/api/recetas` → `/api/ordenes-trabajo` → `/api/ventas` → `/api/clientes/{id}/historial`
