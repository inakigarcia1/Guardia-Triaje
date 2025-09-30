using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guardia.Consola.Entities.Personal;

namespace Guardia.Consola.Entities;
public class Especialidad
{
    public string Descripcion { get; set; }
    public List<Medico> Medicos { get; set; }
}
