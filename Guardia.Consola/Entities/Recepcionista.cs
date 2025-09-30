using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guardia.Consola.Entities.Personal;

namespace Guardia.Consola.Entities;
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
