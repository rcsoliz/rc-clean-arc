using Application.DTOs;
using Application.Features.Likes.Commands.CreateLike;
using Application.Features.Likes.Commands.DeleteLike;
using Application.Features.Likes.Commands.UpdateLike;
using Application.Features.Likes.Queries.GeAllLikes;
using Application.Features.Likes.Queries.GetLikesById;
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
    public class LikeController : Controller
    {
        private readonly IMediator _mediator;

        public LikeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LikeDto>>> GetAll() {
            var likes = await _mediator.Send(new GetAllLikesQuery());
            return Ok(likes);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<LikeDto>> GetById(int id)
        {
            var like = await _mediator.Send(new GetLikeByIdQuery(id));
            if (like == null) return NotFound();

            return Ok(like);
        }

        [HttpPost]
        public async Task<ActionResult<Like>> Create(CreateLikeCommand command) {
            var validatorResult = new LikeValidator().Validate(new Like { UserId = command.UserId, PostId = command.PostId });
            if (!validatorResult.IsValid) {
                return BadRequest(validatorResult.Errors);
            }

            var like = await _mediator.Send(command);
            if (like == null) return BadRequest();

            return CreatedAtAction(nameof(GetById), new { id = like.Id }, like);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateLikeCommand command)
        {
            if(id != command.Id) return BadRequest();

            var like = await _mediator.Send(command);
            if (like == null) return NoContent();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var like = await _mediator.Send(new DeleteLikeCommand(id));
            if(!like) return NoContent();

            return NoContent();
        }

    }
}
