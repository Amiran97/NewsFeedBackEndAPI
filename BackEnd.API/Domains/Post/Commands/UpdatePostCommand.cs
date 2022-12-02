using MediatR;

namespace BackEnd.API.Domains.Post.Commands
{
    public class UpdatePostCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Context { get; set; }
        public string AuthorName { get; set; }
    }
}
