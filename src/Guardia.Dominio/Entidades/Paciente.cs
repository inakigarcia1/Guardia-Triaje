using Guardia.Dominio.Entidades.Personal;

namespace Guardia.Dominio.Entidades;

public class Paciente : Persona
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Domicilio Domicilio { get; set; }
    public Afiliado? Afiliado { get; set; }
    public Paciente(string cuil, string nombre, string apellido, string email, Domicilio domicilio, Afiliado? afiliado) : base(cuil, nombre, apellido, email)
    {
        Domicilio = domicilio;
        Afiliado = afiliado;
    }
}
