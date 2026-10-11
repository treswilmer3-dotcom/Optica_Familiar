using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OpticaFamiliar.Application.Common;

namespace OpticaFamiliar.API.Infrastructure;

/// <summary>Traduce excepciones de negocio a respuestas ProblemDetails; el resto se registra y responde 500 genérico.</summary>
public sealed class ApiExceptionHandler : IExceptionHandler
{
    private readonly ILogger<ApiExceptionHandler> _log;

    public ApiExceptionHandler(ILogger<ApiExceptionHandler> log) => _log = log;

    public async ValueTask<bool> TryHandleAsync(HttpContext ctx, Exception ex, CancellationToken ct)
    {
        var (status, title) = ex switch
        {
            NotFoundException => (StatusCodes.Status404NotFound, "Recurso no encontrado"),
            BusinessRuleException => (StatusCodes.Status400BadRequest, "Regla de negocio incumplida"),
            ForbiddenException => (StatusCodes.Status403Forbidden, "Operación no permitida"),
            Microsoft.EntityFrameworkCore.DbUpdateException => (StatusCodes.Status409Conflict, "Conflicto al guardar los datos"),
            _ => (StatusCodes.Status500InternalServerError, "Error interno del servidor")
        };

        if (status >= 500) _log.LogError(ex, "Error no controlado en {Ruta}", ctx.Request.Path);
        else if (ex is Microsoft.EntityFrameworkCore.DbUpdateException) _log.LogWarning(ex, "Conflicto de datos en {Ruta}", ctx.Request.Path);

        ctx.Response.StatusCode = status;
        await ctx.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = status,
            Title = title,
            Detail = status >= 500 || ex is Microsoft.EntityFrameworkCore.DbUpdateException ? null : ex.Message
        }, ct);
        return true;
    }
}
