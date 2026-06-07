using Core.Entities;

namespace Application.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken =default);
        Task<Product> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task AddAsync(Product entity, CancellationToken cancellation);
        Task UpdateAsync(Product entity, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
