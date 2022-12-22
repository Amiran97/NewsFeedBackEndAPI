using BackEnd.API.Domains.Comment.Queries;
using BackEnd.API.Context;
using BackEnd.API.Models.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using BackEnd.API.Utils.Mappers;

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
            var comments = await context.Comments.Include(c=>c.Post).Where(c=>c.Post.Id == request.PostId).OrderByDescending(c => c.CreateAt).Include(c=>c.Likes).Include(c=>c.Author).ToListAsync();
            var result = CommentMapper.ToCommentResponseCollection(comments, request.PostId);
            return result;
        }
    }
}