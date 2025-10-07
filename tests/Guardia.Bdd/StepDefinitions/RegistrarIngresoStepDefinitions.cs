using Microsoft.Extensions.DependencyInjection;
using Guardia.Aplicacion.Servicios;
using Guardia.Dominio.Entidades.Triajes;
using Guardia.Dominio.Entidades.Personal;
using Guardia.Dominio.Entidades;
using Guardia.Dominio.Repositorios;
using Guardia.Infraestructura.Repositorios;
using Guardia.Aplicacion.DTOs;

namespace Guardia.Bdd.StepDefinitions;

[Binding]
public class RegistrarIngresoStepDefinitions
{
    private readonly ServiceProvider _serviceProvider;
    private readonly IngresoService? _ingresoService;
    private ResultadoIngreso? _ultimoResultado;
    private readonly List<Ingreso> _colaAtencion = [];

    private Paciente? _pacienteActual;
    private Enfermero? _enfermeroActual;
    private ulong _dniPacienteActual;
    private string _nombrePaciente;

    public RegistrarIngresoStepDefinitions()
    {
        var services = new ServiceCollection();
        services.AddScoped<IRepositorioPaciente, RepositorioPacienteEnMemoria>();
        services.AddScoped<IRepositorioIngreso, RepositorioIngresoEnMemoria>();
        services.AddScoped<IRepositorioEnfermero, RepositorioEnfermeroEnMemoria>();
        services.AddScoped<IngresoService>();

        _serviceProvider = services.BuildServiceProvider();
        _ingresoService = _serviceProvider.GetRequiredService<IngresoService>();
    }

    [Given(@"que existe un sistema de guardia con niveles de emergencia:")]
    public void DadoQueExisteUnSistemaDeGuardiaConNivelesDeEmergencia(Table table)
    {
    }

    [Given(@"que existe un paciente con DNI ""([^""]*)"" y nombre ""([^""]*)""")]
    public async Task DadoQueExisteUnPacienteConDNIYNombre(string dni, string nombre)
    {
        _dniPacienteActual = ulong.Parse(dni);
        _pacienteActual = new Paciente
        {
            Dni = _dniPacienteActual,
            NombreCompleto = nombre,
            NumeroDeAfiliado = $"AF{dni}"
        };

        var repositorioPaciente = _serviceProvider.GetRequiredService<IRepositorioPaciente>();
        await repositorioPaciente.CrearAsync(_pacienteActual);
    }

    [Given("que no existe un paciente con DNI {string} y nombre {string}")]
    public void GivenQueNoExisteUnPacienteConDNIYNombre(string dni, string nombre)
    {
        _nombrePaciente = nombre;
    }


    [Given(@"que existe un paciente A con DNI ""([^""]*)"" y nombre ""([^""]*)""")]
    public async Task DadoQueExisteUnPacienteAConDNIYNombre(string dni, string nombre)
    {
        var pacienteA = new Paciente
        {
            Dni = ulong.Parse(dni),
            NombreCompleto = nombre,
            NumeroDeAfiliado = $"AF{dni}"
        };
        _nombrePaciente = nombre;
        var repositorioPaciente = _serviceProvider.GetRequiredService<IRepositorioPaciente>();
        await repositorioPaciente.CrearAsync(pacienteA);
    }

    [Given(@"que existe un paciente B con DNI ""([^""]*)"" y nombre ""([^""]*)""")]
    public async Task DadoQueExisteUnPacienteBConDNIYNombre(string dni, string nombre)
    {
        var pacienteB = new Paciente
        {
            Dni = ulong.Parse(dni),
            NombreCompleto = nombre,
            NumeroDeAfiliado = $"AF{dni}"
        };

        var repositorioPaciente = _serviceProvider.GetRequiredService<IRepositorioPaciente>();
        await repositorioPaciente.CrearAsync(pacienteB);
    }

    [Given(@"que existe una enfermera con matrícula ""([^""]*)"" y nombre ""([^""]*)""")]
    public async Task DadoQueExisteUnaEnfermeraConMatriculaYNombre(string matricula, string nombre)
    {
        _enfermeroActual = new Enfermero
        {
            Matricula = matricula,
            NombreCompleto = nombre,
            Dni = 12345678,
            Cuil = 20123456789
        };

        var repositorioEnfermero = _serviceProvider.GetRequiredService<IRepositorioEnfermero>();
        await repositorioEnfermero.CrearAsync(_enfermeroActual);
    }

    [Given(@"el paciente B está en espera con nivel de emergencia ""([^""]*)""")]
    public async Task DadoElPacienteBEstaEnEsperaConNivelDeEmergencia(string nivel)
    {
        var pacienteB = new Paciente
        {
            Dni = 22222222,
            NombreCompleto = "Carlos López",
            NumeroDeAfiliado = "AF22222222"
        };

        var enfermero = _serviceProvider.GetRequiredService<IRepositorioEnfermero>().ObtenerTodosAsync().Result.First();

        var ingresoB = new Ingreso
        {
            Paciente = pacienteB,
            Enfermero = enfermero,
            NivelEmergencia = CrearNivelEmergencia(nivel),
            FechaIngreso = DateTime.Now.AddMinutes(-10),
            Informe = "Dolor abdominal",
            FrecuenciaCardiaca = 80,
            FrecuenciaRespiratoria = 16,
            TensionArterial = new TensionArterial { Sistolica = 120, Diastolica = 80 },
            Estado = EstadoIngreso.PENDIENTE
        };

        var repositorioIngreso = _serviceProvider.GetRequiredService<IRepositorioIngreso>();
        await repositorioIngreso.CrearAsync(ingresoB);

        _colaAtencion.Add(ingresoB);
    }

    [Given(@"el paciente B está en espera con nivel de emergencia ""([^""]*)"" desde hace (\d+) minutos")]
    public async Task DadoElPacienteBEstaEnEsperaConNivelDeEmergenciaDesdeHaceMinutos(string nivel, int minutos)
    {
        var pacienteB = new Paciente
        {
            Dni = 22222222,
            NombreCompleto = "Carlos López",
            NumeroDeAfiliado = "AF22222222"
        };

        var enfermero = _serviceProvider.GetRequiredService<IRepositorioEnfermero>().ObtenerTodosAsync().Result.First();

        var ingresoB = new Ingreso
        {
            Paciente = pacienteB,
            Enfermero = enfermero,
            NivelEmergencia = CrearNivelEmergencia(nivel),
            FechaIngreso = DateTime.Now.AddMinutes(-minutos),
            Informe = "Dolor abdominal",
            FrecuenciaCardiaca = 80,
            FrecuenciaRespiratoria = 16,
            TensionArterial = new TensionArterial { Sistolica = 120, Diastolica = 80 },
            Estado = EstadoIngreso.PENDIENTE
        };

        var repositorioIngreso = _serviceProvider.GetRequiredService<IRepositorioIngreso>();
        await repositorioIngreso.CrearAsync(ingresoB);

        _colaAtencion.Add(ingresoB);
    }

    [When(@"la enfermera registra un ingreso para el paciente con:")]
    public async Task CuandoLaEnfermeraRegistraUnIngresoParaElPacienteCon(Table table)
    {
        _pacienteActual ??= new Paciente
        {
            Dni = _dniPacienteActual,
            NombreCompleto = _nombrePaciente
        };

        var request = new RegistroIngresoRequest
        {
            DniPaciente = _dniPacienteActual,
            NombrePaciente = _pacienteActual.NombreCompleto,
            MatriculaEnfermero = "ENF001"
        };

        foreach (var row in table.Rows)
        {
            var campo = row["Campo"];
            var valor = row["Valor"];

            switch (campo)
            {
                case "Informe":
                    request.Informe = valor;
                    break;
                case "Nivel de Emergencia":
                    request.NivelEmergencia = Enum.Parse<PrioridadTriaje>(valor);
                    break;
                case "Frecuencia Cardiaca":
                    request.FrecuenciaCardiaca = float.Parse(valor);
                    break;
                case "Frecuencia Respiratoria":
                    request.FrecuenciaRespiratoria = float.Parse(valor);
                    break;
                case "Tensión Sistólica":
                    request.TensionSistolica = float.Parse(valor);
                    break;
                case "Tensión Diastólica":
                    request.TensionDiastolica = float.Parse(valor);
                    break;
            }
        }

        _ultimoResultado = await _ingresoService!.RegistrarIngresoAsync(request);
    }

    [When(@"la enfermera intenta registrar un ingreso para el paciente con DNI ""([^""]*)""")]
    public async Task CuandoLaEnfermeraIntentaRegistrarUnIngresoParaElPacienteConDNI(string dni)
    {
        var request = new RegistroIngresoRequest
        {
            DniPaciente = ulong.Parse(dni),
            MatriculaEnfermero = "ENF001",
            Informe = "Test",
            NivelEmergencia = PrioridadTriaje.Urgencia,
            FrecuenciaCardiaca = 80,
            FrecuenciaRespiratoria = 16,
            TensionSistolica = 120,
            TensionDiastolica = 80
        };

        _ultimoResultado = await _ingresoService!.RegistrarIngresoAsync(request);
    }

    [When(@"la enfermera intenta registrar un ingreso para el paciente omitiendo el informe")]
    public async Task CuandoLaEnfermeraIntentaRegistrarUnIngresoParaElPacienteOmitiendoElInforme()
    {
        var request = new RegistroIngresoRequest
        {
            DniPaciente = _dniPacienteActual,
            MatriculaEnfermero = "ENF001",
            Informe = "",
            NivelEmergencia = PrioridadTriaje.Urgencia,
            FrecuenciaCardiaca = 80,
            FrecuenciaRespiratoria = 16,
            TensionSistolica = 120,
            TensionDiastolica = 80
        };

        _ultimoResultado = await _ingresoService!.RegistrarIngresoAsync(request);
    }

    [When(@"la enfermera intenta registrar un ingreso con frecuencia cardíaca ""([^""]*)""")]
    public async Task CuandoLaEnfermeraIntentaRegistrarUnIngresoConFrecuenciaCardiaca(string frecuencia)
    {
        var request = new RegistroIngresoRequest
        {
            DniPaciente = _dniPacienteActual,
            NombrePaciente = _pacienteActual!.NombreCompleto,
            MatriculaEnfermero = "ENF001",
            Informe = "Test",
            NivelEmergencia = PrioridadTriaje.Urgencia,
            FrecuenciaCardiaca = float.Parse(frecuencia),
            FrecuenciaRespiratoria = 16,
            TensionSistolica = 120,
            TensionDiastolica = 80
        };

        _ultimoResultado = await _ingresoService!.RegistrarIngresoAsync(request);
    }

    [When(@"la enfermera intenta registrar un ingreso con frecuencia respiratoria ""([^""]*)""")]
    public async Task CuandoLaEnfermeraIntentaRegistrarUnIngresoConFrecuenciaRespiratoria(string frecuencia)
    {
        var request = new RegistroIngresoRequest
        {
            DniPaciente = _dniPacienteActual,
            NombrePaciente = _pacienteActual!.NombreCompleto,
            MatriculaEnfermero = "ENF001",
            Informe = "Test",
            NivelEmergencia = PrioridadTriaje.Urgencia,
            FrecuenciaCardiaca = 80,
            FrecuenciaRespiratoria = float.Parse(frecuencia),
            TensionSistolica = 120,
            TensionDiastolica = 80
        };

        _ultimoResultado = await _ingresoService!.RegistrarIngresoAsync(request);
    }

    [When(@"la enfermera registra un ingreso para el paciente A con nivel ""([^""]*)""")]
    public async Task CuandoLaEnfermeraRegistraUnIngresoParaElPacienteAConNivel(string nivel)
    {
        var request = new RegistroIngresoRequest
        {
            DniPaciente = 11111111,
            NombrePaciente = _nombrePaciente,
            MatriculaEnfermero = "ENF001",
            Informe = "Dolor de pecho",
            NivelEmergencia = Enum.Parse<PrioridadTriaje>(nivel),
            FrecuenciaCardiaca = 120,
            FrecuenciaRespiratoria = 20,
            TensionSistolica = 140,
            TensionDiastolica = 90
        };

        _ultimoResultado = await _ingresoService!.RegistrarIngresoAsync(request);
    }

    [Then(@"el ingreso se registra correctamente")]
    public void EntoncesElIngresoSeRegistraCorrectamente()
    {
        Assert.True(_ultimoResultado!.EsExitoso);
        Assert.NotNull(_ultimoResultado.Ingreso);
    }

    [Then(@"el paciente queda en estado PENDIENTE")]
    public void EntoncesElPacienteQuedaEnEstadoPENDIENTE()
    {
        Assert.Equal(EstadoIngreso.PENDIENTE, _ultimoResultado!.Ingreso!.Estado);
    }

    [Then(@"el ingreso se agrega a la cola de atención")]
    public async Task EntoncesElIngresoSeAgregaALaColaDeAtencion()
    {
        var cola = await _ingresoService!.ObtenerColaAtencionAsync();
        Assert.Contains(_ultimoResultado!.Ingreso!, cola);
    }

    [Then(@"se debe crear el paciente antes de proceder al registro del ingreso")]
    public void EntoncesSeDebeCrearElPacienteAntesDeProcederAlRegistroDelIngreso()
    {
        _pacienteActual = new Paciente
        {
            Dni = _ultimoResultado!.Ingreso!.Paciente.Dni,
            NombreCompleto = _ultimoResultado!.Ingreso.Paciente.NombreCompleto,
        };
        Assert.True(_ultimoResultado!.EsExitoso);
    }

    [Then(@"se emite un mensaje de error indicando que el informe es obligatorio")]
    public void EntoncesSeEmiteUnMensajeDeErrorIndicandoQueElInformeEsObligatorio()
    {
        Assert.False(_ultimoResultado!.EsExitoso);
        Assert.Contains("informe es obligatorio", _ultimoResultado.MensajeError!);
    }

    [Then(@"se emite un mensaje de error indicando que la frecuencia cardíaca no puede ser negativa")]
    public void EntoncesSeEmiteUnMensajeDeErrorIndicandoQueLaFrecuenciaCardiacaNoPuedeSerNegativa()
    {
        Assert.False(_ultimoResultado!.EsExitoso);
        Assert.Contains("frecuencia cardíaca no puede ser negativa", _ultimoResultado.MensajeError!);
    }

    [Then(@"se emite un mensaje de error indicando que la frecuencia respiratoria no puede ser negativa")]
    public void EntoncesSeEmiteUnMensajeDeErrorIndicandoQueLaFrecuenciaRespiratoriaNoPuedeSerNegativa()
    {
        Assert.False(_ultimoResultado!.EsExitoso);
        Assert.Contains("frecuencia respiratoria no puede ser negativa", _ultimoResultado.MensajeError!);
    }

    [Then(@"el paciente A debe ser atendido antes que el paciente B")]
    public async Task EntoncesElPacienteADebeSerAtendidoAntesQueElPacienteB()
    {
        var cola = await _ingresoService!.ObtenerColaAtencionAsync();
        var pacienteA = cola.FirstOrDefault(i => i.Paciente.Dni == 11111111);
        var pacienteB = cola.FirstOrDefault(i => i.Paciente.Dni == 22222222);

        Assert.NotNull(pacienteA);
        Assert.NotNull(pacienteB);

        var indiceA = cola.IndexOf(pacienteA);
        var indiceB = cola.IndexOf(pacienteB);

        Assert.True(indiceA < indiceB);
    }

    [Then(@"el paciente B debe ser atendido antes que el paciente A")]
    public async Task EntoncesElPacienteBDebeSerAtendidoAntesQueElPacienteA()
    {
        var cola = await _ingresoService!.ObtenerColaAtencionAsync();
        var pacienteA = cola.FirstOrDefault(i => i.Paciente.Dni == 11111111);
        var pacienteB = cola.FirstOrDefault(i => i.Paciente.Dni == 22222222);

        Assert.NotNull(pacienteA);
        Assert.NotNull(pacienteB);

        var indiceA = cola.IndexOf(pacienteA);
        var indiceB = cola.IndexOf(pacienteB);

        Assert.True(indiceB < indiceA);
    }

    private NivelEmergencia CrearNivelEmergencia(string nivel)
    {
        var prioridad = Enum.Parse<PrioridadTriaje>(nivel);
        return prioridad switch
        {
            PrioridadTriaje.Critico => new NivelEmergencia { Prioridad = prioridad, Color = "Rojo", TiempoMaximoMinutos = 5 },
            PrioridadTriaje.Emergencia => new NivelEmergencia { Prioridad = prioridad, Color = "Naranja", TiempoMaximoMinutos = 30 },
            PrioridadTriaje.Urgencia => new NivelEmergencia { Prioridad = prioridad, Color = "Amarillo", TiempoMaximoMinutos = 60 },
            PrioridadTriaje.UrgenciaMenor => new NivelEmergencia { Prioridad = prioridad, Color = "Verde", TiempoMaximoMinutos = 120 },
            PrioridadTriaje.SinUrgencia => new NivelEmergencia { Prioridad = prioridad, Color = "Azul", TiempoMaximoMinutos = 240 },
            _ => throw new ArgumentException()
        };
    }
}
