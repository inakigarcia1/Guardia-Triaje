using Guardia.Dominio.Entidades;
using Guardia.Dominio.Repositorios;

namespace Guardia.Infraestructura.Repositorios;

public class RepositorioPacienteEnMemoria : IRepositorioPaciente
{
    private readonly List<Paciente> _pacientes = [];

    public Task<Paciente?> ObtenerPorCuilAsync(string cuil)
    {
        var paciente = _pacientes.FirstOrDefault(p => p.Cuil.Equals(cuil, StringComparison.CurrentCultureIgnoreCase));
        return Task.FromResult(paciente);
    }

    public Task<Paciente> CrearAsync(Paciente paciente)
    {
        _pacientes.Add(paciente);
        return Task.FromResult(paciente);
    }

    public Task<List<Paciente>> ObtenerTodosAsync()
    {
        return Task.FromResult(_pacientes.ToList());
    }
}
