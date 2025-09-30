using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guardia.Consola.Entities.Personal;

namespace Guardia.Consola.Entities;
public class Atencion
{
    public DateTime Inicio {  get; set; }
    public DateTime Fin {  get; set; }
    public string DiagnosticoPresuntivo { get; set; }
    public string ProcedimientoRealizado { get; set; }
    public string Observaciones { get; set; }
    public Ingreso Ingreso { get; set; }
    public Medico Medico { get; set; }
    public Derivacion? Derivacion { get; set; }

    public TimeSpan ObtenerTiempoDeAtencion() => Fin - Inicio;
}
