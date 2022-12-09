using BackEnd.API.Domains.Post.Queries;
using BackEnd.API.Context;
using BackEnd.API.Models.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.API.Domains.Post.QueryHandlers
{
    public class GetPostByIdQueryHandler : IRequestHandler<GetPostByIdQuery, PostResponse>
    {
        private readonly NewsFeedContext context;

        public GetPostByIdQueryHandler(NewsFeedContext context)
        {
            this.context = context;
        }

        public async Task<PostResponse> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
        {
            var post = await context.Posts.Include(p=>p.Author).Where(p=>p.Id == request.Id).Include(p => p.Comments).Include(p=>p.Likes).FirstOrDefaultAsync();
            if(post == null)
            {
                return null;
            }
            var likesResult = new List<LikeResponse>();
            post.Likes.ToList().ForEach(item =>
            {
                likesResult.Add(new LikeResponse
                {
                    AuthorName = item.UserName
                });
            });
            var result = new PostResponse()
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                CreatedAt = post.CreatedAt,
                UpdatedAt = post.UpdatedAt,
                AuthorName = post.Author.UserName,
                Likes = likesResult,
                CommentCount = post.Comments.Count
            };
            return result;
        }
    }
}
