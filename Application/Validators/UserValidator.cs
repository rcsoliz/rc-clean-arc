using Core.Entities;
using FluentValidation;

namespace Application.Validators
{
    public class UserValidator:AbstractValidator<User>
    {
        public UserValidator() {
            RuleFor(p => p.Username)
                 .NotEmpty().WithMessage("El nombre del usuario es obligatorio");
            RuleFor(p => p.Email)
                .NotEmpty().WithMessage("El email del usuario es obligatorio")
                .EmailAddress().WithMessage("El email no es valido");
            RuleFor(p => p.PasswordHash)
                .NotEmpty().WithMessage("La contraseña del usuario es obligatoria")
                .NotNull().WithMessage("La contraseña del usuario es obligatoria")
                .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres");
        }
    }
}
