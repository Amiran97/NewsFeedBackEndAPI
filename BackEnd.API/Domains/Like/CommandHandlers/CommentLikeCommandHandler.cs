using BackEnd.API.Domains.Like.Commands;
using BackEnd.API.Context;
using BackEnd.API.Models;
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
            Models.Comment comment = await context.Comments.Where(c=>c.Id == request.Id).Include(c=>c.Author).FirstOrDefaultAsync();

            if (author != null && comment != null)
            {
                User commentLike = comment.Likes.ToList().Find(u=>u.UserName == request.AuthorName);
                if (commentLike != null)
                {
                    comment.Likes.Remove(commentLike);
                    context.Comments.Update(comment);
                    await context.SaveChangesAsync();
                }
                else 
                { 
                    comment.Likes.Add(author);
                    context.Comments.Update(comment);
                    await context.SaveChangesAsync();
                }
            }

            return Unit.Value;
        }
    }
}
