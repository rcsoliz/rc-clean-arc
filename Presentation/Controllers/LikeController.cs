using Application.DTOs;
using Application.Features.Likes.Commands.CreateLike;
using Application.Features.Likes.Commands.DeleteLike;
using Application.Features.Likes.Commands.UpdateLike;
using Application.Features.Likes.Queries.GetAllLikes;
using Application.Features.Likes.Queries.GetLikesById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LikeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LikeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [EnableRateLimiting("read")]
        public async Task<ActionResult<IEnumerable<LikeDto>>> GetAll() {
            var likes = await _mediator.Send(new GetAllLikesQuery());
            return Ok(likes);
        }
        [HttpGet("{id}")]
        [EnableRateLimiting("read")]
        public async Task<ActionResult<LikeDto>> GetById(int id)
        {
            var like = await _mediator.Send(new GetLikeByIdQuery(id));
            if (like == null) return NotFound();
            return Ok(like);
        }

        [HttpPost]
        [EnableRateLimiting("write")]
        public async Task<ActionResult<LikeDto>> Create(CreateLikeCommand command) {
            var like = await _mediator.Send(command);
            if (like == null) return BadRequest();
            return CreatedAtAction(nameof(GetById), new { id = like.Id }, like);
        }

        [HttpPut("{id}")]
        [EnableRateLimiting("write")]
        public async Task<ActionResult> Update(int id, UpdateLikeCommand command)
        {
            if (id != command.Id)
                return BadRequest("El id de la ruta y el del cuerpo no coinciden.");

            var like = await _mediator.Send(command);
            if (!like) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [EnableRateLimiting("write")]
        public async Task<ActionResult> Delete(int id)
        {
            var like = await _mediator.Send(new DeleteLikeCommand(id));
            if(!like) return NotFound();
            return NoContent();
        }

    }
}
