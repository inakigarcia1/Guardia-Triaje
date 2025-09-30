using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guardia.Consola.Entities.Aplicacion.RegistrarIngreso;
public class ControladorRegistrarIngreso
{
    private GestorRegistrarIngreso _gestor;

    public void RegistrarIngreso(string codigo, string fecha, string motivoConsulta, bool esCritico, string dni) { }
    public void ConfirmarRegistro(string dni) { }
    public void IngresarPacienteIncapacitado(string tipoPaciente, string observacion) { }
    public void IngresarPaciente(string dni, string motivoConsulta, string obraSocial) { }
}
