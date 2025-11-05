using Guardia.Dominio.Entidades;
using Guardia.Dominio.Excepciones;

namespace Guardia.Tests;

public class AfiliadoTests
{
    [Fact]
    public void CrearAfiliado_ConDatosValidos_DeberiaCrearCorrectamente()
    {
        // Arrange
        var obraSocial = new ObraSocial("OSDE");
        var numeroAfiliado = "12345678";

        // Act
        var afiliado = new Afiliado(obraSocial, numeroAfiliado);

        // Assert
        Assert.NotNull(afiliado.ObraSocial);
        Assert.Equal(obraSocial.Id, afiliado.ObraSocial.Id);
        Assert.Equal(numeroAfiliado, afiliado.NumeroAfiliado);
    }

    [Fact]
    public void CrearAfiliado_ConNumeroAfiliadoVacio_DeberiaLanzarExcepcion()
    {
        // Arrange
        var obraSocial = new ObraSocial("OSDE");
        var numeroAfiliado = "";

        // Act
        var exception = Assert.Throws<DominioException>(() => new Afiliado(obraSocial, numeroAfiliado));

        // Assert
        Assert.Equal("El número de afiliado no puede estar vacío", exception.Message);
    }

    [Fact]
    public void CrearAfiliado_ConNumeroAfiliadoNull_DeberiaLanzarExcepcion()
    {
        // Arrange
        var obraSocial = new ObraSocial("OSDE");
        string numeroAfiliado = null;

        // Act
        var exception = Assert.Throws<DominioException>(() => new Afiliado(obraSocial, numeroAfiliado));

        // Assert
        Assert.Equal("El número de afiliado no puede estar vacío", exception.Message);
    }
}

