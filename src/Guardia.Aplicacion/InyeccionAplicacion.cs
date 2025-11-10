using FluentValidation;
using Guardia.Aplicacion.DTOs;
using Guardia.Aplicacion.Servicios;
using Guardia.Aplicacion.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace Guardia.Aplicacion;
public static class InyeccionAplicacion
{
    public static IServiceCollection AgregarAplicacion(this IServiceCollection services)
    {
        services.AddScoped<IngresoService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IValidator<RegisterDto>, RegisterValidation>();
        services.AddScoped<IValidator<LoginDto>, LoginValidation>();
        return services;
    }
}
