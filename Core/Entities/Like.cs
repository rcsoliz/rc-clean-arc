
namespace Core.Entities
{
    public class Like : BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public int PostId { get; set; }
        public Post Post { get; set; } = null!;
        public int CommentId { get; set; }

  
    }
}
