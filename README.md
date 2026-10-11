# Óptica Familiar

Sistema de gestión para ópticas: clientes, historia clínica (examen visual y receta), órdenes de trabajo y ventas.
**Multiempresa y multisucursal**, pensado para comercializarse como SaaS (Ecuador primero).

- Backend: .NET 10 · ASP.NET Core · EF Core · PostgreSQL 17 · JWT
- Frontend: Angular 18 · Angular Material
- Entorno de desarrollo: WSL2 (aislado de Windows)

## Inicio rápido

```bash
# 1) Configurar secretos una sola vez (ver docs/06-GUIA-DESARROLLO.md)
# 2) Levantar API y frontend
./scripts/dev.sh start
# 3) Abrir http://localhost:4200  (API y Swagger: http://localhost:5130/swagger)
```

Ingreso: **código de empresa + usuario + contraseña**. Con los datos demo: empresa `DEMO-ANDINA`, usuario `admin`
(contraseña en `Seed:DemoPassword`).

```bash
./scripts/dev.sh status | stop | logs | reset-demo | smoke
```

## Estructura

```text
backend/    API .NET en capas (Domain · Application · Infrastructure · API)
frontend/   Aplicación Angular
scripts/    dev.sh (entorno local) · smoke-test.sh (verificación)
docs/       Documentación de arquitectura, diseño, API y guías
```

## Documentación

| Documento | Contenido |
|---|---|
| [01 Stack tecnológico](docs/01-STACK-TECNOLOGICO.md) | Tecnologías y versiones |
| [02 Arquitectura](docs/02-ARQUITECTURA-OFICIAL.md) | Capas, multiempresa, seguridad, estado y roadmap |
| [03 Fase 1](docs/03-FASE-1.md) | Alcance entregado y pendientes |
| [04 Modelo conceptual](docs/04-Modelo%20Conceptual.md) · [05 Modelo lógico](docs/05-Modelo-Logico.md) · [Diagrama ER](docs/Diagrama-ER.md) | Diseño de datos |
| [06 Guía de desarrollo](docs/06-GUIA-DESARROLLO.md) | Instalación, configuración, migraciones, problemas frecuentes |
| [07 API](docs/07-API.md) | Endpoints, roles y reglas |
| [08 Datos de prueba](docs/08-DATOS-DE-PRUEBA.md) | Empresas y usuarios demo |

## Estado

Fase 1 completada y validada. Siguiente: afinamiento de interfaz (logo, colores, responsive), cambio de credenciales y
empaquetado con Docker. Ver el roadmap en `docs/02-ARQUITECTURA-OFICIAL.md`.
