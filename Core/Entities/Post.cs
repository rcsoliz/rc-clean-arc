namespace Core.Entities
{
    public class Post: BaseEntity
    {


        public string PostContent { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public string? ImageUrl { get; set; }
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public ICollection<PostCategory> PostCategories { get; set; } = new List<PostCategory>();

        public ICollection<Like> Likes { get; set; } = new List<Like>();

    }
}
