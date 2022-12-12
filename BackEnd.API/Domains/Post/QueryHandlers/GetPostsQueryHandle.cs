using BackEnd.API.Domains.Post.Queries;
using BackEnd.API.Context;
using BackEnd.API.Models.Dtos;
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
            var postsQuery = context.Posts.AsQueryable();
            if(request.Option == Models.PostFilterOption.Newest || request.Option == Models.PostFilterOption.None)
            {
                postsQuery = postsQuery.OrderByDescending(p => p.CreatedAt);
            } else if(request.Option == Models.PostFilterOption.Popular)
            {
                postsQuery = postsQuery.OrderByDescending(p => p.Likes.Count());
            }
            var postsResult = await postsQuery.Skip((request.Page-1) * 10).Take(10).Include(p=>p.Author).Include(p=>p.Comments).Include(p=>p.Likes).Include(p=>p.Tags).ToListAsync();
            var posts = new List<PostResponse>();
            postsResult.ForEach(item =>
            {
                var likesResult = new List<string>();
                item.Likes.ToList().ForEach(pl =>
                {
                    likesResult.Add(pl.UserName);
                });
                var tags = new List<string>();
                foreach(var tag in item.Tags)
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
            var result = new PostsResponse() { 
                Posts = posts, 
                Page = request.Page, 
                TotalCount = totalCount, 
                TotalPages = totalPages};
            return result;
        }
    }
}
