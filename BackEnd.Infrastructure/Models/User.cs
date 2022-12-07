using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Infrastructure.Models
{
    public class User : IdentityUser
    {
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Post> PostLikes { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Comment> CommentLikes { get; set; }
    }
}
