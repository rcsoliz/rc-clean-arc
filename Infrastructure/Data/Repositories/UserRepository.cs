using Application.Interfaces;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context; 

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> Access(User user, CancellationToken cancellationToken = default)
        {
            var userByEmail = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == user.Email, cancellationToken);
            if (userByEmail == null || !userByEmail.VerifyPassword(user.PasswordHash))
                return null;
            return userByEmail;
        }

        public async Task<User> AddAsync(User entity, CancellationToken cancellationToken)
        {
            await _context.Users.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return entity;
        }


        public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        }

        public async Task UpdateAsync(User entity, CancellationToken cancellationToken = default)
        {
            _context.Users.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
