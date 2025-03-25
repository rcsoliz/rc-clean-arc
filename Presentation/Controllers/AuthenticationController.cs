using Application.Features.Users.Queries.GetUserById;
using Application.Serivces;
using Application.Validators;
using Core.Entities;
using Core.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Presentation.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : Controller
    {

        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepository;
        private readonly IMediator _mediator;
 
        public AuthenticationController(IConfiguration config, IMediator mediator, IUserRepository userRepository)
        {
            _config = config;
            _mediator = mediator;
            _userRepository = userRepository;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult> Create(UserModel command)
        {
            var validationResult = new UserValidator().Validate(new User { Username = command.Username,Email = command.Email, Password = command.Password });
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var user = await _userRepository.AddAsync(command);// .AddAsync(command);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(int id)
        {

            var user = await _mediator.Send(new GetUserByIdQuery(id));
            if (user == null) return NotFound();

            return Ok(user);
        }


        [HttpPost("logout")]
        public IActionResult Logout()
        {
            return Ok("Sesión cerrada correctamente.");
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var user = await _userRepository.Access(new User { Email = loginRequest.Email, Password = loginRequest.Password });

            if (user == null || string.IsNullOrEmpty(user.Email))
                return Unauthorized("Credenciales incorrectas.");

            string token = GenerateJwtToken(user);
            return Ok(new { Token = token });
        }

        private string GenerateJwtToken(User user)
        {
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email)
            }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
