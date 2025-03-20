using MediatR;


namespace Application.Queries.ProductCommads
{
    public record DeleteProductCommand(int Id) : IRequest<bool>;

}
