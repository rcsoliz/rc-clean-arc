using Application.Features.Categories.Commands.Delete_Category;
using FluentValidation;

namespace Application.Validators.Category
{
    public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
    {
        public DeleteCategoryCommandValidator()
        {
            RuleFor(x => x.id)
                .GreaterThan(0).WithMessage("El identificador de la categoría es requerido.");
        }
    }
}
