using Core.Entities;
using FluentValidation;

namespace Application.Validators
{
    public class ProductValidator: AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("El nombre del producto es obligatorio");
            RuleFor(p => p.Price)
                .GreaterThan(0).WithMessage("El precio del producto debe ser mayor a 0");
        }
    }
}
