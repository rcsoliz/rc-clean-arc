using Application.Features.Products.Commands.UpdateProduct;
using FluentValidation;

namespace Application.Validators.Products
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El identificador del producto es requerido.");
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre del producto es requerido.")
                .MaximumLength(100).WithMessage("El producto no debe exceder los 100 caracteres.");
            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("El precio debe ser mayor que cero.");
        }
    }
}
