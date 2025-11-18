using Guardia.Dominio.Entidades.Personal;
using Guardia.Dominio.Repositorios;

namespace Guardia.Infraestructura.Repositorios;

public class RepositorioEnfermeroEnMemoria : IRepositorioEnfermero
{
    private readonly List<Enfermero> _enfermeros = new();

    public Task<Enfermero?> ObtenerPorMatriculaAsync(string matricula)
    {
        var enfermero = _enfermeros.FirstOrDefault(e => e.Matricula == matricula);
        return Task.FromResult(enfermero);
    }

    public Task<Enfermero> CrearAsync(Enfermero enfermero)
    {
        _enfermeros.Add(enfermero);
        return Task.FromResult(enfermero);
    }

    public Task<List<Enfermero>> ObtenerTodosAsync()
    {
        return Task.FromResult(_enfermeros.ToList());
    }
}
