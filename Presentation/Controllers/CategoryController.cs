using Application.DTOs;
using Application.Features.Categories.Commands.CreateCategory;
using Application.Features.Categories.Commands.Delete_Category;
using Application.Features.Categories.Commands.UpdateCategory;
using Application.Features.Categories.Queries.GatAllCategories;
using Application.Features.Categories.Queries.GetCategoryById;
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
    public class CategoryController : Controller
    {
        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll() {
            var categories = await _mediator.Send(new GetAllCategoriesQuery());
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetById(int id)
        {
            var category = await _mediator.Send(new GetCategoryById(id));
            if(category == null) return NotFound();

            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult<Category>> Create(CreateCategoryCommand command)
        {
            var categoryValidator = new CategoryValidator().Validate(new Category { Name = command.Name});
            if (!categoryValidator.IsValid) {
                return BadRequest(categoryValidator.Errors);
            }

            var category = await _mediator.Send(command);
            if (category == null) return BadRequest();

            return CreatedAtAction(nameof(GetById),new {id=  category.Id}, category);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateCategoryCommand command)
        {
            if(id != command.Id) return BadRequest();

            var category = await _mediator.Send(command);
            if (category == null) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id) 
        {
            var category = await _mediator.Send(new DeleteCategoryCommand(id));
            if(!category) return NotFound();

            return NoContent();
        }

    }
}
