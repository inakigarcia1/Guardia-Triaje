using Guardia.Aplicacion.DTOs;
using Guardia.Dominio.Entidades;

namespace Guardia.Aplicacion.Servicios;

public interface IIngresoService
{
    Task<ResultadoIngreso> RegistrarIngresoAsync(RegistroIngresoRequest request);
    Task<List<Ingreso>> ObtenerColaAtencionAsync();
}

