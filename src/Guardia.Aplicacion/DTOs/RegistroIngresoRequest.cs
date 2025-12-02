using System.Text.Json.Serialization;
using Guardia.Dominio.Entidades;

namespace Guardia.Aplicacion.DTOs;

public class RegistroIngresoRequest
{
    public string CuilPaciente { get; set; } = string.Empty;
    public string NombrePaciente { get; set; } = string.Empty;
    public string ApellidoPaciente { get; set; } = string.Empty;
    public string EmailPaciente { get; set; } = string.Empty;
    public string CalleDomicilio { get; set; } = string.Empty;
    public int NumeroDomicilio { get; set; }
    public string LocalidadDomicilio { get; set; } = string.Empty;
    public string? NombreObraSocial { get; set; }
    public string? NumeroAfiliado { get; set; }
    public string Informe { get; set; } = string.Empty;
    public PrioridadTriaje NivelEmergencia { get; set; }
    public float Temperatura { get; set; }
    public float FrecuenciaCardiaca { get; set; }
    public float FrecuenciaRespiratoria { get; set; }
    public float TensionSistolica { get; set; }
    public float TensionDiastolica { get; set; }
    [JsonIgnore]
    public string MatriculaEnfermero { get; set; } = string.Empty;
}