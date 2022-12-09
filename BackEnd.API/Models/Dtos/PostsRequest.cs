namespace BackEnd.API.Models.Dtos
{
    public class PostsRequest
    {
        public int Page { get; set; } = 1;
        public string? UserName { get; set; }
        public string? Tag { get; set; }
    }
}
