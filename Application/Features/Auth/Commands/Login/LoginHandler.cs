using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Application.Features.Auth.Commands.Login
{
    public class LoginHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IApplicationDbContext _context;
        public LoginHandler(IJwtService jwtService, IRefreshTokenService refreshTokenService, IApplicationDbContext context)
        {
            _jwtService = jwtService;
            _refreshTokenService = refreshTokenService;
            _context = context;
        }

        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);


            if (user == null || !user.VerifyPassword(request.Password))
            {
                throw new UnauthorizedAccessException("Credenciales inválidas");
            }

            // 1. Generar access token
            var jwtToken = _jwtService.GenerateAccessToken(user);

            // 2. Generar refresh token
            var refreshToken = _jwtService.GenerateRefreshToken();

            // 3. Guardar en BD
            var expiration = DateTime.UtcNow.AddDays(7);
            await _refreshTokenService.SaveRefreshTokenAsync(user, refreshToken, expiration);

            // 4. Devolver tokens
            return new LoginResponse
            {
                JwtToken = jwtToken,
                RefreshToken = refreshToken
            };
        }
    }
    
}
