﻿using Application.DTOs;
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
            await using var transaction = await _context.Database.BeginTransactionAsync();
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
        }

        public async Task DeleteAsync(int id, List<int> categoryIds)
        {
           await using var trasnaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var post =await _context.Posts.FindAsync(id);
                if (post != null)
                {
                    var categiries =await _context.PostCategories
                        .Where(pc => pc.PostId == id).ToListAsync();

                    _context.PostCategories.RemoveRange(categiries);
                    _context.Posts.Remove(post);

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

   
        public async Task<PostDto> GetPostByIdtWithCategoriesAsync(int id)
        {
            // PostWithCategoriesDto
            var post = await _context.Posts
                .Include(p => p.User)
                .Include(p => p.PostCategories)
                .ThenInclude(pc => pc.Category)
                .Where(c => c.Id == id)
                .Select(c => new PostDto
                {
                    Id = c.Id,
                    PostContent = c.PostContent,
                    Username = c.User.Username,
                    UserId = c.UserId,
                    Created = c.CreatedAt.ToString(),
                    Categories = c.PostCategories.Select(pc => new PostCategoryDtos
                    {
                        CategoryId = pc.Category.Id,
                        Name = pc.Category.Name
                    }).ToList()
                }).FirstOrDefaultAsync();

            return post;
        }

        public async Task<IEnumerable<PostDto>> GetPostWithCategoryIdAsync(int categoryId)
        {
            var posts = await _context.Posts
                .Include(p => p.User)
                .Include(p => p.PostCategories)
                .ThenInclude(pc => pc.Category)
                .Include(p => p.Likes)
                .Where(p => p.PostCategories.Any(pc => pc.CategoryId == categoryId))
                .Select(p => new PostDto
                {
                    Id = p.Id,
                    PostContent = p.PostContent,
                    Username = p.User.Username,
                    UserId = p.UserId,
                    Created = p.CreatedAt.ToString(),
                    LikeCount = p.Likes.Count,
                    Categories = p.PostCategories.Select(pc => new PostCategoryDtos
                    {
                        CategoryId = pc.Category.Id,
                        Name = pc.Category.Name
                    }).ToList()
                })
                .ToListAsync();

            return posts;
        }

        public async Task<IEnumerable<PostDto>> GetAllPostsWithCategoriesAsync()
        {
            var posts = await _context.Posts
                .Include(p => p.User)
                .Include(p => p.PostCategories)
                .ThenInclude(pc => pc.Category)
                .Include(p => p.Likes)
                .Select(p => new PostDto
                {
                    Id = p.Id,
                    PostContent = p.PostContent,
                    Username = p.User.Username,
                    UserId = p.UserId,
                    Created = p.CreatedAt.ToString(),
                    LikeCount = p.Likes.Count,
                    Categories = p.PostCategories.Select(pc => new PostCategoryDtos
                    {
                        CategoryId = pc.Category.Id,
                        Name = pc.Category.Name
                    }).ToList()
                })
                .ToListAsync();

            return posts;
        }

        public async Task UpdateAsync(Post post, List<int> categoryIds)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Posts.Update(post);

                var existingCategories = await _context.PostCategories
                    .Where(pc => pc.PostId == post.Id)
                    .ToListAsync();

                _context.PostCategories.RemoveRange(existingCategories);

                var newCategories = categoryIds.Select(categoryId => new PostCategory
                {
                    PostId = post.Id,
                    CategoryId = categoryId
                });

                await _context.PostCategories.AddRangeAsync(newCategories);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<IEnumerable<PostWithCategoriesDto>> GetPostsByScrollAsync(DateTime? lastPostDate, int take = 5)
        {
            var query = _context.Posts
                .Include(p => p.User)
                .Include(p => p.PostCategories)
                    .ThenInclude(pc => pc.Category)
                .Include(p => p.Likes)
                .OrderByDescending(p => p.CreatedAt)
                .AsQueryable();

            if (lastPostDate.HasValue)
            {
                query = query.Where(p => p.CreatedAt < lastPostDate);
            }

            var posts = await query
                .Take(take)
                .Select(p => new PostWithCategoriesDto
                {
                    Post = new PostDto
                    {
                        Id = p.Id,
                        PostContent = p.PostContent,
                        Username = p.User.Username,
                        UserId = p.UserId,
                        Created = p.CreatedAt.ToString("s"),
                        LikeCount = p.Likes.Count,
                        CommentCount = p.Comments.Count,
                        Categories = p.PostCategories.Select(pc => new PostCategoryDtos
                        {
                            CategoryId = pc.Category.Id,
                            Name = pc.Category.Name
                        }).ToList()
                    }
                })
                .ToListAsync();

            return posts;
        }

        public async Task<int> CountNewPostsAsync(DateTime afterDate)
        {
            return await _context.Posts
                .Where(p => p.CreatedAt > afterDate)
                .CountAsync();
        }
        public async Task<IEnumerable<PostWithCategoriesDto>> GetNewPostsAfterAsync(DateTime afterDate)
        {
            var posts = await _context.Posts
                .Where(p => p.CreatedAt > afterDate)
                .Include(p => p.User)
                .Include(p => p.PostCategories).ThenInclude(pc => pc.Category)
                .Include(p => p.Likes)
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new PostWithCategoriesDto
                {
                    Post = new PostDto
                    {
                        Id = p.Id,
                        PostContent = p.PostContent,
                        Username = p.User.Username,
                        UserId = p.UserId,
                        Created = p.CreatedAt.ToString("s"),
                        LikeCount = p.Likes.Count,
                        CommentCount = p.Comments.Count,
                        Categories = p.PostCategories.Select(pc => new PostCategoryDtos
                        {
                            CategoryId = pc.Category.Id,
                            Name = pc.Category.Name
                        }).ToList()
                    }
                })
                .ToListAsync();

            return posts;
        }
    
    }
}
