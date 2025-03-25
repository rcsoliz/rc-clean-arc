using Application.DTOs;
using Application.Features.Comments.Commands.CreateComment;
using Application.Features.Comments.Commands.DeleteComment;
using Application.Features.Comments.Queries.GetAllCommentByPostId;
using Application.Features.Comments.Queries.GetAllComments;
using Application.Features.Comments.Queries.GetCommentById;
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
    public class CommentController : Controller
    {
        private readonly IMediator _mediator;

        public CommentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comment>>> GetAll()
        {
            var comments = await _mediator.Send(new GetAllCommentQuery());
            return Ok(comments);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Comment>> GetById(int Id)
        {
            var comment = await _mediator.Send(new GetCommentByIdQuery(Id));
            if (comment == null) return NotFound();
            return Ok(comment);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateCommentCommand command)
        {
            var validationResult = new CommentValidator().Validate(new Comment { CommentContent = command.CommentContent });
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var comment = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = comment.Id }, comment);
        }

        [HttpGet("post/{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetAllCommentByPostId([FromRoute] int id)
        {
            var comments = await _mediator.Send(new GetAllCommentByPostIdQuery(id));

            return Ok(comments);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var comment = await _mediator.Send(new DeleteCommentCommand(id));
            if (!comment) return NotFound();

            return NoContent();
        }

    }
}
