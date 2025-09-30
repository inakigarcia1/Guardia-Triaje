using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guardia.Consola.Entities.Triajes;
public class Nivel
{
    public PrioridadTriaje Prioridad { get; set; }
    public string Color { get; set; }
    public int TiempoMaximo { get; set; }
}
