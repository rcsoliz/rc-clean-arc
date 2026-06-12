using Application.DTOs;
using Core.Entities;

namespace Application.Interfaces
{
    public interface ILikeRepository
    {
        Task<IEnumerable<LikeDto>> GetAll(CancellationToken cancellationToken = default);
        Task<Like?> GetLikeByIdAsync(int id, CancellationToken cancellationToken = default);
        Task AddAsync(Like entity, CancellationToken cancellationToken = default);
        Task UpdateAsync(Like entity, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken= default);
    }
}
