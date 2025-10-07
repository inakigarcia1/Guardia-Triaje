using Guardia.Dominio.Entidades.Triajes;

namespace Guardia.Dominio.Entidades.Personal;
public class Medico : Persona
{
    public string Matricula { get; set; }
    public List<Atencion> Atenciones { get; set; }

    public void RegistrarAtencion(Atencion atencion)
    {

    }

    public void ModificarTriaje(Triaje triaje)
    {
        
    }
}