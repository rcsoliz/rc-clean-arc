using MediatR;

namespace Application.Features.Auth.Commands.Logout
{
    public class LogoutCommand: IRequest
    {
        public string RefreshToken { get; set; }

        public LogoutCommand(string refreshToken)
        {
            RefreshToken = refreshToken;
        }
    }
}
