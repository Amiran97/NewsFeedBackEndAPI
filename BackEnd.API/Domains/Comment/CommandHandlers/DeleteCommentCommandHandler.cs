using BackEnd.API.Domains.Comment.Commands;
using BackEnd.Infrastructure.Context;
using BackEnd.Infrastructure.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.API.Domains.Comment.CommandHandlers
{
    public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, Unit>
    {
        private readonly NewsFeedContext context;
        private readonly UserManager<User> userManager;

        public DeleteCommentCommandHandler(NewsFeedContext context, UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }
        public async Task<Unit> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = await context.Comments.Include(p => p.Author).Include(p=>p.Post).FirstOrDefaultAsync(p => p.Id == request.PostId && p.Author.UserName == request.AuthorName);
            if (comment == null)
            {
                throw new KeyNotFoundException();
            }
            else
            {
                context.Comments.Remove(comment);
                await context.SaveChangesAsync();
            }
            return Unit.Value;
        }
    }
}
