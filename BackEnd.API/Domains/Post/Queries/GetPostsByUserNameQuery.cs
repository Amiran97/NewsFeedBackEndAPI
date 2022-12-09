using BackEnd.API.Models.Dtos;
using MediatR;

namespace BackEnd.API.Domains.Post.Queries
{
    public class GetPostsByUserNameQuery : IRequest<PostsResponse>
    {
        public string UserName { get; set; }
        public int Page { get; set; }
    }
}
