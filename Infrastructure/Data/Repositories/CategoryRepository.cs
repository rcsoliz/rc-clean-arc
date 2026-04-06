using Application.DTOs;
using Application.Interfaces;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace Infrastructure.Data.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Category entity)
        {
            await _context.Categories.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public Task AddAsync(Category entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var category = await _context.Categories.FindAsync(new object[] { id }, cancellationToken);
            if (category is null) return false; 

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var categories = await _context.Categories
                    .Select(c => new Category
                    {
                        Id = c.Id,
                        Name = c.Name
                    }).ToListAsync(cancellationToken);
            return categories;
        }
        public async Task<Category> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var category = await _context.Categories.FindAsync(new object[] { id }, cancellationToken);
            return new Category
            {
                Id = category.Id,
                Name = category.Name
            };
        }

        public async Task UpdateAsync(Category entity, CancellationToken cancellationToken)
        {
            var category = await _context.Categories.FindAsync(new object[] { entity.Id }, cancellationToken);
            if (category != null)
            {
                category.Id = entity.Id;
                category.Name = entity.Name;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
