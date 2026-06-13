using Application.DTOs;
using Application.Features.Users.Commands.CreateUser;
using Application.Features.Users.Queries.GetUserById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        [EnableRateLimiting("register")]
        public async Task<ActionResult<UserDto>> Register(CreateUserCommand command)
        {
            var user = await _mediator.Send(command);
            if (user == null) return BadRequest();
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }


        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetById(int id)
        {
            var user = await _mediator.Send(new GetUserByIdQuery(id));
            if (user == null) return NotFound();
            return Ok(user);
        }


    }
}
