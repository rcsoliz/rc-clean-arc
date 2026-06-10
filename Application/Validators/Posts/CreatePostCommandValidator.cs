using Application.Features.Posts.Commands.CreatePost;
using FluentValidation;

namespace Application.Validators.Posts
{
    public class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
    {
        public CreatePostCommandValidator()
        {
            RuleFor(x => x.PostContent)
                .NotEmpty().WithMessage("El contenido del post es requerido.")
                .MaximumLength(1000).WithMessage("El contenido no debe exceder los 1000 caracteres.");
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("El identificador del usuario es requerido.");
        }
    }
}
