using Application.Features.Likes.Commands.DeleteLike;
using FluentValidation;

namespace Application.Validators.Like
{
    public class DeleteLikeCommandValidator: AbstractValidator<DeleteLikeCommand>
    {
        public DeleteLikeCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El Id del like debe ser mayor que 0.");
        }
    }
}
