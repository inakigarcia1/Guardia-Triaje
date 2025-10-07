using Guardia.Dominio.Entidades;

namespace Guardia.Dominio.Repositorios;

public interface IRepositorioIngreso
{
    Task<Ingreso> CrearAsync(Ingreso ingreso);
    Task<List<Ingreso>> ObtenerPendientesAsync();
    Task<List<Ingreso>> ObtenerTodosAsync();
    Task<Ingreso?> ObtenerPorIdAsync(Guid id);
}
