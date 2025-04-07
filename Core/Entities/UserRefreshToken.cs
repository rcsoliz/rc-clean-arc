
namespace Core.Entities
{
    public class UserRefreshToken: BaseEntity
    {
        public int UserId { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime RevokedAt { get; set; }
        public User User { get; set; }
    }
}
