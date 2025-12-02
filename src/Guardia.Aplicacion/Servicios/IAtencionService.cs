namespace Guardia.Aplicacion.Servicios;
public interface IAtencionService
{
    Task<IngresoSiguienteDto?> ReclamarSiguiente();
    Task RegistrarAtencion(RegistroAtencionDto dto);
    Task<List<AtencionDto>> ObtenerTodasAsync();
}

public record RegistroAtencionDto
{
    public Guid IngresoId { get; set; }
    public string DiagnosticoPresuntivo { get; set; } = string.Empty;
    public string ProcedimientoRealizado { get; set; } = string.Empty;
    public string Observaciones { get; set; } = string.Empty;
}

public record IngresoSiguienteDto
{
    public Guid Id { get; set; }
    public DateTime FechaIngreso { get; set; }
    public string Informe { get; set; } = string.Empty;
    public string NivelEmergencia { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public int TiempoMaximoMinutos { get; set; }
    public float Temperatura { get; set; }
    public float FrecuenciaCardiaca { get; set; }
    public float FrecuenciaRespiratoria { get; set; }
    public string TensionArterial { get; set; } = string.Empty;
    public string PacienteCuil { get; set; } = string.Empty;
    public string PacienteNombre { get; set; } = string.Empty;
    public string EnfermeroMatricula { get; set; } = string.Empty;
    public string EnfermeroNombre { get; set; } = string.Empty;
}

public record AtencionDto
{
    public DateTime CreadoEn { get; set; }
    public string DiagnosticoPresuntivo { get; set; } = string.Empty;
    public string ProcedimientoRealizado { get; set; } = string.Empty;
    public string Observaciones { get; set; } = string.Empty;
    public Guid IngresoId { get; set; }
    public DateTime FechaIngreso { get; set; }
    public string MedicoMatricula { get; set; } = string.Empty;
    public string MedicoNombre { get; set; } = string.Empty;
    public string PacienteCuil { get; set; } = string.Empty;
    public string PacienteNombre { get; set; } = string.Empty;
}
