using System;
using Reqnroll;

namespace Guardia.Bdd.StepDefinitions
{
    [Binding]
    public class ActivacionDeTriajeParaPacienteAdultoStepDefinitions
    {
        [Given("que existe una lista de pacientes PENDIENTES de triaje")]
        public void GivenQueExisteUnaListaDePacientesPENDIENTESDeTriaje()
        {
            throw new PendingStepException();
        }

        [Given("existe el paciente {string} de {int} años con un ingreso registrado")]
        public void GivenExisteElPacienteDeAnosConUnIngresoRegistrado(string p0, int p1)
        {
            throw new PendingStepException();
        }

        [Given("hay prioridades preestablecidas: {string}, {string}, {string}, {string}, {string}")]
        public void GivenHayPrioridadesPreestablecidas(string p0, string p1, string urgencia, string emergencia, string crítica)
        {
            throw new PendingStepException();
        }

        [Given("yo inicio sesión como usuario con rol {string}")]
        public void GivenYoInicioSesionComoUsuarioConRol(string eNFERMERO)
        {
            throw new PendingStepException();
        }

        [When("selecciono el registro de atención PENDIENTE del paciente {string}")]
        public void WhenSeleccionoElRegistroDeAtencionPENDIENTEDelPaciente(string p0)
        {
            throw new PendingStepException();
        }

        [Then("veo los datos del paciente")]
        public void ThenVeoLosDatosDelPaciente()
        {
            throw new PendingStepException();
        }

        [Given("seleccioné al paciente {string} con triaje PENDIENTE")]
        public void GivenSeleccioneAlPacienteConTriajePENDIENTE(string p0)
        {
            throw new PendingStepException();
        }

        [When("registro el motivo {string}")]
        public void WhenRegistroElMotivo(string fiebre)
        {
            throw new PendingStepException();
        }

        [When("registro los signos vitales: FC {int} ppm, TA {float} mmHg, SO {int}, T {int}")]
        public void WhenRegistroLosSignosVitalesFCPpmTAMmHgSOT(int p0, Decimal p1, int p2, int p3)
        {
            throw new PendingStepException();
        }

        [When("registro observaciones {string}")]
        public void WhenRegistroObservaciones(string p0)
        {
            throw new PendingStepException();
        }

        [When("asigno la prioridad {string}")]
        public void WhenAsignoLaPrioridad(string p0)
        {
            throw new PendingStepException();
        }

        [Then("se crea el triaje")]
        public void ThenSeCreaElTriaje()
        {
            throw new PendingStepException();
        }

        [Then("el paciente deja de figurar en la lista de pendientes")]
        public void ThenElPacienteDejaDeFigurarEnLaListaDePendientes()
        {
            throw new PendingStepException();
        }

        [Given("selecciono el registro de atención PENDIENTE del paciente {string}")]
        public void GivenSeleccionoElRegistroDeAtencionPENDIENTEDelPaciente(string p0)
        {
            throw new PendingStepException();
        }

        [When("intento guardar signos vitales inválidos: FC = {int}")]
        public void WhenIntentoGuardarSignosVitalesInvalidosFC(int p0)
        {
            throw new PendingStepException();
        }

        [Then("se solicita corrección del dato {string}")]
        public void ThenSeSolicitaCorreccionDelDato(string fC)
        {
            throw new PendingStepException();
        }

        [When("corrijo {string} a {string}")]
        public void WhenCorrijoA(string fC, string p1)
        {
            throw new PendingStepException();
        }
    }
}
