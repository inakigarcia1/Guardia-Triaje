using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guardia.Consola.Entities.Triajes;

namespace Guardia.Consola.Entities.Aplicacion.Triajes;
public class ControladorActivarTriaje
{
    private GestorTriaje _gestor;

    public List<Triaje> InicioEvaluacionTriage() { return null; }
    public Triaje SeleccionarTriage(string codigoTriage) { return null; }
    public void RegistrarConsulta(string motivoConsulta, string temperatura, string fCardiaca,
        string fRespiratorio, string tension, string sOxigeno, string observaciones)
    { }
    public void SeleccionarPrioridad(string codigoTriage, string codigoPrioridad) { }
}
