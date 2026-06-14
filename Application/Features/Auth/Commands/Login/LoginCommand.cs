using MediatR;
using Application.DTOs.Auth;
namespace Application.Features.Auth.Commands.Login
{
    public class LoginCommand: IRequest<LoginResponse?>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

}
