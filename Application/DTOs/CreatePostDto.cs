
namespace Application.DTOs
{
    public class CreatePostDto
    {
        public string PostContent { get; set; }
        public int UserId { get; set; }
        public string ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<int> CategoryIds { get; set; } = new();
    }
}
