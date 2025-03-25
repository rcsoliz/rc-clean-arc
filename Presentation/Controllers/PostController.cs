using Application.Interfaces;
using Application.Queries.CommentCommands;
using Application.Queries.PostCommands;
using Application.Serivces;
using Application.Validators;
using Core.Dtos;
using Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PostController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IPostService _postService;
        private readonly IPostRepository _repository;
        public PostController(IMediator mediator, IPostService postService, IPostRepository repository)
        {
            _mediator = mediator;
            _postService = postService;
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetAll()
        {
            var posts = await _mediator.Send(new GetAllPostQuery());
            return Ok(posts);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<PostDto>> GetById(int id)
        {
            var post = await _mediator.Send(new GetPostByIdQuery(id));
            if (post == null) return NotFound();
            return Ok(post);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreatePostCommand command)
        {
            var validationResult = new PostValidator().Validate(new Post { PostContent = command.PostContent });
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var post = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = post.Id }, post);
        }

        [HttpGet("detailed")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDetailedPosts()
        {
            var posts = await _postService.GetAllDetailedPostsAsync();
            return Ok(posts);
        }

        [HttpGet("paged")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPagedPosts(int page , int pageSize)
        {
            var posts = await _repository.GetPagedPostsAsyncRefactory(page, pageSize);
            return Ok(posts);
        }

        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetAllPostByUserId(int id)
        {
            var post = await _mediator.Send(new GetAllPostByUserIdQuery(id));
            if (post == null) return NotFound();
            return Ok(post);
        }


        [HttpPost("filter")]
        [AllowAnonymous]
        public async Task<ActionResult> FiltersPost([FromBody]  PostFilterDto filter)
        {
            var fileter = await _mediator.Send(new FiltersPostQuery(filter));
            return Ok(fileter);
        }
    }
}
