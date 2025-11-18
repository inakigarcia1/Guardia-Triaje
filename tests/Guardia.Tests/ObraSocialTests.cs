using Guardia.Dominio.Entidades;
using Guardia.Dominio.Excepciones;

namespace Guardia.Tests;

public class ObraSocialTests
{
    [Fact]
    public void CrearObraSocial_ConNombreValido_DeberiaCrearCorrectamente()
    {
        // Arrange
        var nombre = "OSDE";

        // Act
        var obraSocial = new ObraSocial(nombre);

        // Assert
        Assert.Equal(nombre, obraSocial.Nombre);
        Assert.NotEqual(Guid.Empty, obraSocial.Id);
    }

    [Fact]
    public void CrearObraSocial_ConNombreVacio_DeberiaLanzarExcepcion()
    {
        // Arrange
        var nombre = "";

        // Act
        var exception = Assert.Throws<DominioException>(() => new ObraSocial(nombre));

        // Assert
        Assert.Equal("El nombre de la obra social no puede estar vacío", exception.Message);
    }

    [Fact]
    public void CrearObraSocial_ConNombreNull_DeberiaLanzarExcepcion()
    {
        // Arrange
        string nombre = null;

        // Act
        var exception = Assert.Throws<DominioException>(() => new ObraSocial(nombre));

        // Assert
        Assert.Equal("El nombre de la obra social no puede estar vacío", exception.Message);
    }

    [Fact]
    public void CrearObraSocial_ConNombreConSimbolos_DeberiaLanzarExcepcion()
    {
        // Arrange
        var nombre = "OSDE@";

        // Act
        var exception = Assert.Throws<DominioException>(() => new ObraSocial(nombre));

        // Assert
        Assert.Equal("El nombre de la obra social no puede contener caracteres especiales", exception.Message);
    }

    [Fact]
    public void CrearObraSocial_ConNombreConPuntuacion_DeberiaLanzarExcepcion()
    {
        // Arrange
        var nombre = "OSDE.";

        // Act
        var exception = Assert.Throws<DominioException>(() => new ObraSocial(nombre));

        // Assert
        Assert.Equal("El nombre de la obra social no puede contener caracteres especiales", exception.Message);
    }
}

