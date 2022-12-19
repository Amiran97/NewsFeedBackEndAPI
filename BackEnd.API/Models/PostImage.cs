namespace BackEnd.API.Models
{
    public class PostImage
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public int PostId { get; set; }
        public virtual Post Post { get; set; }
    }
}
