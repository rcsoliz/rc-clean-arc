using Core.Entities;
using Core.Models;

namespace Core.Interfaces
{
    public interface IUserRepository
    {
        Task<User> AddAsync(UserModel entity);
        Task<User> Access(User user);

        Task<User> GetByIdAsync(int id);
    }
}
