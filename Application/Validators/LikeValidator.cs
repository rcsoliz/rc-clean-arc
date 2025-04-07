using Core.Entities;
using FluentValidation;


namespace Application.Validators 
{
    public class LikeValidator : AbstractValidator<Like>
    {
        public LikeValidator() {
            RuleFor(p => p.UserId)
                .NotEmpty().WithMessage("El usuario es requerido");
            RuleFor(p => p.PostId)
                .NotEmpty().WithMessage("El post es requerido");
        }
    }
}
