using FluentValidation;
using Guardia.Aplicacion.Servicios;

namespace Guardia.Aplicacion.Validators;
public class RegistroAtencionValidator : AbstractValidator<RegistroAtencionDto>
{
    public RegistroAtencionValidator()
    {
        RuleFor(a => a.Observaciones).NotNull().NotEmpty().WithMessage("Las observaciones son obligatorias.");
        RuleFor(a => a.DiagnosticoPresuntivo).NotNull().NotEmpty().WithMessage("El diagnóstico presuntivo es obligatorio.");
        RuleFor(a => a.ProcedimientoRealizado).NotNull().NotEmpty().WithMessage("El procedimiento realizado es obligatorio.");
        RuleFor(a => a.IngresoId).NotNull().NotEmpty().WithMessage("El ID de ingreso es obligatorio."); 
    }
}
