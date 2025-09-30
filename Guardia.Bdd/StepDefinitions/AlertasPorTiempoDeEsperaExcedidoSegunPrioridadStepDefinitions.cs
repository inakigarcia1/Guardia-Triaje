using System;
using Reqnroll;

namespace Guardia.Bdd.StepDefinitions
{
    [Binding]
    public class AlertasPorTiempoDeEsperaExcedidoSegunPrioridadStepDefinitions
    {
        [Given("que existen tiempos máximos configurados por prioridad:")]
        public void GivenQueExistenTiemposMaximosConfiguradosPorPrioridad(DataTable dataTable)
        {
            throw new PendingStepException();
        }

        [Given("existe el paciente {string} con prioridad {string} en espera desde hace {int} minutos")]
        public void GivenExisteElPacienteConPrioridadEnEsperaDesdeHaceMinutos(string p0, string urgencia, int p2)
        {
            throw new PendingStepException();
        }

        [Given("estoy viendo la lista de pacientes con triaje asignado")]
        public void GivenEstoyViendoLaListaDePacientesConTriajeAsignado()
        {
            throw new PendingStepException();
        }

        [When("el tiempo de espera de {string} supera el máximo de su prioridad")]
        public void WhenElTiempoDeEsperaDeSuperaElMaximoDeSuPrioridad(string p0)
        {
            throw new PendingStepException();
        }

        [Then("se dispara una alerta en el panel")]
        public void ThenSeDisparaUnaAlertaEnElPanel()
        {
            throw new PendingStepException();
        }

        [Then("la alerta muestra: nombre {string}, prioridad {string} y tiempo excedido {string}")]
        public void ThenLaAlertaMuestraNombrePrioridadYTiempoExcedido(string p0, string urgencia, string p2)
        {
            throw new PendingStepException();
        }

        [Then("{string} pasa a ser la primer paciente en la lista")]
        public void ThenPasaASerLaPrimerPacienteEnLaLista(string p0)
        {
            throw new PendingStepException();
        }

        [Given("existe el paciente {string} con prioridad {string}")]
        public void GivenExisteElPacienteConPrioridad(string p0, string urgencia)
        {
            throw new PendingStepException();
        }

        [When("el paciente {string} supera el tiempo máximo por {int} minutos")]
        public void WhenElPacienteSuperaElTiempoMaximoPorMinutos(string p0, int p1)
        {
            throw new PendingStepException();
        }

        [Then("{string} aparece en la lista con menos prioridad que {string}")]
        public void ThenApareceEnLaListaConMenosPrioridadQue(string p0, string p1)
        {
            throw new PendingStepException();
        }
    }
}
