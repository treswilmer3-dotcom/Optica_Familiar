namespace OpticaFamiliar.Application.Common;

/// <summary>Recurso inexistente (HTTP 404).</summary>
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}

/// <summary>Regla de negocio incumplida (HTTP 400/409).</summary>
public class BusinessRuleException : Exception
{
    public BusinessRuleException(string message) : base(message) { }
}

/// <summary>Operación no permitida para el usuario actual (HTTP 403).</summary>
public class ForbiddenException : Exception
{
    public ForbiddenException(string message) : base(message) { }
}
