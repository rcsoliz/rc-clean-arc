using Core.Dtos;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Data
{
    public class PostRepository : IPostRepository
    {
        private readonly AppDbContext _context;

        public PostRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Post entity)
        {
            await _context.Posts.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Post>> GetAllAsync()
        {
            return await _context.Posts
                .ToListAsync();
        }

        public async Task<Post> GetByIdAsync(int id)
        {
            return await _context.Posts.FindAsync(id);
        }

        public async Task<IEnumerable<PostDto>> GetAllPostWithDetailsAsync()
        {
            return await _context.PostDtos
                .FromSqlRaw(SqlCommandBuilder.Exec(StoredProcedures.GetAllPosts))
                .ToListAsync();
        }


    }
}
