
namespace Application.DTOs
{
    public class CommentDto
    {
        public int Id { get; set; }

        public string CommentContent { get; set; }
        public string Username { get; set; }
        public int UserId { get; set; }
        public string Created { get; set; }
        public int PostId { get; set; }
        public int? ParentCommentId { get; set; }

    }
}
