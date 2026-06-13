
namespace Core.Entities
{
    public class UserRefreshToken: BaseEntity
    {
        public int UserId { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime ExpirationDate { get; set; }
        public User User { get; set; } = null!;
        public DateTime? RevokedAt { get; set; }
        
    }
}
