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
            
            var comments = await context.Comments.Include(c=>c.Post).Where(c=>c.Post.Id == request.PostId).ToListAsync();
            var result = new List<CommentResponse>();
            var likes = await context.CommentsLikes.Include(pl => pl.Comment).Include(pl => pl.Author).ToListAsync();
            comments.ForEach(item =>
            {
                var likesResult = new List<LikeResponse>();
                likes.ForEach(pl =>
                {
                    if (pl.Comment.Id == item.Id)
                    {
                        likesResult.Add(new LikeResponse
                        {
                            AuthorName = pl.Author.UserName
                        });
                    }
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
