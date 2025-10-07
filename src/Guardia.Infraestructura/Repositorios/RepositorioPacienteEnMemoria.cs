using Guardia.Dominio.Entidades;
using Guardia.Dominio.Repositorios;

namespace Guardia.Infraestructura.Repositorios;

public class RepositorioPacienteEnMemoria : IRepositorioPaciente
{
    private readonly List<Paciente> _pacientes = new();

    public Task<Paciente?> ObtenerPorDniAsync(ulong dni)
    {
        var paciente = _pacientes.FirstOrDefault(p => p.Dni == dni);
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
