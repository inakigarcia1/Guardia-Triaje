using Guardia.Dominio.Repositorios;
using Guardia.Infraestructura.Repositorios;
using Microsoft.Extensions.DependencyInjection;

namespace Guardia.Infraestructura;
public static class InyeccionInfraestructura
{
    public static IServiceCollection AgregarInfraestructura(this IServiceCollection services)
    {
        services.AddSingleton<IRepositorioPaciente, RepositorioPacienteEnMemoria>();
        services.AddSingleton<IRepositorioIngreso, RepositorioIngresoEnMemoria>();
        services.AddSingleton<IRepositorioEnfermero, RepositorioEnfermeroEnMemoria>();
        services.AddSingleton<IRepositorioMedico, RepositorioMedicoEnMemoria>();
        services.AddSingleton<IRepositorioAtencion, RepositorioAtencionEnMemoria>();
        return services;
    }
}
