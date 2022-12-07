using BackEnd.API.Domains.Post.Queries;
using BackEnd.Infrastructure.Context;
using BackEnd.Infrastructure.Models.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
            var postsResult = await context.Posts.Skip((request.Page-1) * 10).Take(10).Include(p=>p.Author).ToListAsync();
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
            var result = new PostsResponse() { 
                Posts = posts, 
                Page = request.Page, 
                TotalCount = totalCount, 
                TotalPages = totalPages};
            return result;
        }
    }
}
