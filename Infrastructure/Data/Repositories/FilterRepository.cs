using Application.Common;
using Application.DTOs;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class FilterRepository : IFilterRepository
    {
        private readonly AppDbContext _context;

        public FilterRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<PagedResult<PostDto>> FiltersPost(PostFilterDto filter)
        {
            var query = _context.Posts
                .Include(p => p.User)
                .Include(p => p.Comments)
                .AsQueryable();

            if (!string.IsNullOrEmpty(filter.SearchText))
            {
                query = query.Where(p => p.PostContent.Contains(filter.SearchText));
            }

            if (!string.IsNullOrEmpty(filter.Username))
            {
                query = query.Where(p => p.User.Username == filter.Username);
            }

            if (filter.StartDate.HasValue)
            {
                query = query.Where(p => p.CreatedAt >= filter.StartDate.Value.Date);
            }

            if (filter.EndDate.HasValue)
            {
                query = query.Where(p => p.CreatedAt < filter.EndDate.Value.Date.AddDays(1));
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(p => new PostDto
                {
                    Id = p.Id,
                    PostContent = p.PostContent,
                    Username = p.User.Username,
                    UserId = p.UserId,
                    Created = p.CreatedAt.ToString("yyyy-MM-dd"),
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
