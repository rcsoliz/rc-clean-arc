using Application.Interfaces;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories.Auth
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly AppDbContext _context;
        public RefreshTokenService(AppDbContext context)
        {
            _context = context;
        }
        public async Task SaveRefreshTokenAsync(User user, string refreshToken, DateTime expiration, CancellationToken cancellationToken = default)
        {
            var token = new UserRefreshToken
            {
                UserId = user.Id,
                RefreshToken = refreshToken,
                ExpirationDate = expiration
            };
            _context.UserRefreshTokens.Add(token);
            await _context.SaveChangesAsync(cancellationToken);  
        }
        public async Task<UserRefreshToken?> GetRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
        {
            return await _context.UserRefreshTokens
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.RefreshToken == refreshToken && x.RevokedAt == null, cancellationToken);
        }

        public async Task RevokeRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
        {
            var token = await _context.UserRefreshTokens
                .FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);
            if (token != null)
            {
                token.RevokedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }


    }
}
