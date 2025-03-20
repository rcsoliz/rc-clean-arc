using Core.Entities;
using MediatR;


namespace Application.Queries.ProductCommads
{
    public record  CreateProductCommand(string Name, decimal Price): IRequest<Product>;

}
