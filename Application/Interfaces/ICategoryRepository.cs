using Application.DTOs;
using Core.Entities;

namespace Application.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<CategoryDto>> GetAllAsync();
        Task<CategoryDto> GetByIdAsync(int id);
        Task AddAsync(Category entity);
        Task UpdateAsync(Category entity);
        Task DeleteAsync(int id);
    }
}
