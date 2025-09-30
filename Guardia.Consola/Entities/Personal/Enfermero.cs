using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guardia.Consola.Entities.Triajes;

namespace Guardia.Consola.Entities.Personal;
public class Enfermero : Persona
{
    public List<Triaje> Triajes { get; set; }
    public void CompletarTriaje(Triaje triaje)
    {

    }
}
