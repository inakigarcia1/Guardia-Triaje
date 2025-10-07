using Guardia.Dominio.Repositorios;
using Guardia.Infraestructura.Repositorios;
using Microsoft.Extensions.DependencyInjection;

namespace Guardia.Infraestructura;
public static class InyeccionInfraestructura
{
    public static IServiceCollection AgregarInfraestructura(this IServiceCollection services)
    {
        services.AddScoped<IRepositorioPaciente, RepositorioPacienteEnMemoria>();
        services.AddScoped<IRepositorioIngreso, RepositorioIngresoEnMemoria>();
        services.AddScoped<IRepositorioEnfermero, RepositorioEnfermeroEnMemoria>();
        return services;
    }
}
