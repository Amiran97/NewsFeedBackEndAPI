using BackEnd.Infrastructure.Models.Dtos;
using MediatR;

namespace BackEnd.API.Domains.Post.Queries
{
    public class GetPostByIdQuery : IRequest<PostResponse>
    {
        public int Id { get; set; }
    }
}
