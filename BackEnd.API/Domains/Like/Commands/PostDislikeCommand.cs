using BackEnd.API.Models.Dtos;
using MediatR;

namespace BackEnd.API.Domains.Like.Commands
{
    public class PostDislikeCommand : IRequest<PostResponse>
    {
        public int Id { get; set; }
        public string AuthorName { get; set; }
    }
}
