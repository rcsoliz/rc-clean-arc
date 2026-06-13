using Application.DTOs;
using Application.Features.Posts.Commands.CreatePost;
using Application.Features.Posts.Queries.FiltersPos;
using Application.Features.Posts.Queries.GetAllPost;
using Application.Features.Posts.Queries.GetAllPostByUserId;
using Application.Features.Posts.Queries.GetAllPosts;
using Application.Features.Posts.Queries.GetPostById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PostController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PostController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [EnableRateLimiting("read")]
        public async Task<ActionResult<IEnumerable<PostDto>>> GetAll()
        {
            var posts = await _mediator.Send(new GetAllPostQuery());
            return Ok(posts);
        }

        [HttpGet("{id}")]
        [EnableRateLimiting("read")]
        [AllowAnonymous]
        public async Task<ActionResult<PostDto>> GetById(int id)
        {
            var post = await _mediator.Send(new GetPostByIdQuery(id));
            if (post == null) return NotFound();
            return Ok(post);
        }

        [HttpPost]
        [EnableRateLimiting("write")]
        public async Task<ActionResult<PostDto>> Create(CreatePostCommand command)
        {
            var post = await _mediator.Send(command);
            if (post == null) return NotFound();
            return CreatedAtAction(nameof(GetById), new { id = post.Id }, post);
        }

        [HttpGet("detailed")]
        [EnableRateLimiting("read")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDetailedPosts()
        {
            var posts = await _mediator.Send(new GetAllPostQuery());
            return Ok(posts);
        }

        [HttpGet("paged")]
        [EnableRateLimiting("read")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPagedPosts(int page , int pageSize)
        {
            var posts = await _mediator.Send(new GetPagedPostsAsyncRefactoryQuery(page, pageSize));
            return Ok(posts);
        }

        [HttpGet("user/{id}")]
        [EnableRateLimiting("read")]
        public async Task<IActionResult> GetAllPostByUserId(int id)
        {
            var post = await _mediator.Send(new GetAllPostByUserIdQuery(id));
            if (post == null) return NotFound();
            return Ok(post);
        }


        [HttpPost("filter")]
        [EnableRateLimiting("read")]
        [AllowAnonymous]
        public async Task<ActionResult> FiltersPost([FromBody]  PostFilterDto filter)
        {
            var fileter = await _mediator.Send(new FiltersPostQuery(filter));
            return Ok(fileter);
        }
    }
}
