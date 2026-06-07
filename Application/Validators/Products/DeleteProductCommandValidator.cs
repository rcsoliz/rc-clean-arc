using Application.Features.Products.Commands.DeleteProduct;
using FluentValidation;

namespace Application.Validators.Products
{
    public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El identificador del producto es requerido.");
        }
    }
}
