using Guardia.Dominio.Entidades.Personal;

namespace Guardia.Dominio.Entidades;
public class Atencion
{
    public DateTime Inicio {  get; set; }
    public DateTime Fin {  get; set; }
    public string DiagnosticoPresuntivo { get; set; }
    public string ProcedimientoRealizado { get; set; }
    public string Observaciones { get; set; }
    public Ingreso Ingreso { get; set; }
    public Medico Medico { get; set; }

    public TimeSpan ObtenerTiempoDeAtencion() => Fin - Inicio;
}
