using Application.Interfaces;
using MediatR;

namespace Application.Features.Auth.Commands.Logout
{
    public class LogoutHandler : IRequestHandler<LogoutCommand>
    {
        private readonly IRefreshTokenService _refreshTokenService;

        public LogoutHandler(IRefreshTokenService refreshTokenService)
        {
            _refreshTokenService = refreshTokenService;
        }

        public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            await _refreshTokenService.RevokeRefreshTokenAsync(request.RefreshToken);
            return Unit.Value;
        }
    }
}
