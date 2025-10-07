using Guardia.Dominio.Entidades;

namespace Guardia.Aplicacion.DTOs;

public class ResultadoIngreso
{
    public bool EsExitoso { get; set; }
    public string? MensajeError { get; set; }
    public string? Mensaje { get; set; }
    public Ingreso? Ingreso { get; set; }
}