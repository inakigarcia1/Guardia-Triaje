using Guardia.Dominio.Entidades;
using Guardia.Dominio.Excepciones;

namespace Guardia.Tests;

public class DomicilioTests
{
    [Fact]
    public void CrearDomicilio_ConDatosValidos_DeberiaCrearCorrectamente()
    {
        // Arrange
        var calle = "San Martin";
        var numero = 123;
        var localidad = "San Miguel de Tucuman";

        // Act
        var domicilio = new Domicilio(calle, numero, localidad);

        // Assert
        Assert.Equal(calle, domicilio.Calle);
        Assert.Equal(numero, domicilio.Numero);
        Assert.Equal(localidad, domicilio.Localidad);
        Assert.Equal("Tucumán", domicilio.Provincia);
        Assert.Equal("Argentina", domicilio.Pais);
    }

    [Fact]
    public void CrearDomicilio_ConCalleVacia_DeberiaLanzarExcepcion()
    {
        // Arrange
        var calle = "";
        var numero = 123;
        var localidad = "San Miguel de Tucuman";

        // Act
        var exception = Assert.Throws<DominioException>(() => new Domicilio(calle, numero, localidad));

        // Assert
        Assert.Equal("La calle no puede estar vacía", exception.Message);
    }

    [Fact]
    public void CrearDomicilio_ConCalleNull_DeberiaLanzarExcepcion()
    {
        // Arrange
        string calle = null;
        var numero = 123;
        var localidad = "San Miguel de Tucuman";

        // Act
        var exception = Assert.Throws<DominioException>(() => new Domicilio(calle, numero, localidad));

        // Assert
        Assert.Equal("La calle no puede estar vacía", exception.Message);
    }

    [Fact]
    public void CrearDomicilio_ConNumeroCero_DeberiaLanzarExcepcion()
    {
        // Arrange
        var calle = "San Martin";
        var numero = 0;
        var localidad = "San Miguel de Tucuman";

        // Act
        var exception = Assert.Throws<DominioException>(() => new Domicilio(calle, numero, localidad));

        // Assert
        Assert.Equal("El número de calle debe ser mayor a cero", exception.Message);
    }

    [Fact]
    public void CrearDomicilio_ConNumeroNegativo_DeberiaLanzarExcepcion()
    {
        // Arrange
        var calle = "San Martin";
        var numero = -1;
        var localidad = "San Miguel de Tucuman";

        // Act
        var exception = Assert.Throws<DominioException>(() => new Domicilio(calle, numero, localidad));

        // Assert
        Assert.Equal("El número de calle debe ser mayor a cero", exception.Message);
    }

    [Fact]
    public void CrearDomicilio_ConLocalidadVacia_DeberiaLanzarExcepcion()
    {
        // Arrange
        var calle = "San Martin";
        var numero = 123;
        var localidad = "";

        // Act
        var exception = Assert.Throws<DominioException>(() => new Domicilio(calle, numero, localidad));

        // Assert
        Assert.Equal("La localidad no puede estar vacía", exception.Message);
    }

    [Fact]
    public void CrearDomicilio_ConLocalidadNull_DeberiaLanzarExcepcion()
    {
        // Arrange
        var calle = "San Martin";
        var numero = 123;
        string localidad = null;

        // Act
        var exception = Assert.Throws<DominioException>(() => new Domicilio(calle, numero, localidad));

        // Assert
        Assert.Equal("La localidad no puede estar vacía", exception.Message);
    }

}

