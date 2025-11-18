using FluentValidation;
using Guardia.Aplicacion.DTOs;

namespace Guardia.Aplicacion.Validators;
public class RegistroMedicoValidator : AbstractValidator<RegistroMedicoDto>
{
    public RegistroMedicoValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("El nombre de usuario es obligatorio.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El correo electrónico es obligatorio.")
            .EmailAddress().WithMessage("El correo electrónico no es válido.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("La contraseña es obligatoria.")
            .MinimumLength(8).WithMessage("La contraseña debe tener al menos 8 caracteres.");

        RuleFor(x => x.Matricula).NotNull().NotEmpty().WithMessage("La matrícula es obligatoria.");
    }
}
