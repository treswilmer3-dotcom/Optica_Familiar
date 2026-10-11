using Microsoft.EntityFrameworkCore;
using OpticaFamiliar.Application.Common;
using OpticaFamiliar.Application.DTOs;
using OpticaFamiliar.Application.Interfaces;
using OpticaFamiliar.Domain.Entities.AgendaMedica;
using OpticaFamiliar.Domain.Entities.HistoriaClinica;
using OpticaFamiliar.Domain.Entities.Personas;
using OpticaFamiliar.Infrastructure.Data;

namespace OpticaFamiliar.Infrastructure.Services;

/// <summary>Examen visual = Consulta dentro de la Historia Clínica del paciente (la persona del cliente).</summary>
public class ExamenVisualService : IExamenVisualService
{
    private readonly AppDbContext _db;
    private readonly ICurrentUser _current;
    private readonly NumeradorDocumentos _numerador;

    public ExamenVisualService(AppDbContext db, ICurrentUser current)
    {
        _db = db;
        _current = current;
        _numerador = new NumeradorDocumentos(db);
    }

    public async Task<ExamenVisualDto> CrearAsync(ExamenVisualRequest r, CancellationToken ct = default)
    {
        var cliente = await _db.Clientes.Include(c => c.Persona)
            .FirstOrDefaultAsync(c => c.Id == r.ClienteId && c.Persona.Estado == Estados.Activo, ct)
            ?? throw new NotFoundException("Cliente no encontrado.");

        var optometristaId = r.OptometristaId
            ?? await _db.Optometristas.Where(o => o.UsuarioId == _current.UserId).Select(o => (long?)o.Id).FirstOrDefaultAsync(ct)
            ?? throw new BusinessRuleException("Indique el optometrista: el usuario actual no tiene un perfil de optometrista.");
        if (!await _db.Optometristas.AnyAsync(o => o.Id == optometristaId, ct))
            throw new BusinessRuleException("El optometrista indicado no existe.");

        Cita? cita = null;
        if (r.CitaId.HasValue)
        {
            cita = await _db.Citas.Include(c => c.Paciente).FirstOrDefaultAsync(c => c.Id == r.CitaId, ct)
                ?? throw new BusinessRuleException("La cita indicada no existe.");
            if (cita.Paciente.PersonaId != cliente.PersonaId)
                throw new BusinessRuleException("La cita no corresponde a este cliente.");
            if (await _db.Consultas.AnyAsync(c => c.CitaId == cita.Id, ct))
                throw new BusinessRuleException("La cita ya tiene un examen registrado.");
        }

        await using var tx = await _db.Database.BeginTransactionAsync(ct);

        var paciente = await _db.Pacientes.FirstOrDefaultAsync(p => p.PersonaId == cliente.PersonaId, ct);
        if (paciente == null)
        {
            paciente = new Paciente { PersonaId = cliente.PersonaId, Estado = Estados.Activo };
            _db.Pacientes.Add(paciente);
            await _db.SaveChangesAsync(ct);
        }

        var historia = await _db.HistoriasClinicas.FirstOrDefaultAsync(h => h.PacienteId == paciente.Id, ct);
        if (historia == null)
        {
            historia = new HistoriaClinica
            {
                PacienteId = paciente.Id,
                NumeroHistoria = await _numerador.SiguienteAsync(_current.SucursalId, TiposDocumento.HistoriaClinica, "HC", ct),
                FechaApertura = DateTime.UtcNow,
                Estado = Estados.Activo
            };
            _db.HistoriasClinicas.Add(historia);
            await _db.SaveChangesAsync(ct);
        }

        var consulta = new Consulta
        {
            HistoriaClinicaId = historia.Id,
            OptometristaId = optometristaId,
            CitaId = r.CitaId,
            FechaConsulta = DateTime.UtcNow,
            MotivoConsulta = r.MotivoConsulta,
            Diagnostico = r.Diagnostico,
            Observaciones = r.Observaciones,
            Recomendaciones = r.Recomendaciones
        };
        _db.Consultas.Add(consulta);
        if (cita != null) cita.Estado = "ATENDIDA";
        await _db.SaveChangesAsync(ct);
        await tx.CommitAsync(ct);

        return await ObtenerAsync(consulta.Id, ct);
    }

    public async Task<ExamenVisualDto> ObtenerAsync(long id, CancellationToken ct = default)
    {
        var c = await Consultas().FirstOrDefaultAsync(x => x.Id == id, ct)
            ?? throw new NotFoundException("Examen visual no encontrado.");
        return await MapAsync(c, ct);
    }

    public async Task<IReadOnlyList<ExamenVisualDto>> ListarPorClienteAsync(long clienteId, CancellationToken ct = default)
    {
        var personaId = await _db.Clientes.Where(c => c.Id == clienteId).Select(c => (long?)c.PersonaId).FirstOrDefaultAsync(ct)
            ?? throw new NotFoundException("Cliente no encontrado.");

        var lista = await Consultas()
            .Where(c => c.HistoriaClinica.Paciente.PersonaId == personaId)
            .OrderByDescending(c => c.FechaConsulta).ToListAsync(ct);

        var res = new List<ExamenVisualDto>();
        foreach (var c in lista) res.Add(await MapAsync(c, ct));
        return res;
    }

    private IQueryable<Consulta> Consultas() => _db.Consultas.AsNoTracking()
        .Include(c => c.Receta)
        .Include(c => c.HistoriaClinica).ThenInclude(h => h.Paciente).ThenInclude(p => p.Persona);

    private async Task<ExamenVisualDto> MapAsync(Consulta c, CancellationToken ct)
    {
        var personaId = c.HistoriaClinica.Paciente.PersonaId;
        var clienteId = await _db.Clientes.Where(x => x.PersonaId == personaId).Select(x => x.Id).FirstOrDefaultAsync(ct);
        var p = c.HistoriaClinica.Paciente.Persona;
        return new ExamenVisualDto
        {
            Id = c.Id, ClienteId = clienteId, ClienteNombre = $"{p.Nombres} {p.Apellidos}".Trim(),
            HistoriaClinicaId = c.HistoriaClinicaId, NumeroHistoria = c.HistoriaClinica.NumeroHistoria,
            OptometristaId = c.OptometristaId, CitaId = c.CitaId, FechaConsulta = c.FechaConsulta,
            MotivoConsulta = c.MotivoConsulta, Diagnostico = c.Diagnostico, Observaciones = c.Observaciones,
            Recomendaciones = c.Recomendaciones,
            Receta = c.Receta == null ? null : RecetaService.Map(c.Receta)
        };
    }
}
