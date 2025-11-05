using Guardia.Dominio.Entidades.Personal;
using Guardia.Dominio.Entidades.Triajes;

namespace Guardia.Dominio.Entidades;

public class Ingreso
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime FechaIngreso { get; set; } = DateTime.Now;
    public string Informe { get; set; } = string.Empty;
    public NivelEmergencia NivelEmergencia { get; set; }
    public EstadoIngreso Estado { get; set; } = EstadoIngreso.PENDIENTE;
    public float Temperatura { get; set; }
    public float FrecuenciaCardiaca { get; set; }
    public float FrecuenciaRespiratoria { get; set; }
    public TensionArterial TensionArterial { get; set; }
    public Paciente Paciente { get; set; }
    public Enfermero Enfermero { get; set; }
    public DateTime FechaCreacion { get; set; } = DateTime.Now;
    public Ingreso(NivelEmergencia nivelEmergencia, float temperatura, float frecuenciaCardiaca, float frecuenciaRespiratoria, TensionArterial tensionArterial, Paciente paciente, Enfermero enfermero)
    {
        NivelEmergencia = nivelEmergencia;
        Temperatura = temperatura;
        FrecuenciaCardiaca = frecuenciaCardiaca;
        FrecuenciaRespiratoria = frecuenciaRespiratoria;
        TensionArterial = tensionArterial;
        Paciente = paciente;
        Enfermero = enfermero;
    }
}

public class NivelEmergencia
{
    public PrioridadTriaje Prioridad { get; set; }
    public string Color { get; set; } = string.Empty;
    public int TiempoMaximoMinutos { get; set; }
    public static NivelEmergencia CrearNivelEmergencia(PrioridadTriaje prioridad)
    {
        return prioridad switch
        {
            PrioridadTriaje.Critico => new NivelEmergencia { Prioridad = prioridad, Color = "Rojo", TiempoMaximoMinutos = 5 },
            PrioridadTriaje.Emergencia => new NivelEmergencia { Prioridad = prioridad, Color = "Naranja", TiempoMaximoMinutos = 30 },
            PrioridadTriaje.Urgencia => new NivelEmergencia { Prioridad = prioridad, Color = "Amarillo", TiempoMaximoMinutos = 60 },
            PrioridadTriaje.UrgenciaMenor => new NivelEmergencia { Prioridad = prioridad, Color = "Verde", TiempoMaximoMinutos = 120 },
            PrioridadTriaje.SinUrgencia => new NivelEmergencia { Prioridad = prioridad, Color = "Azul", TiempoMaximoMinutos = 240 },
            _ => throw new ArgumentException("Nivel de emergencia no válido")
        };
    }
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
