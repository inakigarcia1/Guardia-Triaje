using Guardia.Dominio.Entidades.Personal;

namespace Guardia.Dominio.Repositorios;

public interface IRepositorioMedico
{
    Task<Medico?> ObtenerPorMatriculaAsync(string matricula);
    Task<Medico> CrearAsync(Medico medico);
    Task<List<Medico>> ObtenerTodosAsync();
}

