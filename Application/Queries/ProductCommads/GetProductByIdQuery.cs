using Core.Entities;
using MediatR;

namespace Application.Queries.ProductCommads
{
    public record GetProductByIdQuery(int Id) : IRequest<Product>;

}
