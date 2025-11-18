using Guardia.Dominio.Entidades;
using Guardia.Dominio.Repositorios;

namespace Guardia.Infraestructura.Repositorios;

public class RepositorioAtencionEnMemoria : IRepositorioAtencion
{
    private readonly List<Atencion> _atenciones = new();

    public Task<Atencion> CrearAsync(Atencion atencion)
    {
        _atenciones.Add(atencion);
        return Task.FromResult(atencion);
    }

    public Task<List<Atencion>> ObtenerTodosAsync()
    {
        return Task.FromResult(_atenciones.ToList());
    }
}

