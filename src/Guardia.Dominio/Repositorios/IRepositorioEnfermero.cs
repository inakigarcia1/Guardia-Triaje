using Guardia.Dominio.Entidades.Personal;

namespace Guardia.Dominio.Repositorios;

public interface IRepositorioEnfermero
{
    Task<Enfermero?> ObtenerPorMatriculaAsync(string matricula);
    Task<Enfermero> CrearAsync(Enfermero enfermero);
    Task<List<Enfermero>> ObtenerTodosAsync();
}
