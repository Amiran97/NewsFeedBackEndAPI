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
            var result = new PostResponse()
            {
                Id = post.Id,
                Content = post.Content,
                CreatedAt = post.CreatedAt,
                AuthorName = post.Author.UserName
            };
            return result;
        }
    }
}
