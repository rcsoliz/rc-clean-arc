using Application.Features.Likes.Commands.UpdateLike;
using FluentValidation;

namespace Application.Validators.Like
{
    public class UpdateLikeCommandValidator : AbstractValidator<UpdateLikeCommand>
    {
        public UpdateLikeCommandValidator() 
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("El Id del usuario debe ser mayor que 0.");
            RuleFor(x => x.PostId)
                .GreaterThan(0).WithMessage("El Id del post debe ser mayor que 0.");
            RuleFor(x => x.CommentId)
               .GreaterThan(0).WithMessage("El Id del comentario debe ser mayor que 0.");
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El Id del like debe ser mayor que 0.");

        }
    }
}
