using Application.Features.Comments.Commands.DeleteComment;
using FluentValidation;

namespace Application.Validators.Comments
{
    public class DeleteCommentCommandValidator : AbstractValidator<DeleteCommentCommand>
    {
        public DeleteCommentCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El identificador del comentario es requerido.");
        }
    }
}
