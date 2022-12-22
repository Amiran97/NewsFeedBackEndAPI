using BackEnd.API.Models.Dtos;
using MediatR;

namespace BackEnd.API.Domains.Comment.Commands
{
    public class CreateCommentCommand : IRequest<CommentResponse>
    {
        public int PostId { get; set; }
        public string Message { get; set; }
        public string AuthorName { get; set; }
    }
}
