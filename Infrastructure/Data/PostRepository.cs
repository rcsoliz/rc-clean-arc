using Application.Common;
using Application.Interfaces;
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

        public async Task<List<PostDto>> GetPagedPostsAsync(int page, int pageSize)
        {
            return await _context.Posts
                .Include(p => p.User)
                .Include(p => p.Comments)
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new PostDto
                {
                    Id = p.Id,
                    PostContent = p.PostContent,
                    Username = p.User.Username,
                    UserId = p.UserId,
                    Created = p.CreatedAt.ToString(),
                    CommentCount = p.Comments.Count
                })
                .ToListAsync();
        }

        public async Task<PagedResult<PostDto>> GetPagedPostsAsyncRefactory(int page, int pageSize)
        {
            var query = _context.Posts
                .Include(p => p.User)
                .Include(p => p.Comments);

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new PostDto
                {
                    Id = p.Id,
                    PostContent = p.PostContent,
                    Username = p.User.Username,
                    UserId = p.UserId,
                    Created = p.CreatedAt.ToString(),
                    CommentCount = p.Comments.Count
                })
                .ToListAsync();

            return new PagedResult<PostDto>
            {
                Items = items,
                TotalCount = totalCount
            };
        }
            
        public IQueryable<Post> GetQueryableWithUserAndComments()
        {
            return _context.Posts
                .Include(p => p.User)
                .Include(p => p.Comments);
        }

  
    }
}
