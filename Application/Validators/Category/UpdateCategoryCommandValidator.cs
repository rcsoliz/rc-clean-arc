using Application.Features.Categories.Commands.UpdateCategory;
using FluentValidation;

namespace Application.Validators.Category
{
    public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El identificador de la categoría es requerido.");
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre de la categoría es requerido.")
                .MaximumLength(100).WithMessage("La categoría no debe exceder los 100 caracteres.");
        }
    }
}
