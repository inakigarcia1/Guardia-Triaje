using Guardia.Dominio.Entidades;
using Guardia.Dominio.Repositorios;
using Microsoft.AspNetCore.Http;

namespace Guardia.Aplicacion.Servicios;

public class AtencionService : IAtencionService
{
    private readonly IIngresoService _ingresoService;
    private readonly IRepositorioIngreso _repositorioIngreso;
    private readonly IRepositorioMedico _repositorioMedico;
    private readonly IRepositorioAtencion _repositorioAtencion;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AtencionService(
        IIngresoService ingresoService,
        IRepositorioIngreso repositorioIngreso,
        IRepositorioMedico repositorioMedico,
        IRepositorioAtencion repositorioAtencion,
        IHttpContextAccessor httpContextAccessor)
    {
        _ingresoService = ingresoService;
        _repositorioIngreso = repositorioIngreso;
        _repositorioMedico = repositorioMedico;
        _repositorioAtencion = repositorioAtencion;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IngresoSiguienteDto?> ReclamarSiguiente()
    {
        var cola = await _ingresoService.ObtenerColaAtencionAsync();
        if (cola.Count == 0)
        {
            return null;
        }

        var ingreso = cola.First();
        ingreso.Estado = EstadoIngreso.EN_PROCESO;
        await _repositorioIngreso.ActualizarAsync(ingreso);

        return new IngresoSiguienteDto
        {
            Id = ingreso.Id,
            FechaIngreso = ingreso.FechaIngreso,
            Informe = ingreso.Informe,
            NivelEmergencia = ingreso.NivelEmergencia.Prioridad.ToString(),
            Color = ingreso.NivelEmergencia.Color,
            TiempoMaximoMinutos = ingreso.NivelEmergencia.TiempoMaximoMinutos,
            Temperatura = ingreso.Temperatura,
            FrecuenciaCardiaca = ingreso.FrecuenciaCardiaca,
            FrecuenciaRespiratoria = ingreso.FrecuenciaRespiratoria,
            TensionArterial = ingreso.TensionArterial.ToString(),
            PacienteCuil = ingreso.Paciente.Cuil,
            PacienteNombre = ingreso.Paciente.Nombre,
            EnfermeroMatricula = ingreso.Enfermero.Matricula,
            EnfermeroNombre = ingreso.Enfermero.Nombre
        };
    }

    public async Task RegistrarAtencion(RegistroAtencionDto dto)
    {
        var ingreso = await _repositorioIngreso.ObtenerPorIdAsync(dto.IngresoId);
        if (ingreso == null)
        {
            throw new InvalidOperationException("Ingreso no encontrado");
        }

        var matricula = _httpContextAccessor.HttpContext?.User?.FindFirst("Matricula")?.Value;
        if (string.IsNullOrEmpty(matricula))
        {
            throw new UnauthorizedAccessException("Matrícula no encontrada en el token");
        }

        var medico = await _repositorioMedico.ObtenerPorMatriculaAsync(matricula);
        if (medico == null)
        {
            throw new InvalidOperationException("Médico no encontrado");
        }

        var atencion = new Atencion
        {
            Inicio = dto.Inicio,
            Fin = dto.Fin,
            DiagnosticoPresuntivo = dto.DiagnosticoPresuntivo,
            ProcedimientoRealizado = dto.ProcedimientoRealizado,
            Observaciones = dto.Observaciones,
            Ingreso = ingreso,
            Medico = medico
        };

        await _repositorioAtencion.CrearAsync(atencion);
        medico.RegistrarAtencion(atencion);
        ingreso.Estado = EstadoIngreso.FINALIZADO;
        await _repositorioIngreso.ActualizarAsync(ingreso);
    }

    public async Task<List<AtencionDto>> ObtenerTodasAsync()
    {
        var atenciones = await _repositorioAtencion.ObtenerTodosAsync();
        return atenciones.Select(a => new AtencionDto
        {
            Inicio = a.Inicio,
            Fin = a.Fin,
            DiagnosticoPresuntivo = a.DiagnosticoPresuntivo,
            ProcedimientoRealizado = a.ProcedimientoRealizado,
            Observaciones = a.Observaciones,
            IngresoId = a.Ingreso.Id,
            FechaIngreso = a.Ingreso.FechaIngreso,
            MedicoMatricula = a.Medico.Matricula,
            MedicoNombre = a.Medico.Nombre,
            PacienteCuil = a.Ingreso.Paciente.Cuil,
            PacienteNombre = a.Ingreso.Paciente.Nombre
        }).ToList();
    }
}

