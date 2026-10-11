# ÓPTICA FAMILIAR

# Stack Tecnológico Oficial v1.1

## Estado

IMPLEMENTADO en Fase 1 (las versiones reflejan lo que está instalado y probado)

---

# Backend

| Componente | Versión | Uso |
|---|---|---|
| .NET / ASP.NET Core Web API | 10 | API REST |
| Entity Framework Core | 10.0.12 | ORM y migraciones |
| Npgsql EF Core Provider | 10.0.3 | Acceso a PostgreSQL |
| JWT Bearer (`Microsoft.AspNetCore.Authentication.JwtBearer`) | 10.0.12 | Autenticación |
| `System.IdentityModel.Tokens.Jwt` | 8.23.0 | Emisión de tokens |
| BCrypt.Net-Next | 4.2.0 | Hash de contraseñas |
| Swashbuckle.AspNetCore (Swagger UI) | 10.3.0 | Documentación interactiva de la API |
| Serilog.AspNetCore | 10.0.0 | Registro de eventos |

---

# Frontend

| Componente | Versión | Uso |
|---|---|---|
| Angular (componentes standalone) | 18.2 | Aplicación web |
| Angular Material | 18.2 | Componentes de interfaz |
| Bootstrap | 5.3 | Solo grilla y utilidades (`bootstrap-grid` / `bootstrap-utilities`) |
| TypeScript / RxJS | 5.5 / 7.8 | Lenguaje y programación reactiva |
| Node.js / npm | 20 / 10 | Herramientas de compilación |

---

# Base de Datos

- PostgreSQL 17
- Migraciones con Entity Framework (`backend/OpticaFamiliar.Infrastructure/Migrations`)
- Esquema único compartido entre empresas, con `empresa_id` (ver `02-ARQUITECTURA-OFICIAL.md`)

---

# Seguridad

- JWT con claims de usuario, empresa, sucursal y rol
- RBAC por rol: `SUPERADMIN`, `ADMIN`, `VENDEDOR`, `OPTOMETRISTA`
- Contraseñas con BCrypt (factor 11)
- Límite de intentos de login por IP (configurable)
- CORS restringido a los orígenes configurados
- Secretos fuera del repositorio (`dotnet user-secrets` / variables de entorno)
- Auditoría de operaciones: modelada (`auditoria`), **pendiente de implementar**

---

# Herramientas de desarrollo

- Git y GitHub
- WSL2 (AlmaLinux) como entorno aislado de desarrollo
- `scripts/dev.sh` para levantar, detener y probar el entorno
- `scripts/smoke-test.sh` para verificar API, datos y permisos
- Docker y Docker Compose: **siguiente fase** (empaquetado y despliegue)

---

# Arquitectura

Monolito modular: Frontend + API + Base de Datos.

---

# Modelo de Datos

Multiempresa y multisucursal en una sola base de datos.

---

# Principios Arquitectónicos

- Escalabilidad
- Mantenibilidad
- Seguridad
- Trazabilidad
- Auditoría
- SaaS Ready

---

# Alcance

Se prioriza velocidad de desarrollo sobre complejidad arquitectónica.
No se implementan microservicios.
