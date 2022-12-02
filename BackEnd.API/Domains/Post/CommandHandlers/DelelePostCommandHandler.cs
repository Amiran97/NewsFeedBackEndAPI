using BackEnd.API.Domains.Post.Commands;
using BackEnd.Infrastructure.Context;
using BackEnd.Infrastructure.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.API.Domains.Post.CommandHandlers
{
    public class DelelePostCommandHandler : IRequestHandler<DeletePostCommand, Unit>
    {
        private readonly NewsFeedContext context;

        public DelelePostCommandHandler(NewsFeedContext context, UserManager<User> userManager)
        {
            this.context = context;
        }

        public async Task<Unit> Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            var post = await context.Posts.Include(p => p.Author).FirstOrDefaultAsync(p => p.Id == request.Id && p.Author.UserName == request.AuthorName);
            if (post == null)
            {
                throw new KeyNotFoundException();
            } else
            {
                context.Posts.Remove(post);
                await context.SaveChangesAsync();
            }
            return Unit.Value;
        }
    }
}
