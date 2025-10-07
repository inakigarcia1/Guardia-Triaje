using Guardia.Dominio.Entidades.Personal;
using Guardia.Dominio.Entidades.Triajes;

namespace Guardia.Dominio.Entidades;

public class Ingreso
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime FechaIngreso { get; set; } = DateTime.Now;
    public string Informe { get; set; } = string.Empty;
    public NivelEmergencia NivelEmergencia { get; set; } = new();
    public EstadoIngreso Estado { get; set; } = EstadoIngreso.PENDIENTE;
    public float? Temperatura { get; set; }
    public float FrecuenciaCardiaca { get; set; }
    public float FrecuenciaRespiratoria { get; set; }
    public TensionArterial TensionArterial { get; set; } = new();
    public Paciente Paciente { get; set; } = new();
    public Enfermero Enfermero { get; set; } = new();
    public DateTime FechaCreacion { get; set; } = DateTime.Now;
}

public class NivelEmergencia
{
    public PrioridadTriaje Prioridad { get; set; }
    public string Color { get; set; } = string.Empty;
    public int TiempoMaximoMinutos { get; set; }
}

public class TensionArterial
{
    public float Sistolica { get; set; }
    public float Diastolica { get; set; }
    
    public override string ToString()
    {
        return $"{Sistolica}/{Diastolica}";
    }
}

public enum EstadoIngreso
{
    PENDIENTE,
    EN_PROCESO,
    FINALIZADO
}
