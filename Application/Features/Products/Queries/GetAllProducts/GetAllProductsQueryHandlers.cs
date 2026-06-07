using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Products.Queries.GetAllProducts
{
    public class GetAllProductsQueryHandlers : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductDto>>
    {
        private readonly IProductRepository _productRepository;

        public GetAllProductsQueryHandlers(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<IEnumerable<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetAllAsync();
            if (products == null) return null;

            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price
            });
        }
    }
}
