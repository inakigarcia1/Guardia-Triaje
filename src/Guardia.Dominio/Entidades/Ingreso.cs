using Guardia.Dominio.Entidades.Personal;

namespace Guardia.Dominio.Entidades;

public class Ingreso
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime FechaIngreso { get; set; } = DateTime.Now;

    private string _informe = string.Empty;
    public string Informe
    {
        get => _informe;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("El informe es obligatorio.");

            _informe = value;
        }
    }

    private NivelEmergencia _nivelEmergencia;
    public NivelEmergencia NivelEmergencia
    {
        get => _nivelEmergencia;
        set
        {
            if (value is null)
                throw new ArgumentException("El nivel de emergencia es obligatorio.");

            _nivelEmergencia = value;
        }
    }

    public EstadoIngreso Estado { get; set; } = EstadoIngreso.PENDIENTE;

    private float _temperatura;
    public float Temperatura
    {
        get => _temperatura;
        set
        {
            if (value <= 0)
                throw new ArgumentException("La temperatura debe ser mayor que 0.");

            _temperatura = value;
        }
    }

    private float _frecuenciaCardiaca;
    public float FrecuenciaCardiaca
    {
        get => _frecuenciaCardiaca;
        set
        {
            if (value <= 0)
                throw new ArgumentException("La frecuencia cardíaca debe ser mayor que 0.");

            _frecuenciaCardiaca = value;
        }
    }

    private float _frecuenciaRespiratoria;
    public float FrecuenciaRespiratoria
    {
        get => _frecuenciaRespiratoria;
        set
        {
            if (value <= 0)
                throw new ArgumentException("La frecuencia respiratoria debe ser mayor que 0.");

            _frecuenciaRespiratoria = value;
        }
    }

    private TensionArterial _tensionArterial;
    public TensionArterial TensionArterial
    {
        get => _tensionArterial;
        set => _tensionArterial = value ?? throw new ArgumentException("La tensión arterial es obligatoria.");
    }

    public Paciente Paciente { get; set; }
    public Enfermero Enfermero { get; set; }
    public DateTime FechaCreacion { get; set; } = DateTime.Now;

    public Ingreso(
        NivelEmergencia nivelEmergencia,
        float temperatura,
        float frecuenciaCardiaca,
        float frecuenciaRespiratoria,
        TensionArterial tensionArterial,
        Paciente paciente,
        Enfermero enfermero)
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
    private float _sistolica;
    public float Sistolica
    {
        get => _sistolica;
        set
        {
            if (value <= 0)
                throw new ArgumentException("La tensión sistólica debe ser mayor que 0.");

            _sistolica = value;
        }
    }

    private float _diastolica;
    public float Diastolica
    {
        get => _diastolica;
        set
        {
            if (value <= 0)
                throw new ArgumentException("La tensión diastólica debe ser mayor que 0.");

            _diastolica = value;
        }
    }

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
