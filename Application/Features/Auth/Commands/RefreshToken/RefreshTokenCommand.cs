using Application.DTOs.Auth;
using MediatR;

namespace Application.Features.Auth.Commands.RefreshToken
{
    public record RefreshTokenCommand(string RefreshToken) : IRequest<RefreshTokenResponse?>;
}
