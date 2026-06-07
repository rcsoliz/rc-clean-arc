using Application.DTOs;
using MediatR;

namespace Application.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductCommand : IRequest<ProductDto>
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public decimal Price { get; init; }
    }

}
