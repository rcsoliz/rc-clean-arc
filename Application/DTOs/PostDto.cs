
using Core.Entities;

namespace Application.DTOs
{
    public class PostDto
    {
        public int Id { get; set; }

        public string PostContent { get; set; }
        public string Username { get; set; }
        public int UserId { get; set; }
        public string Created { get; set; }
        public int CommentCount { get; set; }

        public int LikeCount { get; set; }
        public List<PostCategoryDtos> Categories { get; set; }
    }
}
