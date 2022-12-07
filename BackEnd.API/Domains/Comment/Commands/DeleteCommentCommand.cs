using MediatR;

namespace BackEnd.API.Domains.Comment.Commands
{
    public class DeleteCommentCommand : IRequest<Unit>
    {
        public int PostId { get; set; }
        public string AuthorName { get; set; }
    }
}
