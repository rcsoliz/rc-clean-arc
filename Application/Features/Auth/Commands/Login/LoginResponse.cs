
namespace Application.Features.Auth.Commands.Login
{
    public class LoginResponse
    {
        public string JwtToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
