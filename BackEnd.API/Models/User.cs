using Microsoft.AspNetCore.Identity;

namespace BackEnd.API.Models
{
    public class User : IdentityUser
    {
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
        public virtual ICollection<Post> PostLikes { get; set; } = new HashSet<Post>();
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public virtual ICollection<Comment> CommentLikes { get; set; } = new HashSet<Comment>();
    }
}
