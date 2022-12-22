namespace BackEnd.API.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime CreateAt { get; set; }
        public string AuthorId { get; set; }
        public virtual User Author { get; set; }
        public int PostId { get; set; }
        public virtual Post Post { get; set; }
        public virtual ICollection<User> Likes { get; set; } = new HashSet<User>();
    }
}
