using Core.Entities;

namespace Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User> AddAsync(User entity);
        Task<User> Access(User user);

        Task<User> GetByIdAsync(int id);
    }
}
