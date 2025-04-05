using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Post: BaseEntity
    {


        public string PostContent { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public string ImageUrl { get; set; }
        public ICollection<Comment> Comments { get; set; }

        public ICollection<PostCategory> PostCategories { get; set; }

        public ICollection<Like> Likes { get; set; }

    }
}
