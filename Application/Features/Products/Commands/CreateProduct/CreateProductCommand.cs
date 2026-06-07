using Application.DTOs;
using MediatR;


namespace Application.Features.Products.Commands.CreateProduct
{
    public record  CreateProductCommand(string Name, decimal Price): IRequest<ProductDto>;

}
