using BackEnd.API.Domains.Post.Queries;
using BackEnd.API.Context;
using BackEnd.API.Models.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using BackEnd.API.Utils.Mappers;

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
            var post = await context.Posts.Include(p=>p.Author).Where(p=>p.Id == request.Id).Include(p => p.Comments).Include(p=>p.Likes).Include(p=>p.Tags).FirstOrDefaultAsync();
            if(post == null)
            {
                return null;
            }
            var result = PostMapper.ToPostResponse(post);
            return result;
        }
    }
}