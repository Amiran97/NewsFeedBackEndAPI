using BackEnd.API.Models.Dtos;
using MediatR;

namespace BackEnd.API.Domains.Comment.Queries
{
    public class GetCommentsByPostIdQuery : IRequest<IEnumerable<CommentResponse>>
    {
        public int PostId { get; set; }
    }
}
