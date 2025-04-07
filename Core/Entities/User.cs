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
        //public string Password { get;  set; }
        public string PasswordHash { get; set; }
        public void SetPassword(string password)
        {
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
        }
        public bool VerifyPassword(string password)
        {
<<<<<<< HEAD
            return BCrypt.Net.BCrypt.Verify(password, PasswordHash);
=======

            return BCrypt.Net.BCrypt.Verify(password, Password);
>>>>>>> aac9bd8d905a3f8f1f37b9a72a6d8ee7cfd2348e
        }


        public ICollection<Post> Posts { get; set; }
        public ICollection<Comment> Comments { get; set; }

        public ICollection<Like> Likes { get; set; }
<<<<<<< HEAD
        public ICollection<UserRefreshToken> UserRefreshTokens { get; set; }
=======


>>>>>>> aac9bd8d905a3f8f1f37b9a72a6d8ee7cfd2348e
    }
}
