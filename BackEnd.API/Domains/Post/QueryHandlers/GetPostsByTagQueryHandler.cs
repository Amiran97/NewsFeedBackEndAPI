using BackEnd.API.Context;
using BackEnd.API.Domains.Post.Queries;
using BackEnd.API.Models.Dtos;
using BackEnd.API.Utils.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.API.Domains.Post.QueryHandlers
{
    public class GetPostsByTagQueryHandler : IRequestHandler<GetPostsByTagQuery, PostsResponse>
    {
        private readonly NewsFeedContext context;

        public GetPostsByTagQueryHandler(NewsFeedContext context)
        {
            this.context = context;
        }

        public async Task<PostsResponse> Handle(GetPostsByTagQuery request, CancellationToken cancellationToken)
        {
            if (request.Page <= 0)
            {
                return null;
            }
            var tag = await context.Tags.Where(t => t.Name == request.Tag)
                .Include(t => t.Posts).ThenInclude(p => p.Author)
                .Include(t => t.Posts).ThenInclude(p => p.Comments)
                .Include(t => t.Posts).ThenInclude(p => p.Likes)
                .FirstOrDefaultAsync();

            var postsQuery = tag.Posts.AsQueryable();
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
            var postsResult = postsQuery.Skip((request.Page - 1) * 10).Take(10).ToList();
            var totalCount = postsResult.Count;
            var totalPages = totalCount / 10 + ((totalCount % 10) != 0 ? 1 : 0);
            var result = PostMapper.ToPostsResponse(postsResult, totalCount, totalPages, request.Page);
            return result;
        }
    }
}