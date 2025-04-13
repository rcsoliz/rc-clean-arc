using Application.Features.Users.Queries.GetUserById;
using Application.Serivces;
using Application.Validators;
using Core.Entities;
using Core.Models;
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
        private readonly IUserRepository _userRepository;
        public UserController(IMediator mediator, IUserRepository userRepository)
        {
            _mediator = mediator;
            _userRepository = userRepository;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult> Create(UserModel command)
        {
            var validationResult = new UserValidator().Validate(new User { Username = command.Username, Email = command.Email, PasswordHash = command.Password });
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var user = await _userRepository.AddAsync(command);
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
