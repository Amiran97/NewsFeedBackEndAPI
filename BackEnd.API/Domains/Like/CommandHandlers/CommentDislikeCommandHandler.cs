using BackEnd.API.Context;
using BackEnd.API.Domains.Like.Commands;
using BackEnd.API.Models;
using BackEnd.API.Models.Dtos;
using BackEnd.API.Utils.Mappers;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.API.Domains.Like.CommandHandlers
{
    public class CommentDisikeCommandHandler : IRequestHandler<CommentDislikeCommand, CommentResponse>
    {
        private readonly NewsFeedContext context;
        private readonly UserManager<User> userManager;

        public CommentDisikeCommandHandler(NewsFeedContext context, UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<CommentResponse> Handle(CommentDislikeCommand request, CancellationToken cancellationToken)
        {
            User author = await userManager.FindByNameAsync(request.AuthorName);
            Models.Comment comment = await context.Comments.Where(c => c.Id == request.Id).Include(c => c.Author).FirstOrDefaultAsync();

            if (author != null && comment != null)
            {
                User commentDislike = comment.Dislikes.ToList().Find(u => u.UserName == request.AuthorName);

                if (commentDislike != null)
                {
                    comment.Dislikes.Remove(commentDislike);
                    context.Comments.Update(comment);
                    await context.SaveChangesAsync();
                }
                else
                {
                    User commentLike = comment.Likes.ToList().Find(u => u.UserName == request.AuthorName);
                    if (commentLike != null)
                    {
                        comment.Likes.Remove(commentLike);
                    }

                    comment.Dislikes.Add(author);
                    context.Comments.Update(comment);
                    await context.SaveChangesAsync();
                }
            }

            return CommentMapper.ToCommentResponse(comment, comment.PostId);
        }
    }
}
