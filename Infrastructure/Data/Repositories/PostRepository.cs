using Application.Common;
using Application.Common.Settings;
using Application.DTOs;
using Application.Interfaces;
using Azure;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
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

        public async Task<PostDto> GetByIdAsync(int id)
        {
            // return await _context.Posts.FindAsync(id);

            return await _context.Posts
                .Where(c => c.Id == id)
                .Select(c => new PostDto
                {
                    Id = c.Id,
                    PostContent = c.PostContent,
                    Username = c.User.Username,
                    UserId = c.UserId,
                    Created = c.CreatedAt.ToString()
                }).FirstOrDefaultAsync(); ;
        }

        public async Task<IEnumerable<PostDto>> GetAllPostWithDetailsAsync()
        {
            return await _context.PostDtos
                .FromSqlRaw(SqlCommandBuilderSp.Exec(StoredProcedures.GetAllPosts))
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

        public async Task<PagedResult<PostDto>> GetAllPostByUserId(int id)
        {
            var query = _context.Posts
                 .Where(p => p.UserId == id)
                 .Include(p => p.User)
                 .Include(p => p.Comments);

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(p => p.CreatedAt)
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


    }
}
