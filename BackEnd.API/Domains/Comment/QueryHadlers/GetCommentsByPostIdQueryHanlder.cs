using BackEnd.API.Domains.Comment.Queries;
using BackEnd.Infrastructure.Context;
using BackEnd.Infrastructure.Models.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.API.Domains.Comment.QueryHadlers
{
    public class GetCommentsByPostIdQueryHanlder : IRequestHandler<GetCommentsByPostIdQuery, IEnumerable<CommentResponse>>
    {
        private readonly NewsFeedContext context;

        public GetCommentsByPostIdQueryHanlder(NewsFeedContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<CommentResponse>> Handle(GetCommentsByPostIdQuery request, CancellationToken cancellationToken)
        {
            
            var comments = await context.Comments.Include(c=>c.Post).Where(c=>c.Post.Id == request.PostId).Include(c=>c.Likes).Include(c=>c.Author).ToListAsync();
            var result = new List<CommentResponse>();
            comments.ForEach(item =>
            {
                var likesResult = new List<LikeResponse>();
                item.Likes.ToList().ForEach(pl =>
                {
                    likesResult.Add(new LikeResponse
                    {
                        AuthorName = pl.UserName
                    });
                });
                result.Add(new CommentResponse
                {
                    Id = item.Id,
                    AuthorName = item.Author.UserName,
                    CreateAt = item.CreateAt,
                    Message = item.Message,
                    PostId = request.PostId,
                    Likes = likesResult
                });
            });
            return result;
        }
    }
}
