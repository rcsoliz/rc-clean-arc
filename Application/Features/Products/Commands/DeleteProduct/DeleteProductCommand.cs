﻿using MediatR;


namespace Application.Features.Products.Commands.DeleteProduct
{
    public record DeleteProductCommand(int Id) : IRequest<bool>;

}
