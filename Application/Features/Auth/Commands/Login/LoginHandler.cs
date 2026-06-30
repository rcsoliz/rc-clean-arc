using Application.DTOs.Auth;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Auth.Commands.Login
{
    public class LoginHandler : IRequestHandler<LoginCommand, LoginResponse?>
    {
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IUserRepository _userRepository;
        public LoginHandler(IJwtService jwtService, IRefreshTokenService refreshTokenService, IUserRepository userRepository )
        {
            _jwtService = jwtService;
            _refreshTokenService = refreshTokenService;
            _userRepository = userRepository;
        }

        public async Task<LoginResponse?> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.Access(new Core.Entities.User 
            { 
                Email = request.Email, 
                PasswordHash = request.Password 
            }, cancellationToken);
            if (user == null) return null;
            

            // 1. Generar access token
            var jwtToken = _jwtService.GenerateAccessToken(user);

            // 2. Generar refresh token
            var refreshToken = _jwtService.GenerateRefreshToken();

            // 3. Guardar en BD
            var expiration = DateTime.UtcNow.AddDays(1);
            await _refreshTokenService.SaveRefreshTokenAsync(user, refreshToken, expiration, cancellationToken);

            // 4. Devolver tokens
            return new LoginResponse
            {
                AccessToken = jwtToken,
                RefreshToken = refreshToken,
                Username = user.Username,
                Email = user.Email
            };
        }
    }
    
}
