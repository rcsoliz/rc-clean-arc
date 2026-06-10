using Core.Entities;
using Core.Models;

namespace Application.Serivces
{
    public interface IUserRepository
    {
        Task<User> AddAsync(User entity);
        Task<User> Access(User user);

        Task<User> GetByIdAsync(int id);
    }
}
