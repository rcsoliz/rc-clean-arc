using Core.Entities;

namespace Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User> AddAsync(User entity, CancellationToken cancellationToken =default);
        Task<User?> Access(User? user, CancellationToken cancellationToken = default);

        Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    }
}
