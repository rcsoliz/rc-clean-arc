using Application.DTOs;
using Application.Features.Categories.Commands.CreateCategory;
using Application.Features.Categories.Commands.DeleteCategory;
using Application.Features.Categories.Commands.UpdateCategory;
using Application.Features.Categories.Queries.GetAllCategories;
using Application.Features.Categories.Queries.GetCategoryById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [EnableRateLimiting("read")]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll() {
            var categories = await _mediator.Send(new GetAllCategoriesQuery());
            return Ok(categories);
        }

        [HttpGet("{id}")]
        [EnableRateLimiting("read")]
        public async Task<ActionResult<CategoryDto>> GetById(int id)
        {
            var category = await _mediator.Send(new GetCategoryById(id));
            if(category == null) return NotFound();

            return Ok(category);
        }

        [HttpPost]
        [EnableRateLimiting("write")]
        public async Task<ActionResult<CategoryDto>> Create(CreateCategoryCommand command)
        {
            var category = await _mediator.Send(command);
            if (category == null) return BadRequest();

            return CreatedAtAction(nameof(GetById),new {id=  category.Id}, category);
        }

        [HttpPut("{id}")]
        [EnableRateLimiting("write")]
        public async Task<ActionResult> Update(int id, UpdateCategoryCommand command)
        {
            if(id != command.Id) return BadRequest();

            var updated = await _mediator.Send(command);
            if (!updated) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [EnableRateLimiting("write")]
        public async Task<ActionResult> Delete(int id) 
        {
            var category = await _mediator.Send(new DeleteCategoryCommand(id));
            if(!category) return NotFound();

            return NoContent();
        }

    }
}
