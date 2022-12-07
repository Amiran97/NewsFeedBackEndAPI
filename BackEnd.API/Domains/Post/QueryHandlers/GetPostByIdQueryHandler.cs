using BackEnd.API.Domains.Post.Queries;
using BackEnd.Infrastructure.Context;
using BackEnd.Infrastructure.Models.Dtos;
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
            var post = await context.Posts.Include(p=>p.Author).FirstOrDefaultAsync(p=>p.Id == request.Id);
            if(post == null)
            {
                return null;
            }
            var likes = await context.PostLikes.Include(pl => pl.Post).Include(pl => pl.Author).Where(pl=>pl.Post.Id == request.Id).ToListAsync();
            var likesResult = new List<LikeResponse>();
            likes.ForEach(pl =>
            {
                likesResult.Add(new LikeResponse
                {
                    AuthorName = pl.Author.UserName
                });
            });
            var commentCount = await context.Comments.Include(c => c.Post).Where(c => c.Post.Id == request.Id).CountAsync();
            var result = new PostResponse()
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                CreatedAt = post.CreatedAt,
                UpdatedAt = post.UpdatedAt,
                AuthorName = post.Author.UserName,
                Likes = likesResult,
                CommentCount = commentCount
            };
            return result;
        }
    }
}
