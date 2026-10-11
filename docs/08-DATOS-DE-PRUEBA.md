# ÓPTICA FAMILIAR

# Datos de demostración

Datos **ficticios** para desarrollar y probar sin tocar información real. Todos los nombres, cédulas, RUC, teléfonos y
correos son inventados (dominios `.example`).

## Separación respecto a datos reales

| Empresa | Código | Contenido |
|---|---|---|
| Plataforma (operación del SaaS) | `PLATAFORMA` | Usuario `superadmin` |
| Óptica Familiar (cliente real) | `OPTICA-FAMILIAR` | Sucursal MATRIZ (Quito, Pichincha) y usuario `admin`; **sin datos de negocio**, lista para cargar datos reales |
| Demo 1 | `DEMO-ANDINA` | Óptica Visión Andina, 3 sucursales |
| Demo 2 | `DEMO-SIERRA` | Óptica Sierra Norte, 1 sucursal |

Las contraseñas **no están en el repositorio**: se definen en `user-secrets` (`Seed:AdminPassword`,
`Seed:SuperAdminPassword`, `Seed:DemoPassword`). Todos los usuarios demo comparten `Seed:DemoPassword`.

## DEMO-ANDINA (Ecuador · USD · IVA 15 %)

Sucursales: **QUITO** (Pichincha), **GUAYAQUIL** (Guayas), **CUENCA** (Azuay).

| Usuario | Rol | Sucursal |
|---|---|---|
| `admin` | ADMIN | QUITO (ve todas) |
| `vend.quito`, `vend.guayaquil`, `vend.cuenca` | VENDEDOR | la de su nombre |
| `opto.quito`, `opto.guayaquil`, `opto.cuenca` | OPTOMETRISTA | la de su nombre |

Registros: 12 clientes, 9 productos (monturas, lentes, servicios), 9 exámenes (8 con receta), 8 órdenes de trabajo en
todos los estados, 9 ventas (pagadas, con saldo pendiente y una anulada) y sus pagos.

## DEMO-SIERRA (Ecuador · USD · IVA 15 %)

Sucursal **AMBATO** (Tungurahua). Usuarios `admin`, `vend.ambato`, `opto.ambato`.
Registros: 6 clientes, 5 productos (con precios distintos a Andina), 3 exámenes, 2 órdenes, 4 ventas.

El cliente «Juan Carlos Mora Salazar» (cédula `1700000011`) existe en **ambas** empresas como registros independientes:
sirve para comprobar el aislamiento.

## Cómo se cargan

- Al iniciar la API en `Development` con `Seed:Demo = true` y `Seed:DemoPassword` definido (`appsettings.Development.json`).
- Es **idempotente**: cada empresa demo se crea una vez (por su código) y su contenido solo si aún no tiene clientes;
  arrancar varias veces no duplica nada.
- Reiniciar solo lo demo (no toca empresas reales):

```bash
./scripts/dev.sh reset-demo
```

- El código está en `backend/OpticaFamiliar.Infrastructure/Seed/DemoDataSeeder.cs`. En producción `Seed:Demo` debe estar
  en `false` (el seed solo corre en `Development`).

## Datos reales de arranque

`docs/Plantilla-Datos-Iniciales.xlsx` indica qué información reunir (empresa, sucursales, usuarios, clientes y productos).
El archivo con datos reales no debe subirse a git (`docs/datos-*.xlsx` está ignorado).
