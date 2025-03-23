namespace BlazorAppRc.Models
{
    public class PostDto
    {
        public int Id { get; set; }
        public string PostContent { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string Created { get; set; } = string.Empty;
        public int CommentCount { get; set; }
    }
}
