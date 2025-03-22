namespace Core.Models
{
    public class CommentModel
    {
        public int Id { get; set; }
        public string CommentContent { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public int? ParentCommentId { get; set; }
    }
}
