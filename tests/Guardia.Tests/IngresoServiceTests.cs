using Guardia.Aplicacion.DTOs;
using Guardia.Aplicacion.Servicios;
using Guardia.Dominio.Entidades;
using Guardia.Dominio.Entidades.Personal;
using Guardia.Dominio.Entidades.Triajes;
using Guardia.Dominio.Repositorios;
using Guardia.Infraestructura.Repositorios;

namespace Guardia.Tests;

public class IngresoServiceTests
{
    private readonly IngresoService _ingresoService;
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
            DniPaciente = 12345678,
            NombrePaciente = _nombrePaciente,
            Informe = "Dolor de pecho intenso",
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
            DniPaciente = 99999999,
            NombrePaciente = "Santino Hamada",
            Informe = "Dolor de pecho",
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
        Assert.True(resultado.EsExitoso);
    }

    [Fact]
    public async Task RegistrarIngreso_InformeVacio_DeberiaRetornarError()
    {
        // Arrange
        await CrearPacienteYEnfermero();
        var request = new RegistroIngresoRequest
        {
            DniPaciente = 12345678,
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
            DniPaciente = 12345678,
            NombrePaciente = _nombrePaciente,
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
            DniPaciente = 12345678,
            NombrePaciente = _nombrePaciente,
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
        var ingresoEmergencia = new Ingreso
        {
            Paciente = await _repositorioPaciente.ObtenerPorDniAsync(12345678),
            Enfermero = await _repositorioEnfermero.ObtenerPorMatriculaAsync("ENF001"),
            NivelEmergencia = new NivelEmergencia { Prioridad = PrioridadTriaje.Emergencia, Color = "Naranja", TiempoMaximoMinutos = 30 },
            FechaIngreso = DateTime.Now.AddMinutes(-5),
            Informe = "Emergencia",
            FrecuenciaCardiaca = 100,
            FrecuenciaRespiratoria = 18,
            TensionArterial = new TensionArterial { Sistolica = 130, Diastolica = 85 },
            Estado = EstadoIngreso.PENDIENTE
        };
        await _repositorioIngreso.CrearAsync(ingresoEmergencia);

        // Crear ingreso crítico
        var ingresoCritico = new Ingreso
        {
            Paciente = await _repositorioPaciente.ObtenerPorDniAsync(12345678),
            Enfermero = await _repositorioEnfermero.ObtenerPorMatriculaAsync("ENF001"),
            NivelEmergencia = new NivelEmergencia { Prioridad = PrioridadTriaje.Critico, Color = "Rojo", TiempoMaximoMinutos = 5 },
            FechaIngreso = DateTime.Now,
            Informe = "Crítico",
            FrecuenciaCardiaca = 120,
            FrecuenciaRespiratoria = 20,
            TensionArterial = new TensionArterial { Sistolica = 140, Diastolica = 90 },
            Estado = EstadoIngreso.PENDIENTE
        };
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
        var paciente = new Paciente
        {
            Dni = 12345678,
            NombreCompleto = "Juan Pérez",
            NumeroDeAfiliado = "AF12345678"
        };
        _nombrePaciente = paciente.NombreCompleto;
        await _repositorioPaciente.CrearAsync(paciente);

        var enfermero = new Enfermero
        {
            Matricula = "ENF001",
            NombreCompleto = "María González",
            Dni = 87654321,
            Cuil = 20876543219
        };
        await _repositorioEnfermero.CrearAsync(enfermero);
    }

    private async Task CrearEnfermero()
    {
        var enfermero = new Enfermero
        {
            Matricula = "ENF001",
            NombreCompleto = "María González",
            Dni = 87654321,
            Cuil = 20876543219
        };
        await _repositorioEnfermero.CrearAsync(enfermero);
    }
}
