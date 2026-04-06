using Application.DTOs;
using Core.Entities;

namespace Application.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellation = default);
        Task<Category> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task AddAsync(Category entity, CancellationToken cancellationToken = default);
        Task UpdateAsync(Category entity, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id , CancellationToken cancellationToken = default);
    }
}
