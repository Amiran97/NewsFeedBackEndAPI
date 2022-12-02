using MediatR;

namespace BackEnd.API.Domains.Post.Commands
{
    public class DeletePostCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public string AuthorName { get; set; }
    }
}
