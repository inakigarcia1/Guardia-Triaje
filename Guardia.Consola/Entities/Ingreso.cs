using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guardia.Consola.Entities.Triajes;

namespace Guardia.Consola.Entities;
public class Ingreso
{
    public DateTime CreadoEn {  get; set; }
    public string Observaciones { get; set; }
    public Guid? IdProvisorio { get; set; }
    public Paciente? Paciente { get; set; }
    public Triaje Triaje { get; set; } = new();
    public Recepcionista Recepcionista { get; set; }
    public Atencion? Atencion { get; set; }

    public EstadoPaciente ObtenerEstadoPaciente()
    {
        return default;
    }
}
