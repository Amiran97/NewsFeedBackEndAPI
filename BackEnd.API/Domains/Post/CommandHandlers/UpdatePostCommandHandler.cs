using BackEnd.API.Domains.Post.Commands;
using BackEnd.API.Context;
using BackEnd.API.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.API.Domains.Post.CommandHandlers
{
    public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, Unit>
    {
        private readonly NewsFeedContext context;

        public UpdatePostCommandHandler(NewsFeedContext context, UserManager<User> userManager)
        {
            this.context = context;
        }

        public async Task<Unit> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {
            var post = await context.Posts.Include(p => p.Author).FirstOrDefaultAsync(p => p.Id == request.Id && p.Author.UserName == request.AuthorName);
            if (post == null)
            {
                throw new KeyNotFoundException();
            }
            else
            {
                post.Title = request.Title;
                post.Content = request.Context;
                post.UpdatedAt = DateTime.UtcNow;
                context.Posts.Update(post);
                await context.SaveChangesAsync();
            }
            return Unit.Value;
        }
    }
}
