using BackEnd.API.Domains.Post.Queries;
using BackEnd.API.Context;
using BackEnd.API.Models.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using BackEnd.API.Utils.Mappers;

namespace BackEnd.API.Domains.Post.QueryHandlers
{
    public class GetPostsQueryHandle : IRequestHandler<GetPostsQuery, PostsResponse>
    {
        private readonly NewsFeedContext context;

        public GetPostsQueryHandle(NewsFeedContext context) {
            this.context = context;
        }

        public async Task<PostsResponse> Handle(GetPostsQuery request, CancellationToken cancellationToken)
        {
            var totalCount = await context.Posts.CountAsync();
            var totalPages = totalCount / 10 + ((totalCount % 10) != 0 ? 1 : 0);
            if(request.Page <= 0)
            {
                return null;
            }
            var postsQuery = context.Posts.AsQueryable();
            switch(request.Option)
            {
                case Models.PostFilterOption.Popular:
                    postsQuery = postsQuery.OrderByDescending(p => p.Likes.Count());
                    break;
                case Models.PostFilterOption.Newest:
                case Models.PostFilterOption.None:
                    postsQuery = postsQuery.OrderByDescending(p => p.CreatedAt);
                    break;
            }
            var postsResult = await postsQuery.Skip((request.Page-1) * 10).Take(10).Include(p=>p.Author).Include(p=>p.Comments).Include(p=>p.Likes).Include(p=>p.Tags).ToListAsync();
            var result = PostMapper.ToPostsResponse(postsResult, totalCount, totalPages, request.Page);
            return result;
        }
    }
}