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
                new Enfermero(cuil: "20451954270", nombre: "Eugenia", matricula: "ENF-2045"),
                new Enfermero(cuil: "27389104562", nombre: "Carlos", matricula: "ENF-3129"),
                new Enfermero(cuil: "23401237895", nombre: "Maria", matricula: "ENF-1876"),
                new Enfermero(cuil: "30115698247", nombre: "Javier", matricula: "ENF-2754")
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
