using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ProductDto>
    {
        public readonly IProductRepository _productRepository;

        public UpdateProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
           var product = await _productRepository.GetByIdAsync(request.Id);
            if (product == null) return null;

            product.Name = request.Name;
            product.Price = request.Price;
            await _productRepository.UpdateAsync(product);

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price
            };

        }
    }
}
