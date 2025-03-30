using Application.Common;
using Application.Common.Settings;
using Application.DTOs;
using Application.Interfaces;
using Azure;
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
        public async Task AddAsync(Post entity)
        {
            await _context.Posts.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task AddAsyncRefactory(Post entity, List<int> categoryIds)
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.Posts.AddAsync(entity);
                await _context.SaveChangesAsync();

                foreach (var categoryId in categoryIds)
                {
                    var postCategory = new PostCategory
                    {
                        PostId = entity.Id,
                        CategoryId = categoryId
                    };
                    await _context.PostCategories.AddAsync(postCategory);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch(Exception)
            {
                await transaction.RollbackAsync();
                throw;
            } 
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
            return await _context.Posts
                .Include(p => p.User)
                .Include(p => p.Comments)
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
            //return await _context.PostDtos
            //    .FromSqlRaw(SqlCommandBuilderSp.Exec(StoredProcedures.GetAllPosts))
            //    .ToListAsync();
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

        public async Task UpdateAsync(Post post, List<int> categoryIds)
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Posts.Update(post);

                var existingCategories = await _context.PostCategories
                    .Where(pc => pc.PostId == post.Id).ToListAsync();

                _context.PostCategories.RemoveRange(existingCategories);

                foreach (var categoryId in categoryIds)
                {
                    var postCategory = new PostCategory
                    {
                        PostId = post.Id,
                        CategoryId = categoryId
                    };
                    await _context.PostCategories.AddAsync(postCategory);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task DeleteAsync(int id, List<int> categoryIds)
        {
            var trasnaction = await _context.Database.BeginTransactionAsync(); 
            try
            {
                var post = _context.Posts.Find(id);
                if (post != null)
                {
                    var categiries = _context.PostCategories
                        .Where(pc => pc.PostId == id).ToList();
                    _context.PostCategories.RemoveRange(categiries);

                    _context.Posts.Remove(post);
                    await _context.SaveChangesAsync();

                    await _context.SaveChangesAsync();
                    await trasnaction.CommitAsync();
                }
            }
            catch (Exception)
            {
                await trasnaction.RollbackAsync();
                throw;
            }
        }
    }
}
