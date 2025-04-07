using Application.DTOs.Auth;
using Application.Interfaces;
using MediatR;
using Microsoft.IdentityModel.Tokens;

namespace Application.Features.Auth.Commands.RefreshToken
{
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenResponse>
    {
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenService _refreshTokenService;

        public RefreshTokenHandler(IJwtService jwtService, IRefreshTokenService refreshTokenService)
        {
            _jwtService = jwtService;
            _refreshTokenService = refreshTokenService;
        }
        public async Task<RefreshTokenResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var tokenInDb = await _refreshTokenService.GetRefreshTokenAsync(request.RefreshToken);
            if (tokenInDb == null || tokenInDb.ExpirationDate < DateTime.UtcNow)
                throw new SecurityTokenException("Token inválido o expirado");

            var user = tokenInDb.User;

            var newAccessToken = _jwtService.GenerateAccessToken(user);
            var newRefreshToken = _jwtService.GenerateRefreshToken();

            await _refreshTokenService.RevokeRefreshTokenAsync(request.RefreshToken);
            await _refreshTokenService.SaveRefreshTokenAsync(user, newRefreshToken, DateTime.UtcNow.AddDays(7));

            return new RefreshTokenResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }
    }
}
