using Application.Features.Categories.Commands.CreateCategory;
using FluentValidation;

namespace Application.Validators.Category
{
    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre de la categoría es requerido.")
                .MaximumLength(100).WithMessage("La categoría no debe exceder los 100 caracteres.");
        }
    }
}
