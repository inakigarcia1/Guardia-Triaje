using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guardia.Consola.Entities.Personal;

namespace Guardia.Consola.Entities.Triajes;
public class Triaje
{
    public Nivel Nivel { get; set; }

    public Enfermero? Enfermero { get; set; }
    public List<CambioNivel> Cambios { get; set; }
    public DateTime CreadoEn {  get; set; }
    public string Motivo {  get; set; }
    public double Temperatura {  get; set; }
    public int FrecuenciaCardiaca {  get; set; }
    public double TensionArterial {  get; set; }
    public int SaturacionOxigeno {  get; set; }
    public string Observaciones {  get; set; }

    public bool EstaVacio()
    {
        return false;
    }
}
