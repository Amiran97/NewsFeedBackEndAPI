using BackEnd.API.Domains.Post.Queries;
using BackEnd.API.Context;
using BackEnd.API.Models.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
            if (request.Option == Models.PostFilterOption.Newest || request.Option == Models.PostFilterOption.None)
            {
                postsQuery = postsQuery.OrderByDescending(p => p.CreatedAt);
            } else if (request.Option == Models.PostFilterOption.Popular)
            {
                postsQuery = postsQuery.OrderByDescending(p => p.Likes.Count());
            }
            var postsResult = await postsQuery.Include(p => p.Author).Where(p => p.Author.UserName == request.UserName).Skip((request.Page - 1) * 10).Take(10).Include(p=>p.Comments).Include(p=>p.Likes).Include(p=>p.Tags).ToListAsync();
            var totalCount = postsResult.Count;
            var totalPages = totalCount / 10 + ((totalCount % 10) != 0 ? 1 : 0);
            var posts = new List<PostResponse>();
            postsResult.ForEach(item =>
            {
                var likesResult = new List<string>();
                item.Likes.ToList().ForEach(pl =>
                {
                    likesResult.Add(pl.UserName);
                });
                var tags = new List<string>();
                foreach (var tag in item.Tags)
                {
                    tags.Add(tag.Name);
                }
                posts.Add(new PostResponse
                {
                    Id = item.Id,
                    Title = item.Title,
                    Content = item.Content,
                    AuthorName = item.Author.UserName,
                    CreatedAt = item.CreatedAt,
                    UpdatedAt = item.UpdatedAt,
                    Likes = likesResult,
                    CommentCount = item.Comments.Count,
                    Tags = tags
                });
            });
            var result = new PostsResponse()
            {
                Posts = posts,
                Page = request.Page,
                TotalCount = totalCount,
                TotalPages = totalPages
            };
            return result;
        }
    }
}
