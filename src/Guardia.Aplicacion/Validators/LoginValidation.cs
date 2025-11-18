using FluentValidation;
using Guardia.Aplicacion.Servicios;

namespace Guardia.Aplicacion.Validators;
public class LoginValidation : AbstractValidator<LoginDto>
{
    public LoginValidation()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("El nombre de usuario es obligatorio.");
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("La contraseña es obligatoria.")
            .MinimumLength(8).WithMessage("La contraseña debe tener al menos 8 caracteres.");
    }
}
