

using Core.Entities;

namespace Application.Interfaces
{
    public interface IRefreshTokenService
    {
        Task SaveRefreshTokenAsync(User user, string refreshToken, DateTime expiration, CancellationToken cancellationToken= default);
        Task<UserRefreshToken?> GetRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
        Task RevokeRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
    }
}
