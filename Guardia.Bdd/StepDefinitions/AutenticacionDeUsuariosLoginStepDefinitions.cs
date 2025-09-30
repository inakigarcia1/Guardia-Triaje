using System;
using Reqnroll;

namespace Guardia.Bdd.StepDefinitions
{
    [Binding]
    public class AutenticacionDeUsuariosLoginStepDefinitions
    {
        [Given("que existe un usuario {string} con contraseña {string}")]
        public void GivenQueExisteUnUsuarioConContrasena(string p0, string p1)
        {
            throw new PendingStepException();
        }

        [When("ingreso email {string} y contraseña {string}")]
        public void WhenIngresoEmailYContrasena(string p0, string p1)
        {
            throw new PendingStepException();
        }

        [Then("ingreso correctamente a la aplicación")]
        public void ThenIngresoCorrectamenteALaAplicacion()
        {
            throw new PendingStepException();
        }

        [When("hago clic en {string}")]
        public void WhenHagoClicEn(string p0)
        {
            throw new PendingStepException();
        }

        [Then("veo el error {string}")]
        public void ThenVeoElError(string p0)
        {
            throw new PendingStepException();
        }

        [Then("no ingreso a la aplicación")]
        public void ThenNoIngresoALaAplicacion()
        {
            throw new PendingStepException();
        }

        [When("dejo vacío el email y la contraseña")]
        public void WhenDejoVacioElEmailYLaContrasena()
        {
            throw new PendingStepException();
        }

        [Then("el login no se realiza")]
        public void ThenElLoginNoSeRealiza()
        {
            throw new PendingStepException();
        }
    }
}
