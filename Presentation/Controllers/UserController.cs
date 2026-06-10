using Application.Features.Users.Commands.CreateUser;
using Application.Features.Users.Queries.GetUserById;
using Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult> Create(CreateUserCommand command)
        {
            var user = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(int id)
        {

            var user = await _mediator.Send(new GetUserByIdQuery(id));
            if (user == null) return NotFound();

            return Ok(user);
        }


    }
}
