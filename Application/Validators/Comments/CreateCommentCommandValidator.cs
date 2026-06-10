using Application.Features.Comments.Commands.CreateComment;
using FluentValidation;

namespace Application.Validators.Comments
{
    public class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
    {
        public CreateCommentCommandValidator()
        {
            RuleFor(x => x.CommentContent)
                .NotEmpty().WithMessage("El contenido del comentario es requerido.")
                .MaximumLength(500).WithMessage("El comentario no debe exceder los 500 caracteres.");
            RuleFor(x => x.PostId)
                .GreaterThan(0).WithMessage("El identificador del post es requerido.");
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("El identificador del usuario es requerido.");
        }
    }
}
