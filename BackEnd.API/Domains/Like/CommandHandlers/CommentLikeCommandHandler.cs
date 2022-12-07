using BackEnd.API.Domains.Like.Commands;
using BackEnd.Infrastructure.Context;
using BackEnd.Infrastructure.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.API.Domains.Like.CommandHandlers
{
    public class CommentLikeCommandHandler : IRequestHandler<CommentLikeCommand, Unit>
    {
        private readonly NewsFeedContext context;
        private readonly UserManager<User> userManager;

        public CommentLikeCommandHandler(NewsFeedContext context, UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<Unit> Handle(CommentLikeCommand request, CancellationToken cancellationToken)
        {
            User author = await userManager.FindByNameAsync(request.AuthorName);
            Infrastructure.Models.Comment comment = await context.Comments.FindAsync(request.Id);

            if (author != null && comment != null)
            {
                CommentLike commentLike = await context.CommentsLikes.Include(cl => cl.Comment).Include(cl => cl.Author)
                    .FirstOrDefaultAsync(cl => cl.Comment.Id == request.Id && cl.Author.UserName == request.AuthorName);
                if (commentLike != null)
                {
                    context.CommentsLikes.Remove(commentLike);
                    await context.SaveChangesAsync();
                }
                else
                {
                    commentLike = new CommentLike { Author = author, Comment = comment };
                    context.CommentsLikes.Add(commentLike);
                    await context.SaveChangesAsync();
                }
            }

            return Unit.Value;
        }
    }
}
