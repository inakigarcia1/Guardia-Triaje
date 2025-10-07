using Guardia.Dominio.Entidades;
using Guardia.Dominio.Repositorios;

namespace Guardia.Infraestructura.Repositorios;

public class RepositorioIngresoEnMemoria : IRepositorioIngreso
{
    private readonly List<Ingreso> _ingresos = new();

    public Task<Ingreso> CrearAsync(Ingreso ingreso)
    {
        _ingresos.Add(ingreso);
        return Task.FromResult(ingreso);
    }

    public Task<List<Ingreso>> ObtenerPendientesAsync()
    {
        var pendientes = _ingresos
            .Where(i => i.Estado == EstadoIngreso.PENDIENTE)
            .OrderBy(i => i.NivelEmergencia.Prioridad)
            .ThenBy(i => i.FechaIngreso)
            .ToList();
        
        return Task.FromResult(pendientes);
    }

    public Task<List<Ingreso>> ObtenerTodosAsync()
    {
        return Task.FromResult(_ingresos.ToList());
    }

    public Task<Ingreso?> ObtenerPorIdAsync(Guid id)
    {
        var ingreso = _ingresos.FirstOrDefault(i => i.Id == id);
        return Task.FromResult(ingreso);
    }
}
