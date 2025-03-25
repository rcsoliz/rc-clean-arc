using Core.Entities;
using MediatR;

namespace Application.Features.Products.Queries.GetProductById
{
    public record GetProductByIdQuery(int Id) : IRequest<Product>;

}
