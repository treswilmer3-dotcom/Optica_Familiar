using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpticaFamiliar.Application.Common;
using OpticaFamiliar.Application.Interfaces;
using OpticaFamiliar.Domain.Entities.HistoriaClinica;
using OpticaFamiliar.Domain.Entities.Organizacion;
using OpticaFamiliar.Domain.Entities.OrdenesTrabajo;
using OpticaFamiliar.Domain.Entities.Pagos;
using OpticaFamiliar.Domain.Entities.Personas;
using OpticaFamiliar.Domain.Entities.Productos;
using OpticaFamiliar.Domain.Entities.Ventas;
using OpticaFamiliar.Infrastructure.Data;
using OpticaFamiliar.Infrastructure.Services;

namespace OpticaFamiliar.Infrastructure.Seed;

/// <summary>
/// Datos ficticios para demostración y pruebas: dos empresas ecuatorianas independientes con sus sucursales,
/// usuarios por rol, catálogo, clientes, exámenes, recetas, órdenes de trabajo y ventas.
/// Es idempotente: cada empresa se crea una sola vez (por su código) y su contenido solo si aún no tiene clientes.
/// Los nombres, identificaciones y RUC son inventados.
/// </summary>
public class DemoDataSeeder
{
    private const string CodigoAndina = "DEMO-ANDINA";
    private const string CodigoSierra = "DEMO-SIERRA";
    public static readonly string[] CodigosDemo = [CodigoAndina, CodigoSierra];

    private readonly AppDbContext _db;
    private readonly IPasswordHasher _hasher;
    private readonly SeedOptions _opts;
    private readonly ILogger<DemoDataSeeder> _log;

    public DemoDataSeeder(AppDbContext db, IPasswordHasher hasher, IOptions<SeedOptions> opts, ILogger<DemoDataSeeder> log)
    {
        _db = db;
        _hasher = hasher;
        _opts = opts.Value;
        _log = log;
    }

    // ---- Definición de los datos -------------------------------------------------------------------------

    private record SucursalDef(string Codigo, string Nombre, string Direccion, string Ciudad, string Provincia, string Telefono);
    private record UsuarioDef(string Username, string Rol, string Sucursal, string Nombres, string Apellidos, string Cedula);
    private record ClienteDef(string Cedula, string Nombres, string Apellidos, string Nacimiento, string Genero, string Celular, string? Correo, string Direccion);
    private record ProductoDef(string Categoria, string Codigo, string Nombre, decimal Costo, decimal Precio, bool Formula);
    private record ExamenDef(int Cliente, string Opto, int Dias, string Motivo, string Diagnostico, decimal? OdEsf, decimal? OdCil, int? OdEje, decimal? OiEsf, decimal? OiCil, int? OiEje, decimal? Adicion, decimal? Dp);
    private record LineaDef(string Producto, int Cantidad, decimal Descuento = 0);
    private record PagoDef(string Metodo, decimal? Valor); // Valor null = saldo completo
    private record VentaDef(int Cliente, string Vendedor, int Dias, LineaDef[] Lineas, PagoDef[] Pagos, bool Anulada = false, int? Orden = null);
    private record OrdenDef(int Examen, string Vendedor, int Dias, string Estado, int EntregaEnDias, string Observaciones);

    private record EmpresaDef(
        string Codigo, string RazonSocial, string Nombre, string Ruc, string Direccion, string Telefono, string Correo,
        SucursalDef[] Sucursales, UsuarioDef[] Usuarios, ClienteDef[] Clientes, ProductoDef[] Productos,
        ExamenDef[] Examenes, OrdenDef[] Ordenes, VentaDef[] Ventas,
        string ColorPrimario, string ColorSecundario);

    private static readonly EmpresaDef Andina = new(
        CodigoAndina, "Visión Andina Cía. Ltda. (DEMO)", "Óptica Visión Andina", "1790000001001",
        "Av. Amazonas N35-12 y Japón, Quito", "022550100", "demo@visionandina.example",
        [
            new("QUITO", "Visión Andina Quito", "Av. Amazonas N35-12 y Japón", "Quito", "Pichincha", "022550101"),
            new("GUAYAQUIL", "Visión Andina Guayaquil", "Av. Francisco de Orellana, Mall del Sol", "Guayaquil", "Guayas", "042690102"),
            new("CUENCA", "Visión Andina Cuenca", "Av. Remigio Crespo y Federico Proaño", "Cuenca", "Azuay", "072830103")
        ],
        [
            new("admin", "ADMIN", "QUITO", "Patricia", "Villacís Andrade", "1700000101"),
            new("vend.quito", "VENDEDOR", "QUITO", "Marcelo", "Tapia Lozada", "1700000102"),
            new("vend.guayaquil", "VENDEDOR", "GUAYAQUIL", "Jessenia", "Macías Bravo", "0900000103"),
            new("vend.cuenca", "VENDEDOR", "CUENCA", "Andrés", "Peñafiel Ordóñez", "0100000104"),
            new("opto.quito", "OPTOMETRISTA", "QUITO", "Carolina", "Reyes Almeida", "1700000105"),
            new("opto.guayaquil", "OPTOMETRISTA", "GUAYAQUIL", "Ricardo", "Zúñiga Cedeño", "0900000106"),
            new("opto.cuenca", "OPTOMETRISTA", "CUENCA", "Daniela", "Arévalo Cordero", "0100000107")
        ],
        [
            new("1700000011", "Juan Carlos", "Mora Salazar", "1985-03-14", "M", "0991000011", "juan.mora@correo.example", "Calle Los Pinos E5-20, Quito"),
            new("1700000012", "Lucía Fernanda", "Cevallos Ruiz", "1992-11-02", "F", "0991000012", "lucia.cevallos@correo.example", "Cumbayá, Quito"),
            new("1700000013", "Pedro Pablo", "Guamán Quispe", "1978-07-21", "M", "0991000013", null, "Calderón, Quito"),
            new("1700000014", "Ana María", "Toapanta Chicaiza", "1969-01-30", "F", "0991000014", "ana.toapanta@correo.example", "Sangolquí, Quito"),
            new("1700000015", "Diego Armando", "Vásquez León", "2001-09-09", "M", "0991000015", "diego.vasquez@correo.example", "La Carolina, Quito"),
            new("1700000016", "Gabriela Estefanía", "Proaño Neira", "1988-05-17", "F", "0991000016", "gaby.proano@correo.example", "Cotocollao, Quito"),
            new("1700000017", "Mateo Sebastián", "Ortiz Rueda", "2014-12-05", "M", "0991000017", null, "El Batán, Quito"),
            new("1700000018", "Rosa Elena", "Pilatuña Cuji", "1955-04-11", "F", "0991000018", null, "Chillogallo, Quito"),
            new("0900000019", "Fernando José", "Zambrano Intriago", "1990-08-25", "M", "0991000019", "fernando.zambrano@correo.example", "Urdesa, Guayaquil"),
            new("0900000020", "Karla Daniela", "Mendoza Villamar", "1996-02-19", "F", "0991000020", "karla.mendoza@correo.example", "Samborondón, Guayaquil"),
            new("0100000021", "Santiago Andrés", "Pacheco Crespo", "1983-10-03", "M", "0991000021", "santiago.pacheco@correo.example", "El Batán, Cuenca"),
            new("0100000022", "Verónica Alexandra", "Illescas Barzallo", "1975-06-28", "F", "0991000022", null, "Totoracocha, Cuenca")
        ],
        [
            new("Monturas", "MON-001", "Montura metálica clásica", 18, 45, false),
            new("Monturas", "MON-002", "Montura de acetato premium", 38, 85, false),
            new("Monturas", "MON-003", "Montura infantil flexible", 14, 35, false),
            new("Lentes", "LEN-001", "Lentes CR-39 antirreflejo (par)", 15, 40, true),
            new("Lentes", "LEN-002", "Lentes policarbonato blue-block (par)", 30, 70, true),
            new("Lentes", "LEN-003", "Lentes progresivos (par)", 70, 150, true),
            new("Lentes", "LEN-004", "Lentes fotocromáticos (par)", 42, 95, true),
            new("Servicios", "SER-001", "Examen visual", 0, 15, false),
            new("Servicios", "SER-002", "Ajuste y mantenimiento", 0, 5, false)
        ],
        [
            new(0, "opto.quito", 55, "Visión borrosa de lejos", "Miopía leve", -1.50m, -0.50m, 90, -1.25m, null, null, null, 62),
            new(1, "opto.quito", 48, "Cansancio visual en computador", "Astigmatismo leve", 0.25m, -0.75m, 180, 0.50m, -0.50m, 170, null, 61),
            new(2, "opto.quito", 40, "Dificultad para leer de cerca", "Presbicia", 0.50m, null, null, 0.75m, null, null, 1.50m, 64),
            new(3, "opto.quito", 33, "Control anual", "Presbicia y astigmatismo", 1.00m, -1.00m, 85, 1.25m, -0.75m, 95, 2.00m, 63),
            new(6, "opto.quito", 20, "Dolor de cabeza al leer", "Hipermetropía", 2.00m, null, null, 2.25m, null, null, null, 54),
            new(8, "opto.guayaquil", 27, "Visión borrosa de lejos", "Miopía moderada", -3.00m, -1.00m, 10, -2.75m, -0.75m, 175, null, 65),
            new(9, "opto.guayaquil", 12, "Control", "Sin alteraciones significativas", null, null, null, null, null, null, null, null),
            new(10, "opto.cuenca", 18, "Cansancio visual", "Miopía leve", -0.75m, null, null, -1.00m, null, null, null, 62),
            new(11, "opto.cuenca", 6, "Dificultad para leer de cerca", "Presbicia", 1.50m, -0.25m, 90, 1.50m, null, null, 2.25m, 60)
        ],
        [
            new(0, "vend.quito", 53, "ENTREGADA", -45, "Lentes CR-39 antirreflejo con montura clásica"),
            new(1, "vend.quito", 46, "ENTREGADA", -38, "Lentes policarbonato blue-block"),
            new(2, "vend.quito", 38, "TERMINADA", -30, "Progresivos: llamar al cliente al terminar"),
            new(3, "vend.quito", 31, "EN_LABORATORIO", 5, "Lentes fotocromáticos"),
            new(4, "vend.quito", 18, "EN_PRODUCCION", 7, "Montura infantil, lentes CR-39"),
            new(5, "vend.guayaquil", 25, "ENTREGADA", -18, "Lentes policarbonato, montura de acetato"),
            new(7, "vend.cuenca", 16, "CREADA", 10, "Pendiente de aprobación del cliente"),
            new(8, "vend.cuenca", 4, "CREADA", 9, "Progresivos con tratamiento antirreflejo")
        ],
        [
            new(0, "vend.quito", 53, [new("MON-001", 1), new("LEN-001", 1)], [new("EFECTIVO", null)], Orden: 0),
            new(1, "vend.quito", 46, [new("MON-002", 1), new("LEN-002", 1, 5)], [new("TARJETA", null)], Orden: 1),
            new(2, "vend.quito", 38, [new("MON-002", 1), new("LEN-003", 1)], [new("TRANSFERENCIA", 150), new("EFECTIVO", null)], Orden: 2),
            new(3, "vend.quito", 31, [new("MON-001", 1), new("LEN-004", 1)], [new("EFECTIVO", 60)], Orden: 3),
            new(6, "vend.quito", 18, [new("MON-003", 1), new("LEN-001", 1)], [new("TARJETA", 30)], Orden: 4),
            new(5, "vend.quito", 9, [new("SER-001", 1), new("SER-002", 1)], [new("EFECTIVO", null)]),
            new(8, "vend.guayaquil", 25, [new("MON-002", 1), new("LEN-002", 1)], [new("TARJETA", null)], Orden: 5),
            new(9, "vend.guayaquil", 10, [new("MON-001", 2)], [new("EFECTIVO", null)], Anulada: true),
            new(10, "vend.cuenca", 3, [new("SER-001", 1)], [new("EFECTIVO", null)])
        ], "#2E6F4E", "#F2A33A");

    private static readonly EmpresaDef Sierra = new(
        CodigoSierra, "Óptica Sierra Norte S.A. (DEMO)", "Óptica Sierra Norte", "1890000002001",
        "Av. Cevallos y Mera, Ambato", "032420200", "demo@sierranorte.example",
        [new("AMBATO", "Sierra Norte Ambato", "Av. Cevallos y Mera", "Ambato", "Tungurahua", "032420201")],
        [
            new("admin", "ADMIN", "AMBATO", "Hernán", "Cisneros Bonilla", "1800000201"),
            new("vend.ambato", "VENDEDOR", "AMBATO", "Lorena", "Sánchez Yánez", "1800000202"),
            new("opto.ambato", "OPTOMETRISTA", "AMBATO", "Esteban", "Freire Mera", "1800000203")
        ],
        [
            // El primer cliente comparte identificación con uno de Visión Andina: son registros independientes por empresa.
            new("1700000011", "Juan Carlos", "Mora Salazar", "1985-03-14", "M", "0992000031", "juan.mora@correo.example", "Av. Los Andes, Ambato"),
            new("1800000032", "Mercedes del Carmen", "Sánchez Yánez", "1962-09-12", "F", "0992000032", null, "Ficoa, Ambato"),
            new("1800000033", "Byron Patricio", "Lascano Mera", "1987-01-23", "M", "0992000033", "byron.lascano@correo.example", "Huachi Chico, Ambato"),
            new("1800000034", "Silvia Marcela", "Tapia Freire", "1994-06-30", "F", "0992000034", "silvia.tapia@correo.example", "Atocha, Ambato"),
            new("1800000035", "Wilson Gonzalo", "Llerena Bonilla", "1971-11-15", "M", "0992000035", null, "Pelileo, Tungurahua"),
            new("1800000036", "Jessica Paola", "Acosta Vaca", "2008-04-08", "F", "0992000036", null, "Izamba, Ambato")
        ],
        [
            new("Monturas", "MON-001", "Montura ligera de titanio", 25, 60, false),
            new("Monturas", "MON-002", "Montura deportiva", 20, 48, false),
            new("Lentes", "LEN-001", "Lentes antirreflejo (par)", 18, 42, true),
            new("Lentes", "LEN-002", "Lentes bifocales (par)", 45, 98, true),
            new("Servicios", "SER-001", "Examen visual", 0, 12, false)
        ],
        [
            new(0, "opto.ambato", 30, "Visión borrosa de lejos", "Miopía leve", -1.00m, null, null, -1.25m, -0.25m, 80, null, 63),
            new(2, "opto.ambato", 14, "Dificultad para leer de cerca", "Presbicia", 1.75m, null, null, 1.75m, null, null, 2.00m, 62),
            new(5, "opto.ambato", 5, "Control escolar", "Hipermetropía leve", 1.00m, null, null, 1.25m, null, null, null, 56)
        ],
        [
            new(0, "vend.ambato", 28, "ENTREGADA", -21, "Lentes antirreflejo y montura de titanio"),
            new(1, "vend.ambato", 12, "EN_PRODUCCION", 6, "Lentes bifocales")
        ],
        [
            new(0, "vend.ambato", 28, [new("MON-001", 1), new("LEN-001", 1)], [new("EFECTIVO", null)], Orden: 0),
            new(2, "vend.ambato", 12, [new("MON-002", 1), new("LEN-002", 1)], [new("TARJETA", 50)], Orden: 1),
            new(3, "vend.ambato", 7, [new("SER-001", 1)], [new("EFECTIVO", null)]),
            new(4, "vend.ambato", 2, [new("MON-002", 1)], [], Anulada: true)
        ], "#8A4B14", "#E0A030");

    // ---- Carga -------------------------------------------------------------------------------------------

    public async Task SeedAsync(CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(_opts.DemoPassword))
        {
            _log.LogWarning("Seed:Demo está activo pero falta Seed:DemoPassword: no se cargan datos demo.");
            return;
        }
        foreach (var def in new[] { Andina, Sierra })
        {
            await using var tx = await _db.Database.BeginTransactionAsync(ct);
            if (await CargarEmpresa(def, ct)) _log.LogInformation("Datos demo cargados: {Empresa}", def.Codigo);
            await tx.CommitAsync(ct);
        }
    }

    /// <summary>Elimina únicamente las empresas demo y todo lo que les pertenece. No toca empresas reales.</summary>
    public async Task ResetAsync(CancellationToken ct = default)
    {
        var ids = await _db.Empresas.IgnoreQueryFilters().Where(e => CodigosDemo.Contains(e.Codigo)).Select(e => e.Id).ToListAsync(ct);
        if (ids.Count == 0) return;
        await using var tx = await _db.Database.BeginTransactionAsync(ct);
        // Orden: de las tablas hijas hacia las padres.
        string[] tablas =
        [
            "pago", "venta_detalle", "orden_trabajo", "venta", "receta", "consulta", "cita", "historia_clinica", "paciente",
            "optometrista", "cliente", "usuario", "persona", "producto", "categoria_producto", "numeracion_documento",
            "sucursal_configuracion", "sucursal", "empresa_configuracion"
        ];
        // Los nombres de tabla provienen de la lista constante de arriba (no de entrada externa); el valor va parametrizado.
#pragma warning disable EF1002
        foreach (var t in tablas)
            await _db.Database.ExecuteSqlRawAsync($"DELETE FROM \"{t}\" WHERE empresa_id = ANY({{0}})", [ids.ToArray()], ct);
#pragma warning restore EF1002
        await _db.Database.ExecuteSqlRawAsync("DELETE FROM empresa WHERE id = ANY({0})", [ids.ToArray()], ct);
        await tx.CommitAsync(ct);
        _log.LogInformation("Datos demo eliminados ({Cantidad} empresas).", ids.Count);
    }

    /// <summary>Devuelve false si la empresa ya tenía datos demo (no se repite la carga).</summary>
    private async Task<bool> CargarEmpresa(EmpresaDef d, CancellationToken ct)
    {
        var empresa = await _db.Empresas.IgnoreQueryFilters().FirstOrDefaultAsync(e => e.Codigo == d.Codigo, ct);
        if (empresa == null)
        {
            empresa = new Empresa
            {
                Codigo = d.Codigo, RazonSocial = d.RazonSocial, NombreComercial = d.Nombre, IdentificacionFiscal = d.Ruc,
                Direccion = d.Direccion, Telefono = d.Telefono, Correo = d.Correo, Pais = "EC", Moneda = "USD",
                ZonaHoraria = "America/Guayaquil", Idioma = "es", IvaPorcentaje = 15m
            };
            _db.Empresas.Add(empresa);
            await _db.SaveChangesAsync(ct);
        }
        await DbSeeder.AsegurarMarcaAsync(_db, empresa.Id, d.ColorPrimario, d.ColorSecundario, null, ct);
        if (await _db.Clientes.IgnoreQueryFilters().AnyAsync(c => c.EmpresaId == empresa.Id, ct)) return false;

        var eid = empresa.Id;

        // Sucursales + numeración
        var sucursales = new Dictionary<string, Sucursal>();
        foreach (var s in d.Sucursales)
        {
            var suc = new Sucursal { EmpresaId = eid, Codigo = s.Codigo, Nombre = s.Nombre, Direccion = s.Direccion, Ciudad = s.Ciudad, Provincia = s.Provincia, Telefono = s.Telefono };
            _db.Sucursales.Add(suc);
            sucursales[s.Codigo] = suc;
        }
        await _db.SaveChangesAsync(ct);
        var contadores = new Dictionary<(long, string), NumeracionCtr>();
        foreach (var suc in sucursales.Values)
            foreach (var tipo in NumeracionDefecto.Tipos)
            {
                var n = NumeracionDefecto.Crear(suc.Id, suc.Codigo, tipo);
                n.EmpresaId = eid;
                _db.NumeracionDocumentos.Add(n);
                contadores[(suc.Id, tipo)] = new NumeracionCtr(n);
            }

        // Categorías base y catálogo
        var categorias = Aprovisionamiento.CategoriasBase(eid).ToList();
        _db.CategoriaProductos.AddRange(categorias);
        await _db.SaveChangesAsync(ct);
        var productos = new Dictionary<string, Producto>();
        foreach (var p in d.Productos)
        {
            var cat = categorias.First(c => c.Nombre == p.Categoria);
            var prod = new Producto { EmpresaId = eid, CategoriaId = cat.Id, Codigo = p.Codigo, Nombre = p.Nombre, Costo = p.Costo, Precio = p.Precio, RequiereFormula = p.Formula };
            _db.Productos.Add(prod);
            productos[p.Codigo] = prod;
        }

        // Usuarios (y optometristas)
        var roles = await _db.Roles.ToDictionaryAsync(r => r.Codigo, ct);
        var usuarios = new Dictionary<string, Usuario>();
        var optometristas = new Dictionary<string, Optometrista>();
        foreach (var u in d.Usuarios)
        {
            var persona = new Persona { EmpresaId = eid, TipoIdentificacion = "CEDULA", NumeroIdentificacion = u.Cedula, Nombres = u.Nombres, Apellidos = u.Apellidos, Correo = $"{u.Username}@demo.example" };
            var usuario = new Usuario { EmpresaId = eid, Persona = persona, RolId = roles[u.Rol].Id, SucursalId = sucursales[u.Sucursal].Id, Username = u.Username, PasswordHash = _hasher.Hash(_opts.DemoPassword!) };
            _db.Usuarios.Add(usuario);
            usuarios[u.Username] = usuario;
        }
        await _db.SaveChangesAsync(ct);
        foreach (var u in d.Usuarios.Where(x => x.Rol == Roles.Optometrista))
        {
            var opto = new Optometrista { EmpresaId = eid, PersonaId = usuarios[u.Username].PersonaId!.Value, UsuarioId = usuarios[u.Username].Id, NumeroRegistro = $"SENESCYT-DEMO-{u.Cedula[^3..]}", Estado = Estados.Activo };
            _db.Optometristas.Add(opto);
            optometristas[u.Username] = opto;
        }

        // Clientes
        var clientes = new List<Cliente>();
        foreach (var c in d.Clientes)
        {
            var cliente = new Cliente
            {
                EmpresaId = eid, FechaRegistro = DateTime.UtcNow.AddDays(-60),
                Persona = new Persona
                {
                    EmpresaId = eid, TipoIdentificacion = "CEDULA", NumeroIdentificacion = c.Cedula, Nombres = c.Nombres, Apellidos = c.Apellidos,
                    FechaNacimiento = DateTime.SpecifyKind(DateTime.Parse(c.Nacimiento), DateTimeKind.Utc), Genero = c.Genero, Celular = c.Celular, Correo = c.Correo, Direccion = c.Direccion
                }
            };
            _db.Clientes.Add(cliente);
            clientes.Add(cliente);
        }
        await _db.SaveChangesAsync(ct);

        // Exámenes visuales + recetas (paciente e historia por cliente)
        var historias = new Dictionary<int, HistoriaClinica>();
        var consultas = new List<(Consulta Consulta, ExamenDef Def)>();
        foreach (var e in d.Examenes)
        {
            if (!historias.TryGetValue(e.Cliente, out var historia))
            {
                var paciente = new Paciente { EmpresaId = eid, PersonaId = clientes[e.Cliente].PersonaId, Estado = Estados.Activo };
                _db.Pacientes.Add(paciente);
                await _db.SaveChangesAsync(ct);
                var sucOpto = usuarios[e.Opto].SucursalId;
                historia = new HistoriaClinica
                {
                    EmpresaId = eid, PacienteId = paciente.Id, NumeroHistoria = Siguiente(contadores, sucOpto, TiposDocumento.HistoriaClinica),
                    FechaApertura = DateTime.UtcNow.AddDays(-e.Dias), Estado = Estados.Activo
                };
                _db.HistoriasClinicas.Add(historia);
                await _db.SaveChangesAsync(ct);
                historias[e.Cliente] = historia;
            }
            var consulta = new Consulta
            {
                EmpresaId = eid, HistoriaClinicaId = historia.Id, OptometristaId = optometristas[e.Opto].Id,
                FechaConsulta = DateTime.UtcNow.AddDays(-e.Dias), MotivoConsulta = e.Motivo, Diagnostico = e.Diagnostico,
                Recomendaciones = "Control anual. Uso de protección UV."
            };
            _db.Consultas.Add(consulta);
            consultas.Add((consulta, e));
        }
        await _db.SaveChangesAsync(ct);
        var recetas = new Dictionary<int, Receta>();
        for (var i = 0; i < consultas.Count; i++)
        {
            var (c, e) = consultas[i];
            if (e.OdEsf == null && e.OiEsf == null) continue; // examen sin corrección
            var receta = new Receta
            {
                EmpresaId = eid, ConsultaId = c.Id, FechaEmision = c.FechaConsulta,
                OdEsfera = e.OdEsf, OdCilindro = e.OdCil, OdEje = e.OdEje, OdAdicion = e.Adicion,
                OiEsfera = e.OiEsf, OiCilindro = e.OiCil, OiEje = e.OiEje, OiAdicion = e.Adicion, DistanciaPupilar = e.Dp
            };
            _db.Recetas.Add(receta);
            recetas[i] = receta;
        }
        await _db.SaveChangesAsync(ct);

        // Órdenes de trabajo
        var ordenes = new List<OrdenTrabajo>();
        foreach (var o in d.Ordenes)
        {
            var ingreso = DateTime.UtcNow.AddDays(-o.Dias);
            var orden = new OrdenTrabajo
            {
                EmpresaId = eid, RecetaId = recetas[o.Examen].Id, NumeroOrden = Siguiente(contadores, usuarios[o.Vendedor].SucursalId, TiposDocumento.OrdenTrabajo),
                FechaIngreso = ingreso, FechaEntregaEstimada = DateTime.UtcNow.AddDays(o.EntregaEnDias), Estado = o.Estado, Observaciones = o.Observaciones,
                FechaEntregaReal = o.Estado == Estados.OrdenEntregada ? DateTime.UtcNow.AddDays(o.EntregaEnDias) : null
            };
            _db.OrdenesTrabajo.Add(orden);
            ordenes.Add(orden);
        }
        await _db.SaveChangesAsync(ct);

        // Ventas con detalle y pagos
        foreach (var v in d.Ventas)
        {
            var detalles = v.Lineas.Select(l =>
            {
                var p = productos[l.Producto];
                return new VentaDetalle { EmpresaId = eid, ProductoId = p.Id, Cantidad = l.Cantidad, PrecioUnitario = p.Precio, Descuento = l.Descuento, Subtotal = p.Precio * l.Cantidad - l.Descuento };
            }).ToList();
            var subtotal = detalles.Sum(x => x.Subtotal);
            var iva = Math.Round(subtotal * empresa.IvaPorcentaje / 100m, 2);
            var total = subtotal + iva;
            var fecha = DateTime.UtcNow.AddDays(-v.Dias);
            var pagos = new List<Pago>();
            decimal pagado = 0;
            foreach (var p in v.Pagos)
            {
                var valor = p.Valor ?? total - pagado;
                pagado += valor;
                pagos.Add(new Pago { EmpresaId = eid, MetodoPago = p.Metodo, Valor = valor, FechaPago = fecha, Referencia = p.Metodo == "TRANSFERENCIA" ? "TRF-DEMO-" + (1000 + v.Dias) : null });
            }
            var vendedor = usuarios[v.Vendedor];
            var venta = new Venta
            {
                EmpresaId = eid, ClienteId = clientes[v.Cliente].Id, UsuarioId = vendedor.Id, SucursalId = vendedor.SucursalId,
                NumeroFactura = Siguiente(contadores, vendedor.SucursalId, TiposDocumento.Venta), FechaVenta = fecha,
                Subtotal = subtotal, Descuento = detalles.Sum(x => x.Descuento), Iva = iva, Total = total,
                Estado = v.Anulada ? Estados.VentaAnulada : pagado >= total ? Estados.VentaPagada : Estados.VentaPendiente,
                VentaDetalles = detalles, Pagos = pagos
            };
            _db.Ventas.Add(venta);
            if (v.Orden is int idx) ordenes[idx].Venta = venta;
        }

        await _db.SaveChangesAsync(ct);
        return true;
    }

    private sealed class NumeracionCtr
    {
        private readonly Domain.Entities.Configuracion.NumeracionDocumento _fila;
        public NumeracionCtr(Domain.Entities.Configuracion.NumeracionDocumento fila) => _fila = fila;
        public string Siguiente() { _fila.NumeroActual++; return $"{_fila.Serie}-{_fila.NumeroActual:D9}"; }
    }

    // Numeración local del seed (no usa el bloqueo de BD: no hay concurrencia aquí).
    private static string Siguiente(Dictionary<(long, string), NumeracionCtr> c, long sucursalId, string tipo) => c[(sucursalId, tipo)].Siguiente();
}
