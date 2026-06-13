using Application.DTOs;
using Application.Features.Comments.Commands.CreateComment;
using Application.Features.Comments.Commands.DeleteComment;
using Application.Features.Comments.Queries.GetAllCommentByPostId;
using Application.Features.Comments.Queries.GetAllComments;
using Application.Features.Comments.Queries.GetCommentById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CommentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [EnableRateLimiting("read")]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetAll()
        {
            var comments = await _mediator.Send(new GetAllCommentQuery());
            return Ok(comments);
        }

        [HttpGet("{id}")]
        [EnableRateLimiting("read")]
        public async Task<ActionResult<CommentDto>> GetById(int Id)
        {
            var comment = await _mediator.Send(new GetCommentByIdQuery(Id));
            if (comment == null) return NotFound();
            return Ok(comment);
        }

        [HttpPost]
        [EnableRateLimiting("write")]
        public async Task<ActionResult<CommentDto>> Create(CreateCommentCommand command)
        {
            var comment = await _mediator.Send(command);
            if (comment == null) return BadRequest();
            return CreatedAtAction(nameof(GetById), new { id = comment.Id }, comment);
        }

        [HttpGet("post/{id}")]
        [EnableRateLimiting("read")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetAllCommentByPostId([FromRoute] int id)
        {
            var comments = await _mediator.Send(new GetAllCommentByPostIdQuery(id));

            return Ok(comments);
        }

        [HttpDelete("{id}")]
        [EnableRateLimiting("write")]
        public async Task<ActionResult> Delete(int id)
        {
            var comment = await _mediator.Send(new DeleteCommentCommand(id));
            if (!comment) return NotFound();

            return NoContent();
        }

    }
}
