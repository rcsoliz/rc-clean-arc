using Application.DTOs;
using Application.Features.PostsCategories.Commands.CreatePostCategories;
using Application.Features.PostsCategories.Commands.DeletePostCategories;
using Application.Features.PostsCategories.Commands.UpdatePostCategories;
using Application.Features.PostsCategories.Queries.CountNewPostsAsync;
using Application.Features.PostsCategories.Queries.GetAllPostsWithCategories;
using Application.Features.PostsCategories.Queries.GetAllPostWithCategoryId;
using Application.Features.PostsCategories.Queries.GetByPostWithCategoriesById;
using Application.Features.PostsCategories.Queries.GetNewPostsAfterAsync;
using Application.Features.PostsCategories.Queries.GetPostsByScroll;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PostCategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PostCategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [EnableRateLimiting("read")]
        public async Task<ActionResult<IEnumerable<PostCategoryDto>>> GetAll()
        {
            var postCategories = await _mediator.Send(new GetAllPostsWithCategoriesQuery());
            return Ok(postCategories);
        }
        [HttpGet("scroll")]
        [EnableRateLimiting("read")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<PostCategoryDto>>> GetAllWithScroll([FromQuery] DateTime? lastPostDate, [FromQuery] int take = 5)
        {
            var postCategories = await _mediator.Send(new GetPostsByScrollQuery(lastPostDate, take));
            return Ok(postCategories);
        }

        [HttpGet("{id}")]
        [EnableRateLimiting("read")]
        public async Task<ActionResult<PostWithCategoriesDto>> GetById(int id)
        {
            var postCategory = await _mediator.Send(new GetPostByIdWithCategoriesQuery(id));
            if (postCategory == null) return NotFound();
            return Ok(postCategory);
        }

        [HttpGet("category/{categoryId}")]
        [EnableRateLimiting("read")]
        public async Task<ActionResult<IEnumerable<PostCategoryDto>>> GetByCategoryId(int categoryId)
        {
            var postCategories = await _mediator.Send(new GetPostWithCategoryIdQuery(categoryId));
            if (postCategories == null) return NotFound();
            return Ok(postCategories);
        }

        [HttpPost]
        [EnableRateLimiting("write")]
        public async Task<ActionResult> Create([FromBody] CreatePostCategoryCommand command)
        {
            var post = await _mediator.Send(command);
            if (post == null) return BadRequest();
            return CreatedAtAction(nameof(GetById), new { id = post.Id }, post);
        }

        [HttpPut("{id}")]
        [EnableRateLimiting("write")]
        public async Task<ActionResult> Update(int id, UpdatePostCategoryCommand command)
        {
            if (id != command.Id) return BadRequest("Id mismatch");
            var update = await _mediator.Send(command);
            if (!update) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [EnableRateLimiting("write")]
        public async Task<ActionResult> Delete(int id, List<int> categoryIds)
        {
            var deleted = await _mediator.Send(new DeletePostCategoryCommand(id, categoryIds));
            if (!deleted) return NotFound();
            return NoContent();
        }

        [HttpGet("newPosts")]
        [EnableRateLimiting("read")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<PostCategoryDto>>> GetNewPosts([FromQuery] DateTime afterDate)
        {
            var postCategories = await _mediator.Send(new GetNewPostsAfterAsyncQuery(afterDate));
            if (postCategories == null) return NotFound();
            return Ok(postCategories);
        }

        [HttpGet("countNewPosts")]
        [EnableRateLimiting("read")]
        [AllowAnonymous]
        public async Task<ActionResult<int>> CountNewPosts([FromQuery] DateTime afterDate)
        {
            var count = await _mediator.Send(new CountNewPostsAsyncQuery(afterDate));
            return Ok(count);

        }
    }
}
