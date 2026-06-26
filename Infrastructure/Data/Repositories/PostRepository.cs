using Application.Common;
using Application.DTOs;
using Application.Interfaces;
using Core.Entities;
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
        public async Task AddAsync(Post entity, CancellationToken cancellationToken)
        {
            await _context.Posts.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<Post>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Posts
                .Include(p => p.User)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<PostDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Posts
                .Where(c => c.Id == id)
                .Select(c => new PostDto
                {
                    Id = c.Id,
                    ImageUrl = c.ImageUrl,
                    PostContent = c.PostContent,
                    Username = c.User.Username,
                    UserId = c.UserId,
                    Created = c.CreatedAt.ToString()
                }).FirstOrDefaultAsync(cancellationToken); ;
        }

        public async Task<IEnumerable<PostDto>> GetAllPostWithDetailsAsync(CancellationToken cancellationToken)
            {
                return await _context.Posts
                    .Include(p => p.User)
                    .Include(p => p.Comments)
                    .Include(p => p.Likes)
                    .Include(p => p.PostCategories)
                        .ThenInclude(pc => pc.Category)
                    .OrderByDescending(p => p.CreatedAt)
                    .Select(p => new PostDto
                    {
                        Id = p.Id,
                        PostContent = p.PostContent,
                        ImageUrl = p.ImageUrl,
                        Username = p.User.Username,
                        UserId = p.UserId,
                        Created = p.CreatedAt.ToString("s"),
                        CommentCount = p.Comments.Count,
                        LikeCount = p.Likes.Count,
                        Categories = p.PostCategories
                            .Select(pc => new PostCategoryDto
                            {
                                CategoryId = pc.CategoryId,
                                Name = pc.Category.Name
                            })
                            .ToList()
                    })
                    .ToListAsync(cancellationToken);
            }

        public async Task<List<PostDto>> GetPagedPostsAsync(int page, int pageSize, CancellationToken cancellationToken)
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
                .ToListAsync(cancellationToken);
        }

        public async Task<PagedResult<PostDto>> GetPagedPostsAsyncRefactory(int page, int pageSize, CancellationToken cancellationToken)
        {
            var query = _context.Posts
                .Include(p => p.User)
                .Include(p => p.Comments)
                .Include(p => p.Likes)
                .Include(p => p.PostCategories)
                    .ThenInclude(pc => pc.Category);

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new PostDto
                {
                    Id = p.Id,
                    PostContent = p.PostContent,
                    ImageUrl = p.ImageUrl,
                    Username = p.User.Username,
                    UserId = p.UserId,
                    Created = p.CreatedAt.ToString("s"),
                    CommentCount = p.Comments.Count,
                    LikeCount = p.Likes.Count,
                    Categories = p.PostCategories
                        .Select(pc => new PostCategoryDto
                        {
                            CategoryId = pc.CategoryId,
                            Name = pc.Category.Name
                        })
                        .ToList()
                })
                .ToListAsync(cancellationToken);

            return new PagedResult<PostDto>
            {
                Items = items,
                TotalCount = totalCount
            };
        }
        public IQueryable<Post> GetQueryableWithUserAndComments(CancellationToken cancellationToken)
        {
            return _context.Posts
                .Include(p => p.User)
                .Include(p => p.Comments);
        }

        public async Task<PagedResult<PostDto>> GetAllPostByUserId(int id, CancellationToken cancellationToken)
        {
            var query = _context.Posts
                 .Where(p => p.UserId == id)
                 .Include(p => p.User)
                 .Include(p => p.Comments)
                 .Include(p => p.Likes)
                 .Include(p => p.PostCategories)
                     .ThenInclude(pc => pc.Category);

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new PostDto
                {
                    Id = p.Id,
                    PostContent = p.PostContent,
                    ImageUrl = p.ImageUrl,
                    Username = p.User.Username,
                    UserId = p.UserId,
                    Created = p.CreatedAt.ToString("s"),
                    CommentCount = p.Comments.Count,
                    LikeCount = p.Likes.Count,
                    Categories = p.PostCategories
                        .Select(pc => new PostCategoryDto
                        {
                            CategoryId = pc.CategoryId,
                            Name = pc.Category.Name
                        })
                        .ToList()
                })
                .ToListAsync(cancellationToken);

            return new PagedResult<PostDto>
            {
                Items = items,
                TotalCount = totalCount
            };
        }

    }
}
