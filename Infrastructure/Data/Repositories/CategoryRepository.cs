using Application.Interfaces;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;
        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }
   
        public async Task AddAsync(Category entity, CancellationToken cancellationToken = default)
        {
            await _context.Categories.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
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
            return await _context.Categories
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        public async Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
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
