using System;
using Reqnroll;

namespace Guardia.Bdd.StepDefinitions
{
    [Binding]
    public class AtencionEnGuardiaPorPrioridadStepDefinitions
    {
        [Given("estoy autenticado como médico de guardia")]
        public void GivenEstoyAutenticadoComoMedicoDeGuardia()
        {
            throw new PendingStepException();
        }

        [Given("hay pacientes con triaje en espera con prioridades: {string}, {string}, {string}, {string}, {string}")]
        public void GivenHayPacientesConTriajeEnEsperaConPrioridades(string crítica, string emergencia, string urgencia, string p3, string p4)
        {
            throw new PendingStepException();
        }

        [Given("existen los pacientes {string} \\(Crítica), {string} \\(Urgencia), {string} \\(Emergencia)")]
        public void GivenExistenLosPacientesCriticaUrgenciaEmergencia(string ana, string bruno, string carla)
        {
            throw new PendingStepException();
        }

        [When("consulto la lista de pacientes con triaje")]
        public void WhenConsultoLaListaDePacientesConTriaje()
        {
            throw new PendingStepException();
        }

        [Then("la lista se presenta en orden: {string}, {string}, {string}")]
        public void ThenLaListaSePresentaEnOrden(string ana, string carla, string bruno)
        {
            throw new PendingStepException();
        }

        [Given("{string} \\(Emergencia, espera {int} min) y {string} \\(Emergencia, espera {int} min)")]
        public void GivenEmergenciaEsperaMinYEmergenciaEsperaMin(string dora, int p1, string ezequiel, int p3)
        {
            throw new PendingStepException();
        }

        [Then("la lista se presenta en orden: {string}, {string}")]
        public void ThenLaListaSePresentaEnOrden(string ana, string ezequiel)
        {
            throw new PendingStepException();
        }

        [Given("el paciente {string} \\(Urgencia) está en espera")]
        public void GivenElPacienteUrgenciaEstaEnEspera(string mateo)
        {
            throw new PendingStepException();
        }

        [When("inicio la atención de {string}")]
        public void WhenInicioLaAtencionDe(string mateo)
        {
            throw new PendingStepException();
        }

        [When("registro diagnóstico presuntivo {string}")]
        public void WhenRegistroDiagnosticoPresuntivo(string estreñimiento)
        {
            throw new PendingStepException();
        }

        [When("registro procedimientos realizados:")]
        public void WhenRegistroProcedimientosRealizados(string multilineText)
        {
            throw new PendingStepException();
        }

        [Then("queda registrada la Atención en Guardia de {string} con todos los datos ingresados")]
        public void ThenQuedaRegistradaLaAtencionEnGuardiaDeConTodosLosDatosIngresados(string mateo)
        {
            throw new PendingStepException();
        }

        [Then("{string} deja de figurar en la lista de espera")]
        public void ThenDejaDeFigurarEnLaListaDeEspera(string mateo)
        {
            throw new PendingStepException();
        }
    }
}
