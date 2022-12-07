using BackEnd.API.Domains.Post.Queries;
using BackEnd.Infrastructure.Context;
using BackEnd.Infrastructure.Models.Dtos;
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
            var postsResult = await context.Posts.Include(p => p.Author).Where(p => p.Author.UserName == request.UserName).Skip((request.Page - 1) * 10).Take(10).ToListAsync();
            var totalCount = postsResult.Count;
            var totalPages = totalCount / 10 + ((totalCount % 10) != 0 ? 1 : 0);

            var likes = await context.PostLikes.Include(pl => pl.Post).Include(pl => pl.Author).ToListAsync();

            var posts = new List<PostResponse>();
            postsResult.ForEach(item =>
            {
                var likesResult = new List<PostLikeResponse>();
                likes.ForEach(pl =>
                {
                    if (pl.Post.Id == item.Id)
                    {
                        likesResult.Add(new PostLikeResponse
                        {
                            AuthorName = pl.Author.UserName
                        });
                    }
                });
                posts.Add(new PostResponse
                {
                    Id = item.Id,
                    Title = item.Title,
                    Content = item.Content,
                    AuthorName = item.Author.UserName,
                    CreatedAt = item.CreatedAt,
                    UpdatedAt = item.UpdatedAt,
                    Likes = likesResult
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
