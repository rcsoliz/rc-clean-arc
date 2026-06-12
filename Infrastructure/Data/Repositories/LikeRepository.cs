using Application.DTOs;
using Application.Interfaces;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class LikeRepository : ILikeRepository
    {
        private readonly AppDbContext _appDbContext;
        public LikeRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task AddAsync(Like entity, CancellationToken cancellationToken)
        {
            await _appDbContext.Likes.AddAsync(entity,cancellationToken);
            await _appDbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> DeleteAsync(int id , CancellationToken cancellationToken)
        {
            var like = await _appDbContext.Likes.FindAsync(new object[] {id}, cancellationToken);
            if (like == null) return false;
            _appDbContext.Likes.Remove(like);
            await _appDbContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task UpdateAsync(Like entity, CancellationToken cancellationToken)
        {
            _appDbContext.Likes.Update(entity);
            await _appDbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<Like?> GetLikeByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _appDbContext.Likes
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<LikeDto>> GetAll(CancellationToken cancellationToken)
        {
            var likes =await  _appDbContext.Likes
                .Select(l => new LikeDto
                {
                    Id = l.Id,
                    PostId = l.PostId,
                    UserId = l.UserId
                }).ToListAsync(cancellationToken);

            return likes;
        }

  
    }
}
