using Guardia.Dominio.Entidades;

namespace Guardia.Dominio.Repositorios;

public interface IRepositorioAtencion
{
    Task<Atencion> CrearAsync(Atencion atencion);
    Task<List<Atencion>> ObtenerTodosAsync();
}

