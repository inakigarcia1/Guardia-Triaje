using Guardia.Dominio.Entidades.Triajes;

namespace Guardia.Aplicacion.DTOs;

public class RegistroIngresoRequest
{
    public string CuilPaciente { get; set; } = string.Empty;
    public string NombrePaciente { get; set; } = string.Empty;
    public string Informe { get; set; } = string.Empty;
    public PrioridadTriaje NivelEmergencia { get; set; }
    public float Temperatura { get; set; }
    public float FrecuenciaCardiaca { get; set; }
    public float FrecuenciaRespiratoria { get; set; }
    public float TensionSistolica { get; set; }
    public float TensionDiastolica { get; set; }
    public string MatriculaEnfermero { get; set; } = string.Empty;
}