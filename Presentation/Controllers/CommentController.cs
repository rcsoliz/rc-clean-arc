using Application.Queries.CommentCommands;
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

        [HttpPost("{id}")]
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

    }
}
