using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guardia.Consola.Entities.Personal;
using Guardia.Consola.Entities.Triajes;

namespace Guardia.Consola.Entities.Aplicacion;
public class Repositorio
{
    public Usuario ObtenerUsuario(string username, string password) { return null; }
    public void AgregarIngreso(Ingreso registro) { }
    public Paciente BuscarPaciente(string dni) { return null; }
    public void ActualizarPaciente(Paciente paciente) { }
    public Paciente CrearPacienteProvisorio(Paciente paciente) { return null; }
    public List<Triaje> ObtenerTriajePorPrioridadPendientes() { return null; }
    public List<Triaje> ObtenerTriajePendientesDeAtencion() { return null; }
    public Triaje ObtenerTriaje(string codigoTriaje) { return null; }
    public void ActualizarTriaje(Triaje Triaje) { }
    public void ActualizarEstadoTriaje(string codigoTriaje, string EN_ATENCION) { }
    public List<Triaje> BuscarTriajesPendientes() { return null; }
    public Ingreso BuscarRegistroPendiente(Paciente paciente) { return null; }
    public Guardia BuscarGuardia(string codigoGuardia) { return null; }
    public List<Guardia> ListarGuardias() { return null; }
    public Medico BuscarMedico(Medico medico) { return null; }
    public void GuardarAtencionEnGuardia(Atencion atencion) { }
    public void GuardarDerivacion(Derivacion derivacion) { }
}
