using BackEnd.API.Models.Dtos;
using MediatR;

namespace BackEnd.API.Domains.Post.Commands
{
    public class UpdatePostCommand : IRequest<PostResponse>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Context { get; set; }
        public string AuthorName { get; set; }
        public ICollection<string> Tags { get; set; }
        public IFormFileCollection? Images { get; set; }
    }
}
