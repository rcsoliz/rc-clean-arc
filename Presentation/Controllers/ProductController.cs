using Application.Features.Products.Commands.CreateProduct;
using Application.Features.Products.Commands.DeleteProduct;
using Application.Features.Products.Commands.UpdateProduct;
using Application.Features.Products.Queries.GetAllProducts;
using Application.Features.Products.Queries.GetProductById;
using Application.Validators;
using Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IMediator _mediator;
        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }
 
        // Api/Product
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAll()
        {
            var products = await _mediator.Send(new GetAllProductsQuery());
            return Ok(products);
        }

        // Api/Product/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetById(int id)
        {
            
            var product = await _mediator.Send(new GetProductByIdQuery(id));
            if(product == null) return NotFound();

            return Ok(product);
        }


        // Post: Api/Product
        [HttpPost]
        public async Task<ActionResult> Create(CreateProductCommand command)
        {
            var validationResult = new ProductValidator().Validate(new Product { Name = command.Name, Price= command.Price});
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var product = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        // Put: Api/Product/1   
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct(int id, UpdateProductCommand command)
        {
            if(id != command.Id) return BadRequest();   
            
            var update = await _mediator.Send(command);
            if(!update) return NotFound();

            return NoContent();
        }

        // Delete: Api/Product/1
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var delete = await _mediator.Send(new DeleteProductCommand(id));
            if (!delete) return NotFound();

            return NoContent();
        }

    }
}
