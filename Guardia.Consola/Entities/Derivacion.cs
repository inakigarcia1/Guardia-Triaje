using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guardia.Consola.Entities;
public class Derivacion
{
    public string Motivo { get; set; }
    public Guardia Destino { get; set; }
}
