

using Core.Entities;

namespace Application.Interfaces
{
    public interface IRefreshTokenService
    {
        Task SaveRefreshTokenAsync(User user, string refreshToken, DateTime expiration);
        Task<UserRefreshToken> GetRefreshTokenAsync(string refreshToken);
        Task RevokeRefreshTokenAsync(string refreshToken);
    }
}
