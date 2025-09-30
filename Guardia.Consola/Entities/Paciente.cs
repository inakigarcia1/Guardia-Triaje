using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guardia.Consola.Entities.Personal;

namespace Guardia.Consola.Entities;
public class Paciente : Persona
{
   public string NumeroDeAfiliado { get; set; }
    public List<Ingreso> Ingresos { get; set; }
}
