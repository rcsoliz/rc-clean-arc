﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Comment: BaseEntity
    {
        public string CommentContent { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }



        public User User { get; set; }
        public Post Post { get; set; }

        public int? ParentCommentId { get; set; }
        public Comment ParentComment { get; set; }  // Comentario padre
        public ICollection<Comment> Replies { get; set; } = new List<Comment>();

    }
}
