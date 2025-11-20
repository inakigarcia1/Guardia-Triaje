using FluentValidation;
using Guardia.Aplicacion.DTOs;

namespace Guardia.Aplicacion.Validators;
public class RegistroIngresoValidator : AbstractValidator<RegistroIngresoRequest>
{
    public RegistroIngresoValidator()
    {
        // Historia 
        RuleFor(i => i.Informe).NotNull().NotEmpty().WithMessage("El informe es obligatorio.");
        RuleFor(i => i.NivelEmergencia).NotNull().NotEmpty().WithMessage("El nivel de emergencia es obligatorio.");
        RuleFor(i => i.Temperatura).GreaterThan(0).WithMessage("La temperatura debe ser mayor que 0.");
        RuleFor(i => i.FrecuenciaCardiaca).GreaterThan(0).WithMessage("La frecuencia cardíaca debe ser mayor que 0.");
        RuleFor(i => i.FrecuenciaRespiratoria).GreaterThan(0).WithMessage("La frecuencia respiratoria debe ser mayor que 0.");
        RuleFor(i => i.TensionSistolica).GreaterThan(0).WithMessage("La tensión sistólica debe ser mayor que 0.");
        RuleFor(i => i.TensionDiastolica).GreaterThan(0).WithMessage("La tensión diastólica debe ser mayor que 0.");

        // Historia 2: Registro de paciente
        RuleFor(i => i.CuilPaciente).NotNull().NotEmpty().WithMessage("El CUIL del paciente es obligatorio.");
        RuleFor(i => i.CuilPaciente).MinimumLength(10).WithMessage("El CUIL del paciente debe tener al menos 10 dígitos.");
        RuleFor(i => i.CuilPaciente).MaximumLength(11).WithMessage("El CUIL del paciente no puede tener más de 11 dígitos.");
        RuleFor(i => i.NombrePaciente).NotNull().NotEmpty().WithMessage("El nombre del paciente es obligatorio.");
        RuleFor(i => i.ApellidoPaciente).NotNull().NotEmpty().WithMessage("El apellido del paciente es obligatorio.");
        RuleFor(i => i.EmailPaciente).NotNull().NotEmpty().WithMessage("El email del paciente es obligatorio.").EmailAddress().WithMessage("El email del paciente no es válido.");
        RuleFor(i => i.CalleDomicilio).NotNull().NotEmpty().WithMessage("La calle del domicilio es obligatoria.");
        RuleFor(i => i.NumeroDomicilio).GreaterThan(0).WithMessage("El número del domicilio debe ser mayor que 0.");
        RuleFor(i => i.LocalidadDomicilio).NotNull().NotEmpty().WithMessage("La localidad del domicilio es obligatoria.");
        RuleFor(i => i).Must(VerificarObraSocial).WithMessage("Si se proporciona el nombre de la obra social, también debe proporcionarse el número de afiliado, y viceversa.");
    }

    private static bool VerificarObraSocial(RegistroIngresoRequest request)
    {
        bool bien = request.NombreObraSocial is null && request.NumeroAfiliado is null || request.NombreObraSocial is not null && request.NumeroAfiliado is not null;
        return bien;
    }
}
