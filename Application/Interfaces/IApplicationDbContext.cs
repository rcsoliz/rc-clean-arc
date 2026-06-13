using Core.Entities;
using Microsoft.EntityFrameworkCore;
namespace Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Post> Posts { get; set; }
        DbSet<Comment> Comments { get; set; }
        DbSet<Like> Likes { get; set; }
        DbSet<UserRefreshToken> UserRefreshTokens { get; set; }
    }
}
