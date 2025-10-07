using Guardia.Dominio.Entidades.Personal;
using Guardia.Dominio.Repositorios;

namespace Guardia.Infraestructura.Repositorios;

public class RepositorioEnfermeroEnMemoria : IRepositorioEnfermero
{
    private readonly List<Enfermero> _enfermeros = new();

    public RepositorioEnfermeroEnMemoria()
    {
        _enfermeros.AddRange(
            [

            new Enfermero
            {
                Matricula = "ENF-2045"
            },
            new Enfermero
            {
                Matricula = "ENF-3129"
            },
            new Enfermero
            {
                Matricula = "ENF-1876"
            },
            new Enfermero
            {
                Matricula = "ENF-2754"
            },

            ]
            );
    }

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
