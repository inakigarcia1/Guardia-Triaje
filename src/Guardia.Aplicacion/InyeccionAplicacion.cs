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
        services.AddScoped<IIngresoService, IngresoService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IAtencionService, AtencionService>();
        services.AddScoped<IValidator<RegistroUsuarioDto>, RegistroUsuarioValidator>();
        services.AddScoped<IValidator<LoginDto>, LoginValidation>();
        services.AddScoped<IValidator<RegistroIngresoRequest>, RegistroIngresoValidator>();
        return services;
    }
}
