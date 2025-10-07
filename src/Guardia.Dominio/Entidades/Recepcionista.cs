using Guardia.Dominio.Entidades.Personal;

namespace Guardia.Dominio.Entidades;
public class Recepcionista : Persona
{
    public List<Ingreso> Ingresos { get; set; }
    public void RegistrarIngreso(Ingreso ingreso)
    {

    }

    public void CompletarRegistroDePaciente(Guid idProvisorio, Paciente paciente)
    {

    }

    private bool TieneCoberturaObraSocial(ulong cuil)
    {
        return false;
    }

    private void TrasladarPacientePediatrico(Paciente paciente)
    {
        
    }

}
