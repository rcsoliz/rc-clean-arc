using Application.DTOs;
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
        public async Task AddAsync(Category entity)
        {
            await _context.Categories.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
           var categories = await _context.Categories
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToListAsync();

           return categories;
        }

        public async Task<CategoryDto> GetByIdAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            };  
        }

        public async Task UpdateAsync(Category entity)
        {
            var category = await _context.Categories.FindAsync(entity.Id);
            if (category != null)
            {
                category.Id = entity.Id;
                category.Name = entity.Name;
                await _context.SaveChangesAsync();
            }

            //_context.Categories.Update(entity);
            //await _context.SaveChangesAsync();
        }
    }
}
