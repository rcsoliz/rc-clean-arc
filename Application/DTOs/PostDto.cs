﻿
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
    }
}
