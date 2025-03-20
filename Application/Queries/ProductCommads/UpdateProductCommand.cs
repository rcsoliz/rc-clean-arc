using Core.Entities;
using MediatR;

namespace Application.Queries.ProductCommads
{
    public record UpdateProductCommand(int Id, string Name, decimal Price): IRequest<bool>;
}
