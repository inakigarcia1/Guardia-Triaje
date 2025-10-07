using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guardia.Aplicacion.Servicios;
using Guardia.Dominio.Repositorios;
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
