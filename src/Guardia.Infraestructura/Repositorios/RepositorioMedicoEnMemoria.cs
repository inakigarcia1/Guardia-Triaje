using Guardia.Dominio.Entidades.Personal;
using Guardia.Dominio.Repositorios;

namespace Guardia.Infraestructura.Repositorios;

public class RepositorioMedicoEnMemoria : IRepositorioMedico
{
    private readonly List<Medico> _medicos = new();

    public Task<Medico?> ObtenerPorMatriculaAsync(string matricula)
    {
        var medico = _medicos.FirstOrDefault(m => m.Matricula == matricula);
        return Task.FromResult(medico);
    }

    public Task<Medico> CrearAsync(Medico medico)
    {
        _medicos.Add(medico);
        return Task.FromResult(medico);
    }

    public Task<List<Medico>> ObtenerTodosAsync()
    {
        return Task.FromResult(_medicos.ToList());
    }
}

