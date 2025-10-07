using Guardia.Dominio.Entidades.Personal;

namespace Guardia.Dominio.Entidades;
public class Especialidad
{
    public string Descripcion { get; set; }
    public List<Medico> Medicos { get; set; }
}
