using Application.Features.Products.Commands.CreateProduct;
using FluentValidation;

namespace Application.Validators.Products
{
    public class CreateProductCommandValidator: AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre del producto es requerido.")
                .MaximumLength(100).WithMessage("El producto no debe execeder los 100 caracteres.");
            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("El precio debe ser mayor que zero.");
        }
    }
}
