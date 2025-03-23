namespace BlazorAppRc.Models
{
    public class ComentarioDto
    {
        public int Id { get; set; }
        public string CommentContent { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public int PostId { get; set; }
        public int UserId {get; set;}
        public int? ParentCommentId { get; set; }
        public string Created { get; set; } = string.Empty;
    }
}
