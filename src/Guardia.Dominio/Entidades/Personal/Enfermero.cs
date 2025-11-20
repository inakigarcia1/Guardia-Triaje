namespace Guardia.Dominio.Entidades.Personal;

public class Enfermero : Persona
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Matricula { get; set; }
    public List<Ingreso> IngresosRegistrados { get; set; } = [];
    public Enfermero(string cuil, string nombre, string apellido, string email, string matricula) : base(cuil, nombre, apellido, email)
    {
        Matricula = matricula;
    }

    public void RegistrarIngreso(Ingreso ingreso)
    {
        IngresosRegistrados.Add(ingreso);
    }
}
