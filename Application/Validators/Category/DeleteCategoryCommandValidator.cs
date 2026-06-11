using Application.Features.Categories.Commands.DeleteCategory;
using FluentValidation;

namespace Application.Validators.Category
{
    public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
    {
        public DeleteCategoryCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El identificador de la categoría es requerido.");
        }
    }
}
