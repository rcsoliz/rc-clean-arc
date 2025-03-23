namespace BlazorAppRc.Models
{
    public class CommentModel
    {
        public string CommentContent { get; set; } = string.Empty;
        public int PostId { get; set; }
        public int UserId { get; set; }
        public int? ParentCommentId { get; set; }

    }
}
