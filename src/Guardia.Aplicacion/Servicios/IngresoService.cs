using Guardia.Aplicacion.DTOs;
using Guardia.Dominio.Entidades;
using Guardia.Dominio.Entidades.Triajes;
using Guardia.Dominio.Repositorios;

namespace Guardia.Aplicacion.Servicios;

public class IngresoService
{
    private readonly IRepositorioIngreso _repositorioIngreso;
    private readonly IRepositorioPaciente _repositorioPaciente;
    private readonly IRepositorioEnfermero _repositorioEnfermero;

    public IngresoService(
        IRepositorioIngreso repositorioIngreso,
        IRepositorioPaciente repositorioPaciente,
        IRepositorioEnfermero repositorioEnfermero)
    {
        _repositorioIngreso = repositorioIngreso;
        _repositorioPaciente = repositorioPaciente;
        _repositorioEnfermero = repositorioEnfermero;
    }

    public async Task<ResultadoIngreso> RegistrarIngresoAsync(RegistroIngresoRequest request)
    {
        try
        {
            // Validar datos mandatorios
            var validacion = ValidarDatosMandatorios(request);
            if (!validacion.EsValido)
            {
                return new ResultadoIngreso { EsExitoso = false, MensajeError = validacion.MensajeError };
            }

            // Validar valores negativos
            var validacionNegativos = ValidarSignosVitales(request);
            if (!validacionNegativos.EsValido)
            {
                return new ResultadoIngreso { EsExitoso = false, MensajeError = validacionNegativos.MensajeError };
            }

            // Buscar o crear paciente
            var paciente = await _repositorioPaciente.ObtenerPorDniAsync(request.DniPaciente);
            if (paciente == null)
            {
                paciente = new Paciente
                {
                    Dni = request.DniPaciente,
                    NombreCompleto = request.NombrePaciente
                };

                await _repositorioPaciente.CrearAsync(paciente);
            }

            // Buscar enfermero
            var enfermero = await _repositorioEnfermero.ObtenerPorMatriculaAsync(request.MatriculaEnfermero);
            if (enfermero == null)
            {
                return new ResultadoIngreso { EsExitoso = false, MensajeError = "Enfermero no encontrado." };
            }

            // Crear ingreso
            var ingreso = new Ingreso
            {
                FechaIngreso = DateTime.Now,
                Informe = request.Informe,
                NivelEmergencia = CrearNivelEmergencia(request.NivelEmergencia),
                Estado = EstadoIngreso.PENDIENTE,
                Temperatura = request.Temperatura,
                FrecuenciaCardiaca = request.FrecuenciaCardiaca,
                FrecuenciaRespiratoria = request.FrecuenciaRespiratoria,
                TensionArterial = new TensionArterial
                {
                    Sistolica = request.TensionSistolica,
                    Diastolica = request.TensionDiastolica
                },
                Paciente = paciente,
                Enfermero = enfermero
            };

            // Guardar ingreso
            await _repositorioIngreso.CrearAsync(ingreso);

            return new ResultadoIngreso 
            { 
                EsExitoso = true, 
                Ingreso = ingreso,
                Mensaje = "Ingreso registrado correctamente"
            };
        }
        catch (Exception ex)
        {
            return new ResultadoIngreso { EsExitoso = false, MensajeError = $"Error interno: {ex.Message}" };
        }
    }

    public async Task<List<Ingreso>> ObtenerColaAtencionAsync()
    {
        return await _repositorioIngreso.ObtenerPendientesAsync();
    }

    private ValidacionResultado ValidarDatosMandatorios(RegistroIngresoRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Informe))
            return new ValidacionResultado { EsValido = false, MensajeError = "El informe es obligatorio" };

        if (string.IsNullOrWhiteSpace(request.MatriculaEnfermero))
            return new ValidacionResultado { EsValido = false, MensajeError = "La matrícula del enfermero es obligatoria" };

        if(string.IsNullOrWhiteSpace(request.NombrePaciente))
            return new ValidacionResultado { EsValido = false, MensajeError = "El nombre del paciente es obligatorio" };

        return new ValidacionResultado { EsValido = true };
    }

    private ValidacionResultado ValidarSignosVitales(RegistroIngresoRequest request)
    {
        if (request.FrecuenciaCardiaca <= 0)
            return new ValidacionResultado { EsValido = false, MensajeError = "La frecuencia cardíaca no puede ser negativa" };

        if (request.FrecuenciaRespiratoria <= 0)
            return new ValidacionResultado { EsValido = false, MensajeError = "La frecuencia respiratoria no puede ser negativa" };

        if (request.TensionSistolica <= 0)
            return new ValidacionResultado { EsValido = false, MensajeError = "La tensión sistólica no puede ser negativa" };

        if (request.TensionDiastolica <= 0)
            return new ValidacionResultado { EsValido = false, MensajeError = "La tensión diastólica no puede ser negativa" };

        return new ValidacionResultado { EsValido = true };
    }

    private NivelEmergencia CrearNivelEmergencia(PrioridadTriaje prioridad)
    {
        return prioridad switch
        {
            PrioridadTriaje.Critico => new NivelEmergencia { Prioridad = prioridad, Color = "Rojo", TiempoMaximoMinutos = 5 },
            PrioridadTriaje.Emergencia => new NivelEmergencia { Prioridad = prioridad, Color = "Naranja", TiempoMaximoMinutos = 30 },
            PrioridadTriaje.Urgencia => new NivelEmergencia { Prioridad = prioridad, Color = "Amarillo", TiempoMaximoMinutos = 60 },
            PrioridadTriaje.UrgenciaMenor => new NivelEmergencia { Prioridad = prioridad, Color = "Verde", TiempoMaximoMinutos = 120 },
            PrioridadTriaje.SinUrgencia => new NivelEmergencia { Prioridad = prioridad, Color = "Azul", TiempoMaximoMinutos = 240 },
            _ => throw new ArgumentException("Nivel de emergencia no válido")
        };
    }
}