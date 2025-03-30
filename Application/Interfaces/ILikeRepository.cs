using Application.DTOs;
using Core.Entities;

namespace Application.Interfaces
{
    public interface ILikeRepository
    {
        Task<IEnumerable<LikeDto>> GetAll();
        Task<LikeDto> GetLikeByIdAsync(int id);
        Task AddAsync(Like entity);
        Task UpdateAsync(Like entity);
        Task DeleteAsync(int id);
    }
}
