using Guardia.Aplicacion.Servicios;
using Microsoft.Extensions.DependencyInjection;

namespace Guardia.Aplicacion;
public static class InyeccionAplicacion
{
    public static IServiceCollection AgregarAplicacion(this IServiceCollection services)
    {
        services.AddScoped<IngresoService>();
        return services;
    }
}
