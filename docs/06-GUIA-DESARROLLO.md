# ÓPTICA FAMILIAR

# Guía de desarrollo local

Entorno previsto: **WSL2** (Linux) para mantener el proyecto aislado de Windows. Todo corre dentro de WSL; el navegador
de Windows accede por `localhost`.

---

# Requisitos

| Herramienta | Versión |
|---|---|
| .NET SDK | 10 |
| Node.js / npm | 20 / 10 |
| Angular CLI | 18 (`npx ng` lo usa desde el proyecto) |
| PostgreSQL | 17 (escuchando en `127.0.0.1:5432`) |
| `dotnet-ef` | `dotnet tool install -g dotnet-ef` |
| curl, python3 | para `scripts/smoke-test.sh` |

---

# Primera configuración

1. Crear rol y base de datos en PostgreSQL:

```sql
CREATE ROLE optica_user LOGIN PASSWORD '<clave>';
CREATE DATABASE optica_familiar OWNER optica_user;
```

2. Configurar secretos (nunca se versionan):

```bash
cd backend/OpticaFamiliar.API
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=optica_familiar;Username=optica_user;Password=<clave>"
dotnet user-secrets set "Jwt:Key" "<mínimo 32 caracteres aleatorios>"
dotnet user-secrets set "Seed:AdminPassword" "<clave inicial de 'admin' de OPTICA-FAMILIAR>"
dotnet user-secrets set "Seed:SuperAdminPassword" "<clave inicial de 'superadmin'>"
dotnet user-secrets set "Seed:DemoPassword" "<clave de todos los usuarios demo>"
```

3. Instalar dependencias del frontend (la primera vez lo hace `dev.sh` si falta `node_modules`):

```bash
cd frontend && npm install
```

---

# Uso diario

```bash
./scripts/dev.sh start        # compila e inicia API y frontend
./scripts/dev.sh status
./scripts/dev.sh logs
./scripts/dev.sh stop
./scripts/dev.sh reset-demo   # borra y recarga solo las empresas demo
./scripts/dev.sh smoke        # verificación (equivale a ./scripts/smoke-test.sh)
```

| Servicio | Dirección |
|---|---|
| Frontend | http://localhost:4200 |
| API | http://localhost:5130 |
| Swagger | http://localhost:5130/swagger |
| PostgreSQL | `localhost:5432` · base `optica_familiar` |

En `Development` la API aplica las migraciones y carga los datos iniciales al arrancar.

## Acceso a PostgreSQL desde Windows

PostgreSQL escucha solo en loopback de WSL; WSL2 reenvía `localhost`, así que DBeaver o pgAdmin en Windows se conectan a
`localhost:5432` (si Windows tuviera su propio PostgreSQL en ese puerto habría conflicto).

## Probar la API

- **Swagger**: `POST /api/auth/login` (con `codigoEmpresa`), copiar el `token`, pulsar **Authorize** y pegarlo.
- **Postman**: importar `http://localhost:5130/swagger/v1/swagger.json`.
- **Script**: `./scripts/smoke-test.sh` (solo lectura). `SMOKE_WRITE=1 ./scripts/smoke-test.sh` recorre además el flujo
  completo y crea registros nuevos; después conviene `./scripts/dev.sh reset-demo`.

---

# Frontend: identidad visual

- Logo y recursos de marca en `frontend/public/brand/` (`optica-familiar.png`, `icon-192.png`, `favicon.png`).
- Paleta por defecto en `frontend/src/styles.scss` (variables `--brand*`); cada empresa la sobrescribe en ejecución.
- Si se actualiza Angular Material: `node frontend/scripts/generar-tokens-material.mjs` y revisar el resultado.
- En celular, las tablas se convierten en tarjetas: cada `<td>` lleva `data-label` con el nombre de su columna y el
  contenedor la clase `tabla-cards`.

---

# Migraciones

```bash
cd backend
dotnet ef migrations add <Nombre> --project OpticaFamiliar.Infrastructure --startup-project OpticaFamiliar.API
dotnet ef database update          --project OpticaFamiliar.Infrastructure --startup-project OpticaFamiliar.API
```

Si la migración agrega columnas obligatorias a tablas con datos, incluir un `UPDATE` de relleno antes de crear índices
y claves foráneas (ver `Multiempresa`).

---

# Configuración

| Clave | Dónde | Descripción |
|---|---|---|
| `ConnectionStrings:DefaultConnection` | secretos | Cadena de PostgreSQL |
| `Jwt:Key` | secretos | Clave de firma (≥ 32 bytes) |
| `Jwt:Issuer`, `Jwt:Audience`, `Jwt:ExpiraMinutos` | `appsettings.json` | Parámetros del token |
| `Cors:Origins` | `appsettings.json` | Orígenes permitidos (frontend) |
| `RateLimit:LoginPorMinuto` | `appsettings*.json` | Intentos de login por IP (10; 100 en desarrollo) |
| `Seed:AdminPassword`, `Seed:SuperAdminPassword` | secretos | Claves iniciales de las cuentas de arranque |
| `Seed:Demo`, `Seed:DemoPassword` | `appsettings.Development.json` / secretos | Carga de datos demo |

En el frontend la URL de la API está en `frontend/src/environments/` (`/api` en producción).

---

# Problemas frecuentes

| Síntoma | Causa y solución |
|---|---|
| Login responde 429 | Límite de intentos por IP; esperar un minuto |
| `Jwt:Key no está configurada` | Falta el secreto `Jwt:Key` |
| No se crea `admin` | Falta `Seed:AdminPassword`; se advierte en el log |
| No aparecen datos demo | Falta `Seed:DemoPassword` o `Seed:Demo` está en `false` |
| El navegador de Windows no abre `localhost:4200` | Verificar `./scripts/dev.sh status`; reiniciar WSL (`wsl --shutdown`) |
| Un `ERR` sobre `__EFMigrationsHistory` en el primer arranque de una base vacía | Normal: EF consulta la tabla de migraciones antes de crearla; no requiere acción |
| Puerto ocupado | `./scripts/dev.sh stop` y volver a iniciar |

---

# Reglas del repositorio

- No versionar contraseñas, claves ni cadenas de conexión; solo `user-secrets`/variables de entorno.
- No versionar `bin/`, `obj/`, `node_modules/`, `dist/`, `.angular/`, `.run/` ni archivos con datos reales
  (`docs/datos-*.xlsx`).
- Los datos reales se cargan por la aplicación o el seed; los demo solo en desarrollo.
