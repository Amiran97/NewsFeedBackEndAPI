using BackEnd.Infrastructure.Models.Dtos;
using MediatR;

namespace BackEnd.API.Domains.Post.Queries
{
    public class GetPostsQuery : IRequest<PostsResponse>
    {
        public int Page { get;set; }
    }
}
