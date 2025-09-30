using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guardia.Consola.Entities.Personal;

namespace Guardia.Consola.Entities.Triajes;
public class CambioNivel
{
    public DateTime CreadoEn {  get; set; }
    public string Motivo { get; set; }
    public Triaje Triaje { get; set; }
    public Medico Medico { get; set; }
}
