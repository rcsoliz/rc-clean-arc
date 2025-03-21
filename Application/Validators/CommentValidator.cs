using Core.Entities;
using FluentValidation;

namespace Application.Validators
{
    public class CommentValidator:AbstractValidator<Comment>
    {
        public CommentValidator()
        {
            RuleFor(p => p.CommentContent)
                .NotEmpty().WithMessage("El contenido del comentario es obligatorio");
        }
    }
}
