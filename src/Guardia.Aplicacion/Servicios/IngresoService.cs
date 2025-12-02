using Guardia.Aplicacion.DTOs;
using Guardia.Dominio.Entidades;
using Guardia.Dominio.Repositorios;

namespace Guardia.Aplicacion.Servicios;

public class IngresoService : IIngresoService
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

            // Buscar enfermero
            var enfermero = await _repositorioEnfermero.ObtenerPorMatriculaAsync(request.MatriculaEnfermero);
            if (enfermero is null)
            {
                return new ResultadoIngreso { EsExitoso = false, MensajeError = "Enfermero no encontrado." };
            }

            var paciente = await _repositorioPaciente.ObtenerPorCuilAsync(request.CuilPaciente);
            if (paciente is null)
            {
                var domicilio = new Domicilio(request.CalleDomicilio, request.NumeroDomicilio, request.LocalidadDomicilio);
                Afiliado? afiliado = null;
                if (!string.IsNullOrWhiteSpace(request.NombreObraSocial) && !string.IsNullOrWhiteSpace(request.NumeroAfiliado))
                {
                    var obraSocial = new ObraSocial(request.NombreObraSocial);
                    afiliado = new Afiliado(obraSocial, request.NumeroAfiliado);
                }
                paciente = new Paciente(
                    request.CuilPaciente,
                    request.NombrePaciente,
                    request.ApellidoPaciente,
                    request.EmailPaciente,
                    domicilio,
                    afiliado
                );
                await _repositorioPaciente.CrearAsync(paciente);
            }

            var ingreso = new Ingreso(
                nivelEmergencia: NivelEmergencia.CrearNivelEmergencia(request.NivelEmergencia),
                temperatura: request.Temperatura,
                frecuenciaCardiaca: request.FrecuenciaCardiaca,
                frecuenciaRespiratoria: request.FrecuenciaRespiratoria,
                tensionArterial: new TensionArterial
                {
                    Sistolica = request.TensionSistolica,
                    Diastolica = request.TensionDiastolica
                },
                paciente: paciente,
                enfermero: enfermero)
                {
                    Informe = request.Informe
                };

            // Guardar ingreso
            await _repositorioIngreso.CrearAsync(ingreso);
            enfermero.RegistrarIngreso(ingreso);

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

    public async Task<PacienteDto?> BuscarPacientePorCuilAsync(string cuil)
    {
        var paciente = await _repositorioPaciente.ObtenerPorCuilAsync(cuil);
        if (paciente is null)
            return null;

        return new PacienteDto
        {
            Cuil = paciente.Cuil,
            Nombre = paciente.Nombre,
            Apellido = paciente.Apellido,
            Email = paciente.Email,
            Domicilio = paciente.Domicilio != null ? new DomicilioDto
            {
                Calle = paciente.Domicilio.Calle,
                Numero = paciente.Domicilio.Numero,
                Localidad = paciente.Domicilio.Localidad,
                Provincia = paciente.Domicilio.Provincia,
                Pais = paciente.Domicilio.Pais
            } : null,
            Afiliado = paciente.Afiliado != null ? new AfiliadoDto
            {
                NombreObraSocial = paciente.Afiliado.ObraSocial.Nombre,
                NumeroAfiliado = paciente.Afiliado.NumeroAfiliado
            } : null
        };
    }

    private ValidacionResultado ValidarDatosMandatorios(RegistroIngresoRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Informe))
            return new ValidacionResultado { EsValido = false, MensajeError = "El informe es obligatorio" };

        if (string.IsNullOrWhiteSpace(request.MatriculaEnfermero))
            return new ValidacionResultado { EsValido = false, MensajeError = "La matrícula del enfermero es obligatoria" };

        if (string.IsNullOrWhiteSpace(request.NombrePaciente))
            return new ValidacionResultado { EsValido = false, MensajeError = "El nombre del paciente es obligatorio" };

        if (string.IsNullOrWhiteSpace(request.ApellidoPaciente))
            return new ValidacionResultado { EsValido = false, MensajeError = "El apellido del paciente es obligatorio" };

        if (string.IsNullOrWhiteSpace(request.EmailPaciente))
            return new ValidacionResultado { EsValido = false, MensajeError = "El email del paciente es obligatorio" };

        if (string.IsNullOrWhiteSpace(request.CalleDomicilio))
            return new ValidacionResultado { EsValido = false, MensajeError = "La calle del domicilio es obligatoria" };

        if (request.NumeroDomicilio <= 0)
            return new ValidacionResultado { EsValido = false, MensajeError = "El número del domicilio debe ser mayor a cero" };

        if (string.IsNullOrWhiteSpace(request.LocalidadDomicilio))
            return new ValidacionResultado { EsValido = false, MensajeError = "La localidad del domicilio es obligatoria" };

        if (!string.IsNullOrWhiteSpace(request.NombreObraSocial) && string.IsNullOrWhiteSpace(request.NumeroAfiliado))
            return new ValidacionResultado { EsValido = false, MensajeError = "Si se especifica obra social, el número de afiliado es obligatorio" };

        if (string.IsNullOrWhiteSpace(request.NombreObraSocial) && !string.IsNullOrWhiteSpace(request.NumeroAfiliado))
            return new ValidacionResultado { EsValido = false, MensajeError = "Si se especifica número de afiliado, la obra social es obligatoria" };

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
}