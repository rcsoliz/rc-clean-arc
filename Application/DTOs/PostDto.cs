namespace Application.DTOs
{
    public class PostDto
    {
        public int Id { get; set; }

        public string PostContent { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string Created { get; set; } = string.Empty;
        public int CommentCount { get; set; }

        public int LikeCount { get; set; }
        public List<PostCategoryDto> Categories { get; set; } = new();
        
    }
}
