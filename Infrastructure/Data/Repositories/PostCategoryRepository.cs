using Application.DTOs;
using Application.Interfaces;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class PostCategoryRepository : IPostCategoryRepository
    {
        private readonly AppDbContext _context;

        public PostCategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Post entity, List<int> categoryIds)
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
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
            await _context.Posts.AddAsync(entity);
            await _context.SaveChangesAsync();
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

        public Task<(PostDto Post, IEnumerable<PostCategoryDtos> Categories)> GetByIDAsync(int id)
        {
            throw new NotImplementedException();
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
    }
}
