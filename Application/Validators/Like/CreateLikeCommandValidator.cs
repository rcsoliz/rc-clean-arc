using Application.Features.Likes.Commands.CreateLike;
using FluentValidation;

namespace Application.Validators.Like
{
    public class CreateLikeCommandValidator: AbstractValidator<CreateLikeCommand>
    {
        public CreateLikeCommandValidator()
        {
            RuleFor(p => p.UserId)
                .NotEmpty().WithMessage("El usuario es requerido");
            RuleFor(p => p.PostId)
                .NotEmpty().WithMessage("El post es requerido");
        }
    }
}
