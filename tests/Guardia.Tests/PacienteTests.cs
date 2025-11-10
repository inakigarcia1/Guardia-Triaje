using Guardia.Dominio.Entidades;
using Guardia.Dominio.Excepciones;

namespace Guardia.Tests;
public class PacienteTests
{
    [Fact]
    public void CrearPaciente_ConCuilYNombre_DeberiaCrearCorrectamente()
    {
        // Arrange
        var cuil = "20451954270";
        var nombre = "Juan Perez";

        // Act
        var paciente = new Paciente(cuil, nombre);

        // Assert
        Assert.Equal(cuil, paciente.Cuil);
        Assert.Equal(nombre, paciente.Nombre);
    }

    [Fact]
    public void CrearPaciente_ConCuilVacio_DeberiaLanzarExcepcion()
    {
        // Arrange
        var cuil = "";
        var nombre = "Juan Perez";

        // Act
        var exception = Assert.Throws<DominioException>(() => new Paciente(cuil, nombre));

        // Assert
        Assert.Equal("El cuil no puede estar vacío", exception.Message);
    }

    [Fact]
    public void CrearPaciente_ConCuilNull_DeberiaLanzarExcepcion()
    {
        // Arrange
        string cuil = null;
        var nombre = "Juan Perez";

        // Act
        var exception = Assert.Throws<DominioException>(() => new Paciente(cuil, nombre));

        // Assert
        Assert.Equal("El cuil no puede estar vacío", exception.Message);
    }

    [Fact]
    public void CrearPaciente_ConCuilConLetras_DeberiaLanzarExcepcion()
    {
        // Arrange
        var cuil = "2045195427A";
        var nombre = "Juan Perez";

        // Act
        var exception = Assert.Throws<DominioException>(() => new Paciente(cuil, nombre));

        // Assert
        Assert.Equal("El cuil solo puede contener numeros", exception.Message);
    }

    [Fact]
    public void CrearPaciente_ConCuilConSimbolos_DeberiaLanzarExcepcion()
    {
        // Arrange
        var cuil = "2045195427@";
        var nombre = "Juan Perez";

        // Act
        var exception = Assert.Throws<DominioException>(() => new Paciente(cuil, nombre));

        // Assert
        Assert.Equal("El cuil solo puede contener numeros", exception.Message);
    }

    [Fact]
    public void CrearPaciente_ConCuilMenorA10Digitos_DeberiaLanzarExcepcion()
    {
        // Arrange
        var cuil = "123456789";
        var nombre = "Juan Perez";

        // Act
        var exception = Assert.Throws<DominioException>(() => new Paciente(cuil, nombre));

        // Assert
        Assert.Equal("El cuil no puede tener menos de 10 dígitos", exception.Message);
    }

    [Fact]
    public void CrearPaciente_ConCuilMayorA11Digitos_DeberiaLanzarExcepcion()
    {
        // Arrange
        var cuil = "123456789012";
        var nombre = "Juan Perez";

        // Act
        var exception = Assert.Throws<DominioException>(() => new Paciente(cuil, nombre));

        // Assert
        Assert.Equal("El cuil no puede tener más de 11 digitos", exception.Message);
    }

    [Fact]
    public void CrearPaciente_ConNombreVacio_DeberiaLanzarExcepcion()
    {
        // Arrange
        var cuil = "20451954270";
        var nombre = "";

        // Act
        var exception = Assert.Throws<DominioException>(() => new Paciente(cuil, nombre));

        // Assert
        Assert.Equal("El nombre de la persona no puede estar vacío", exception.Message);
    }

    [Fact]
    public void CrearPaciente_ConNombreNull_DeberiaLanzarExcepcion()
    {
        // Arrange
        var cuil = "20451954270";
        string nombre = null;

        // Act
        var exception = Assert.Throws<DominioException>(() => new Paciente(cuil, nombre));

        // Assert
        Assert.Equal("El nombre de la persona no puede estar vacío", exception.Message);
    }

    [Fact]
    public void CrearPaciente_ConNombreConSimbolos_DeberiaLanzarExcepcion()
    {
        // Arrange
        var cuil = "20451954270";
        var nombre = "Juan@Perez";

        // Act
        var exception = Assert.Throws<DominioException>(() => new Paciente(cuil, nombre));

        // Assert
        Assert.Equal("El nombre no puede contener caracteres especiales", exception.Message);
    }

    [Fact]
    public void CrearPaciente_ConNombreConPuntuacion_DeberiaLanzarExcepcion()
    {
        // Arrange
        var cuil = "20451954270";
        var nombre = "Juan.Perez";

        // Act
        var exception = Assert.Throws<DominioException>(() => new Paciente(cuil, nombre));

        // Assert
        Assert.Equal("El nombre no puede contener caracteres especiales", exception.Message);
    }

    [Fact]
    public void CrearPaciente_ConTodosLosDatosYObraSocial_DeberiaCrearCorrectamente()
    {
        // Arrange
        var cuil = "20451954270";
        var nombre = "Juan";
        var apellido = "Perez";
        var email = "juan.perez@email.com";
        var domicilio = new Domicilio("San Martin", 123, "San Miguel de Tucuman");
        var obraSocial = new ObraSocial("OSDE");
        var afiliado = new Afiliado(obraSocial, "12345678");

        // Act
        var paciente = new Paciente(cuil, nombre, apellido, email, domicilio, afiliado);

        // Assert
        Assert.Equal(cuil, paciente.Cuil);
        Assert.Equal(nombre, paciente.Nombre);
        Assert.Equal(apellido, paciente.Apellido);
        Assert.Equal(email, paciente.Email);
        Assert.NotNull(paciente.Domicilio);
        Assert.NotNull(paciente.Afiliado);
        Assert.Equal(obraSocial.Id, paciente.Afiliado.ObraSocial.Id);
    }

    [Fact]
    public void CrearPaciente_ConTodosLosDatosSinObraSocial_DeberiaCrearCorrectamente()
    {
        // Arrange
        var cuil = "20451954270";
        var nombre = "Juan";
        var apellido = "Perez";
        var email = "juan.perez@email.com";
        var domicilio = new Domicilio("San Martin", 123, "San Miguel de Tucuman");

        // Act
        var paciente = new Paciente(cuil, nombre, apellido, email, domicilio, null);

        // Assert
        Assert.Equal(cuil, paciente.Cuil);
        Assert.Equal(nombre, paciente.Nombre);
        Assert.Equal(apellido, paciente.Apellido);
        Assert.Equal(email, paciente.Email);
        Assert.NotNull(paciente.Domicilio);
        Assert.Null(paciente.Afiliado);
    }

    [Fact]
    public void CrearPaciente_ConTodosLosDatosConApellidoVacio_DeberiaLanzarExcepcion()
    {
        // Arrange
        var cuil = "20451954270";
        var nombre = "Juan";
        var apellido = "";
        var email = "juan.perez@email.com";
        var domicilio = new Domicilio("San Martin", 123, "San Miguel de Tucuman");

        // Act
        var exception = Assert.Throws<DominioException>(() => new Paciente(cuil, nombre, apellido, email, domicilio, null));

        // Assert
        Assert.Equal("El apellido de la persona no puede estar vacío", exception.Message);
    }

    [Fact]
    public void CrearPaciente_ConTodosLosDatosConApellidoNull_DeberiaLanzarExcepcion()
    {
        // Arrange
        var cuil = "20451954270";
        var nombre = "Juan";
        string apellido = null;
        var email = "juan.perez@email.com";
        var domicilio = new Domicilio("San Martin", 123, "San Miguel de Tucuman");

        // Act
        var exception = Assert.Throws<DominioException>(() => new Paciente(cuil, nombre, apellido, email, domicilio, null));

        // Assert
        Assert.Equal("El apellido de la persona no puede estar vacío", exception.Message);
    }

    [Fact]
    public void CrearPaciente_ConTodosLosDatosConApellidoConSimbolos_DeberiaLanzarExcepcion()
    {
        // Arrange
        var cuil = "20451954270";
        var nombre = "Juan";
        var apellido = "Perez@";
        var email = "juan.perez@email.com";
        var domicilio = new Domicilio("San Martin", 123, "San Miguel de Tucuman");

        // Act
        var exception = Assert.Throws<DominioException>(() => new Paciente(cuil, nombre, apellido, email, domicilio, null));

        // Assert
        Assert.Equal("El apellido no puede contener caracteres especiales", exception.Message);
    }

    [Fact]
    public void CrearPaciente_ConTodosLosDatosConEmailVacio_DeberiaLanzarExcepcion()
    {
        // Arrange
        var cuil = "20451954270";
        var nombre = "Juan";
        var apellido = "Perez";
        var email = "";
        var domicilio = new Domicilio("San Martin", 123, "San Miguel de Tucuman");

        // Act
        var exception = Assert.Throws<DominioException>(() => new Paciente(cuil, nombre, apellido, email, domicilio, null));

        // Assert
        Assert.Equal("El email de la persona no puede estar vacío", exception.Message);
    }

    [Fact]
    public void CrearPaciente_ConTodosLosDatosConEmailNull_DeberiaLanzarExcepcion()
    {
        // Arrange
        var cuil = "20451954270";
        var nombre = "Juan";
        var apellido = "Perez";
        string email = null;
        var domicilio = new Domicilio("San Martin", 123, "San Miguel de Tucuman");

        // Act
        var exception = Assert.Throws<DominioException>(() => new Paciente(cuil, nombre, apellido, email, domicilio, null));

        // Assert
        Assert.Equal("El email de la persona no puede estar vacío", exception.Message);
    }

    [Fact]
    public void CrearPaciente_ConTodosLosDatosConEmailInvalido_DeberiaLanzarExcepcion()
    {
        // Arrange
        var cuil = "20451954270";
        var nombre = "Juan";
        var apellido = "Perez";
        var email = "emailinvalido";
        var domicilio = new Domicilio("San Martin", 123, "San Miguel de Tucuman");

        // Act
        var exception = Assert.Throws<DominioException>(() => new Paciente(cuil, nombre, apellido, email, domicilio, null));

        // Assert
        Assert.Equal("El email no cumple con el formato requerido", exception.Message);
    }

}
