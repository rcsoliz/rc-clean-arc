using Core.Entities;
using MediatR;

namespace Application.Features.Products.Commands.UpdateProduct
{
    public record UpdateProductCommand(int Id, string Name, decimal Price): IRequest<bool>;
}
