using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guardia.Consola.Entities.Triajes;

namespace Guardia.Consola.Entities.Personal;
public class Medico : Persona
{
    public string Matricula { get; set; }
    public Especialidad Especialidad { get; set; }
    public List<Atencion> Atenciones { get; set; }
    public List<CambioNivel> Cambios { get; set; }

    public void RegistrarAtencion(Atencion atencion)
    {

    }

    public void ModificarTriaje(Triaje triaje)
    {
        
    }
}