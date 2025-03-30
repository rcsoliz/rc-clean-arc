using BCrypt.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class User: BaseEntity
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get;  set; }

        public void SetPassword(string password)
        {
            Password = BCrypt.Net.BCrypt.HashPassword(password);
        }
        public bool VerifyPassword(string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, Password);
        }

        public ICollection<Post> Posts { get; set; }
        public ICollection<Comment> Comments { get; set; }

        public ICollection<Like> Likes { get; set; }
    }
}
