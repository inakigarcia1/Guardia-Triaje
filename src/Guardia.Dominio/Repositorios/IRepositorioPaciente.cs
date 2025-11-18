using Guardia.Dominio.Entidades;

namespace Guardia.Dominio.Repositorios;

public interface IRepositorioPaciente
{
    Task<Paciente?> ObtenerPorCuilAsync(string cuil);
    Task<Paciente> CrearAsync(Paciente paciente);
    Task<List<Paciente>> ObtenerTodosAsync();
}
