using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpticaFamiliar.Application.Interfaces;
using OpticaFamiliar.Infrastructure.Data;
using OpticaFamiliar.Infrastructure.Security;
using OpticaFamiliar.Infrastructure.Seed;
using OpticaFamiliar.Infrastructure.Services;

namespace OpticaFamiliar.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        var cs = config.GetConnectionString("DefaultConnection");
        if (string.IsNullOrWhiteSpace(cs))
            throw new InvalidOperationException(
                "Falta ConnectionStrings:DefaultConnection. Configúrela con 'dotnet user-secrets' o variables de entorno.");

        services.AddDbContext<AppDbContext>(o => o.UseNpgsql(cs));

        services.Configure<JwtOptions>(config.GetSection(JwtOptions.Seccion));
        services.Configure<SeedOptions>(config.GetSection(SeedOptions.Seccion));

        services.AddSingleton<IPasswordHasher, BCryptPasswordHasher>();
        services.AddSingleton<IJwtTokenService, JwtTokenService>();

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IEmpresaService, EmpresaService>();
        services.AddScoped<ISucursalService, SucursalService>();
        services.AddScoped<IRolService, RolService>();
        services.AddScoped<IUsuarioService, UsuarioService>();
        services.AddScoped<IClienteService, ClienteService>();
        services.AddScoped<IExamenVisualService, ExamenVisualService>();
        services.AddScoped<IRecetaService, RecetaService>();
        services.AddScoped<IOrdenTrabajoService, OrdenTrabajoService>();
        services.AddScoped<IVentaService, VentaService>();
        services.AddScoped<IProductoService, ProductoService>();
        services.AddScoped<DbSeeder>();

        return services;
    }
}
