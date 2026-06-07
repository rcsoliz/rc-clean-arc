using Application.DTOs;
using MediatR;


namespace Application.Features.Products.Queries.GetAllProducts
{
    public record GetAllProductsQuery(): IRequest<IEnumerable<ProductDto>>;

}
