using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guardia.Consola.Entities.Triajes;

namespace Guardia.Consola.Entities.Aplicacion.Triajes;
public class GestorTriaje
{
    private Repositorio _repositorio;

    public List<Triaje> InicioEvaluacionTriage() { return null; }
    public void RegistrarConsulta(string motivoConsulta, string temperatura, string fCardiaca,
        string fRespiratorio, string tension, string sOxigeno, string observaciones)
    { }
    public void SeleccionarPrioridad(string codigoTriage, string codigoPrioridad) { }

    public List<Triaje> CambiarPrioridad() { return null; }
    public Triaje SeleccionarTriage(string codigoTriage) { return null; }
}
