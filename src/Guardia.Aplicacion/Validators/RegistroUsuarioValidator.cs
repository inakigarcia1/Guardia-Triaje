using FluentValidation;
using Guardia.Aplicacion.Servicios;

namespace Guardia.Aplicacion.Validators;
public class RegistroUsuarioValidator : AbstractValidator<RegistroUsuarioDto>
{
    public RegistroUsuarioValidator()
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

        RuleFor(x => x.Cuil)
            .NotNull().NotEmpty().WithMessage("El CUIL es obligatorio.")
            .Matches(@"^\d+$").WithMessage("El CUIL solo puede contener números.")
            .MinimumLength(10).WithMessage("El CUIL debe tener al menos 10 dígitos.")
            .MaximumLength(11).WithMessage("El CUIL no puede tener más de 11 dígitos.");
    }
}

