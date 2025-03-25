using Core.Entities;
using MediatR;
using System.Collections.Generic;


namespace Application.Features.Products.Queries.GetAllProducts
{
    public record GetAllProductsQuery(): IRequest<IEnumerable<Product>>;

}
