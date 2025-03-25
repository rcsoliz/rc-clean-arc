﻿using Application.Interfaces;
using Application.Queries.ProductCommads;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers.ProductHandlers
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, bool>
    {
        public readonly IProductRepository _productRepository;

        public UpdateProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
           var product = await _productRepository.GetByIdAsync(request.Id);
            if (product == null) return false;

            product.Name = request.Name;
            product.Price = request.Price;
            await _productRepository.UpdateAsync(product);
            return true;
        }
    }
}
