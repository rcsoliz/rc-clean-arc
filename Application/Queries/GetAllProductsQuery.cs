using Core.Entities;
using MediatR;
using System.Collections.Generic;


namespace Application.Queries
{
    public record GetAllProductsQuery(): IRequest<IEnumerable<Product>>;

}
