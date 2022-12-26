using BackEnd.API.Models.Dtos;
using MediatR;

namespace BackEnd.API.Domains.Like.Commands
{
    public class CommentDislikeCommand : IRequest<CommentResponse>
    {
        public int Id { get; set; }
        public string AuthorName { get; set; }
    }
}
