namespace Guardia.Aplicacion.Servicios;
public interface IAtencionService
{
    Task<IngresoSiguienteDto> ReclamarSiguiente();
    Task RegistrarAtencion(RegistroAtencionDto dto);
}

public record RegistroAtencionDto();

public record IngresoSiguienteDto();
