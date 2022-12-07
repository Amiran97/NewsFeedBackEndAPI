using MediatR;

namespace BackEnd.API.Domains.Like.Commands
{
    public class PostLikeCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public string AuthorName { get; set; }
    }
}
