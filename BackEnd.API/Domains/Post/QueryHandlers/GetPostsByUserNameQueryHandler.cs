using BackEnd.API.Domains.Post.Queries;
using BackEnd.API.Context;
using BackEnd.API.Models.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using BackEnd.API.Utils.Mappers;

namespace BackEnd.API.Domains.Post.QueryHandlers
{
    public class GetPostsByUserNameQueryHandler : IRequestHandler<GetPostsByUserNameQuery, PostsResponse>
    {
        private readonly NewsFeedContext context;

        public GetPostsByUserNameQueryHandler(NewsFeedContext context)
        {
            this.context = context;
        }

        public async Task<PostsResponse> Handle(GetPostsByUserNameQuery request, CancellationToken cancellationToken)
        {
            if (request.Page <= 0)
            {
                return null;
            }
            var postsQuery = context.Posts.AsQueryable();
            switch (request.Option)
            {
                case Models.PostFilterOption.Popular:
                    postsQuery = postsQuery.OrderByDescending(p => p.Likes.Count());
                    break;
                case Models.PostFilterOption.Newest:
                case Models.PostFilterOption.None:
                    postsQuery = postsQuery.OrderByDescending(p => p.CreatedAt);
                    break;
            }
            var postsResult = await postsQuery.Include(p => p.Author).Where(p => p.Author.UserName == request.UserName).Skip((request.Page - 1) * 10).Take(10).Include(p=>p.Comments).Include(p=>p.Likes).Include(p=>p.Tags).ToListAsync();
            var totalCount = postsResult.Count;
            var totalPages = totalCount / 10 + ((totalCount % 10) != 0 ? 1 : 0);
            var result = PostMapper.ToPostsResponse(postsResult, totalCount, totalPages, request.Page);
            return result;
        }
    }
}