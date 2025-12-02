using Guardia.Aplicacion.DTOs;
using Guardia.Aplicacion.Servicios;
using Guardia.Dominio.Entidades;
using Guardia.Dominio.Entidades.Personal;
using Guardia.Dominio.Repositorios;
using Guardia.Infraestructura.Repositorios;

namespace Guardia.Tests;

public class IngresoServiceTests
{
    private readonly IIngresoService _ingresoService;
    private readonly IRepositorioPaciente _repositorioPaciente;
    private readonly IRepositorioIngreso _repositorioIngreso;
    private readonly IRepositorioEnfermero _repositorioEnfermero;
    private string _nombrePaciente;

    public IngresoServiceTests()
    {
        _repositorioPaciente = new RepositorioPacienteEnMemoria();
        _repositorioIngreso = new RepositorioIngresoEnMemoria();
        _repositorioEnfermero = new RepositorioEnfermeroEnMemoria();
        _ingresoService = new IngresoService(_repositorioIngreso, _repositorioPaciente, _repositorioEnfermero);
    }

    [Fact]
    public async Task RegistrarIngreso_PacienteExistente_DatosValidos_DeberiaRegistrarCorrectamente()
    {
        // Arrange
        await CrearPacienteYEnfermero();
        var request = new RegistroIngresoRequest
        {
            CuilPaciente = "20451954270",
            NombrePaciente = "Pedro",
            ApellidoPaciente = "Gomez",
            EmailPaciente = "pedro-gomez@mail.com",
            CalleDomicilio = "9 de julio",
            NumeroDomicilio = 500,
            LocalidadDomicilio = "San miguel de tucuman",
            NombreObraSocial = "OSDE",
            NumeroAfiliado = "123456789",
            Informe = "Dolor de pecho intenso",
            NivelEmergencia = PrioridadTriaje.Critico,
            FrecuenciaCardiaca = 120,
            FrecuenciaRespiratoria = 20,
            TensionSistolica = 140,
            TensionDiastolica = 90,
            MatriculaEnfermero = "ENF001",
            Temperatura = 36
        };

        // Act
        var resultado = await _ingresoService.RegistrarIngresoAsync(request);

        // Assert
        Assert.True(resultado.EsExitoso);
        Assert.NotNull(resultado.Ingreso);
        Assert.Equal(EstadoIngreso.PENDIENTE, resultado.Ingreso.Estado);
        Assert.Equal("Dolor de pecho intenso", resultado.Ingreso.Informe);
        Assert.Equal(PrioridadTriaje.Critico, resultado.Ingreso.NivelEmergencia.Prioridad);
    }

    [Fact]
    public async Task RegistrarIngreso_PacienteNoExistente_DeberiaCrearPaciente()
    {
        // Arrange
        await CrearEnfermero();
        var request = new RegistroIngresoRequest
        {
            CuilPaciente = "20123456785",
            NombrePaciente = "Santino",
            ApellidoPaciente = "Hamada",
            EmailPaciente = "santino.hamada@mail.com",
            CalleDomicilio = "San Martin",
            NumeroDomicilio = 123,
            LocalidadDomicilio = "San Miguel de Tucuman",
            Informe = "Dolor de pecho",
            NivelEmergencia = PrioridadTriaje.Critico,
            FrecuenciaCardiaca = 120,
            FrecuenciaRespiratoria = 20,
            TensionSistolica = 140,
            TensionDiastolica = 90,
            MatriculaEnfermero = "ENF001",
            Temperatura = 37
        };

        // Act
        var resultado = await _ingresoService.RegistrarIngresoAsync(request);

        // Assert
        Assert.True(resultado.EsExitoso);
    }

    [Fact]
    public async Task RegistrarIngreso_InformeVacio_DeberiaRetornarError()
    {
        // Arrange
        await CrearPacienteYEnfermero();
        var request = new RegistroIngresoRequest
        {
            CuilPaciente = "20451954270",
            NombrePaciente = "Pedro",
            ApellidoPaciente = "Gomez",
            EmailPaciente = "pedro-gomez@mail.com",
            CalleDomicilio = "9 de julio",
            NumeroDomicilio = 500,
            LocalidadDomicilio = "San miguel de tucuman",
            NombreObraSocial = "OSDE",
            NumeroAfiliado = "123456789",
            Informe = "",
            NivelEmergencia = PrioridadTriaje.Critico,
            FrecuenciaCardiaca = 120,
            FrecuenciaRespiratoria = 20,
            TensionSistolica = 140,
            TensionDiastolica = 90,
            MatriculaEnfermero = "ENF001"
        };

        // Act
        var resultado = await _ingresoService.RegistrarIngresoAsync(request);

        // Assert
        Assert.False(resultado.EsExitoso);
        Assert.Contains("informe es obligatorio", resultado.MensajeError);
    }

    [Fact]
    public async Task RegistrarIngreso_FrecuenciaCardiacaNegativa_DeberiaRetornarError()
    {
        // Arrange
        await CrearPacienteYEnfermero();
        var request = new RegistroIngresoRequest
        {
            CuilPaciente = "20451954270",
            NombrePaciente = "Pedro",
            ApellidoPaciente = "Gomez",
            EmailPaciente = "pedro-gomez@mail.com",
            CalleDomicilio = "9 de julio",
            NumeroDomicilio = 500,
            LocalidadDomicilio = "San miguel de tucuman",
            NombreObraSocial = "OSDE",
            NumeroAfiliado = "123456789",
            Informe = "Dolor de pecho",
            NivelEmergencia = PrioridadTriaje.Critico,
            FrecuenciaCardiaca = -10,
            FrecuenciaRespiratoria = 20,
            TensionSistolica = 140,
            TensionDiastolica = 90,
            MatriculaEnfermero = "ENF001"
        };

        // Act
        var resultado = await _ingresoService.RegistrarIngresoAsync(request);

        // Assert
        Assert.False(resultado.EsExitoso);
        Assert.Contains("frecuencia cardíaca no puede ser negativa", resultado.MensajeError);
    }

    [Fact]
    public async Task RegistrarIngreso_FrecuenciaRespiratoriaNegativa_DeberiaRetornarError()
    {
        // Arrange
        await CrearPacienteYEnfermero();
        var request = new RegistroIngresoRequest
        {
            CuilPaciente = "20451954270",
            NombrePaciente = "Pedro",
            ApellidoPaciente = "Gomez",
            EmailPaciente = "pedro-gomez@mail.com",
            CalleDomicilio = "9 de julio",
            NumeroDomicilio = 500,
            LocalidadDomicilio = "San miguel de tucuman",
            NombreObraSocial = "OSDE",
            NumeroAfiliado = "123456789",
            Informe = "Dolor de pecho",
            NivelEmergencia = PrioridadTriaje.Critico,
            FrecuenciaCardiaca = 120,
            FrecuenciaRespiratoria = -5,
            TensionSistolica = 140,
            TensionDiastolica = 90,
            MatriculaEnfermero = "ENF001"
        };

        // Act
        var resultado = await _ingresoService.RegistrarIngresoAsync(request);

        // Assert
        Assert.False(resultado.EsExitoso);
        Assert.Contains("frecuencia respiratoria no puede ser negativa", resultado.MensajeError);
    }

    [Fact]
    public async Task ObtenerColaAtencion_ConIngresosPendientes_DeberiaRetornarOrdenadosPorPrioridad()
    {
        // Arrange
        await CrearPacienteYEnfermero();

        // Crear ingreso de emergencia

        var ingresoEmergencia = new Ingreso(
            nivelEmergencia: NivelEmergencia.CrearNivelEmergencia(PrioridadTriaje.Emergencia),
            temperatura: 37.5f,
            frecuenciaCardiaca: 100,
            frecuenciaRespiratoria: 18,
            tensionArterial: new TensionArterial { Sistolica = 130, Diastolica = 85 },
            paciente: (await _repositorioPaciente.ObtenerPorCuilAsync("20451954270"))!,
            enfermero: (await _repositorioEnfermero.ObtenerPorMatriculaAsync("ENF001"))!
            );

        await _repositorioIngreso.CrearAsync(ingresoEmergencia);

        // Crear ingreso crítico
        var ingresoCritico = new Ingreso(
            nivelEmergencia: NivelEmergencia.CrearNivelEmergencia(PrioridadTriaje.Critico),
            temperatura: 39f,
            frecuenciaCardiaca: 120,
            frecuenciaRespiratoria: 20,
            tensionArterial: new TensionArterial { Sistolica = 140, Diastolica = 90 },
            paciente: (await _repositorioPaciente.ObtenerPorCuilAsync("20451954270"))!,
            enfermero: (await _repositorioEnfermero.ObtenerPorMatriculaAsync("ENF001"))!
        );

        await _repositorioIngreso.CrearAsync(ingresoCritico);

        // Act
        var cola = await _ingresoService.ObtenerColaAtencionAsync();

        // Assert
        Assert.Equal(2, cola.Count);
        Assert.Equal(PrioridadTriaje.Critico, cola[0].NivelEmergencia.Prioridad);
        Assert.Equal(PrioridadTriaje.Emergencia, cola[1].NivelEmergencia.Prioridad);
    }

    private async Task CrearPacienteYEnfermero()
    {
        var paciente = new Paciente(
            cuil: "20451954270",
            nombre: "Pedro",
            apellido: "Gomez",
            email: "pedro-gomez@mail.com",
            domicilio: new Domicilio("9 de julio", 500, "San miguel de tucuman"),
            afiliado: new Afiliado(
                new ObraSocial("OSDE")
                , "123456789")
            );

        _nombrePaciente = $"{paciente.Nombre} {paciente.Apellido}";
        await _repositorioPaciente.CrearAsync(paciente);

        var enfermero = new Enfermero(
            cuil: "20345678901",
            nombre: "María",
            apellido: "González",
            email: "matia.gonzales@mail.com",
            matricula: "ENF001"
            );

        await _repositorioEnfermero.CrearAsync(enfermero);
    }

    private async Task CrearEnfermero()
    {
        var enfermero = new Enfermero(
            cuil: "20345678901",
            nombre: "María",
            apellido: "González",
            email: "matia.gonzales@mail.com",
            matricula: "ENF001"
        );

        await _repositorioEnfermero.CrearAsync(enfermero);
    }
}
