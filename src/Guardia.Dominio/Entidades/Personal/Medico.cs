using Guardia.Dominio.Entidades.Triajes;

namespace Guardia.Dominio.Entidades.Personal;
public class Medico : Persona
{
    public string Matricula { get; set; }
    public List<Atencion> Atenciones { get; set; } = [];

    public Medico(string cuil, string nombre, string apellido, string email, string matricula) : base(cuil, nombre, apellido, email)
    {
        Matricula = matricula;
    }
    public Medico(string cuil, string nombre, string matricula) : base(cuil, nombre)
    {
        Matricula = matricula;
    }

    public void RegistrarAtencion(Atencion atencion)
    {

    }
}