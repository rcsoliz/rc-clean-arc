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

        public async Task AddAsync(Like entity)
        {
            await _appDbContext.Likes.AddAsync(entity);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var like = await _appDbContext.Likes.FindAsync(id);
            if(like != null)
            {
               _appDbContext.Likes.Remove(like);
               await _appDbContext.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(Like entity)
        {
            _appDbContext.Likes.Update(entity);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<LikeDto> GetLikeByIdAsync(int id)
        {
            var like  = await _appDbContext.Likes.FindAsync(id);
            return new LikeDto
            {
                Id = like.Id,
                PostId = like.PostId,
                UserId = like.UserId
            };
        }

        public async Task<IEnumerable<LikeDto>> GetAll()
        {
            var likes =await  _appDbContext.Likes
                .Select(l => new LikeDto
                {
                    Id = l.Id,
                    PostId = l.PostId,
                    UserId = l.UserId
                }).ToListAsync();

            return likes;
        }

  
    }
}
