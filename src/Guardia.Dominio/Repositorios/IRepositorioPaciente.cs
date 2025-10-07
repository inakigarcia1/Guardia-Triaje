using Guardia.Dominio.Entidades;

namespace Guardia.Dominio.Repositorios;

public interface IRepositorioPaciente
{
    Task<Paciente?> ObtenerPorDniAsync(ulong dni);
    Task<Paciente> CrearAsync(Paciente paciente);
    Task<List<Paciente>> ObtenerTodosAsync();
}
