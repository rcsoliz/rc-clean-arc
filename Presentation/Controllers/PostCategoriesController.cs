using Application.Features.PostsCategories.Commands.CreatePostCategories;
using Application.Features.PostsCategories.Commands.DeletePostCategories;
using Application.Features.PostsCategories.Commands.UpdatePostCategories;
using Application.Features.PostsCategories.Queries.GetAllPostsWithCategories;
using Application.Features.PostsCategories.Queries.GetAllPostWithCategoryId;
using Application.Features.PostsCategories.Queries.GetByPostWithCategoriesById;
using Application.Features.PostsCategories.Queries.GetPostsByScroll;
using Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PostCategoriesController : Controller
    {
        private readonly IMediator _mediator;

        public PostCategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostCategory>>> GetAll()
        {
            var postCategories = await _mediator.Send(new GetAllPostsWithCategoriesQuery());
            return Ok(postCategories);
        }
        [HttpGet("scroll")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<PostCategory>>> GetAllWithScroll([FromQuery] DateTime? lastPostDate, [FromQuery] int take=5)
        {
            var postCategories = await _mediator.Send(new GetPostsByScrollQuery(lastPostDate, take));
            return Ok(postCategories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PostCategory>> GetById(int id)
        {
            var postCategory = await _mediator.Send(new GetPostByIdWithCategoriesQuery(id));
            if (postCategory == null) return NotFound();
            return Ok(postCategory);
        }

        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<PostCategory>>> GetByCategoryId(int categoryId)
        {
            var postCategories = await _mediator.Send(new GetPostWithCategoryIdQuery(categoryId));
            if (postCategories == null) return NotFound();
            return Ok(postCategories);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreatePostCategoryCommand command)
        {
            var postCategory = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = postCategory.post.Id }, postCategory);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdatePostCategoryCommand command)
        {
            if (id != command.Id) return BadRequest("Id mismatch");
            var postCategory = await _mediator.Send(command);
            if (postCategory.post == null) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id, List<int> categoryIds)
        {
            var postCategory = await _mediator.Send(new DeletePostCategoryCommand(id, categoryIds));
            if (postCategory == null) return NotFound();
            return NoContent();
        }

    }
}
