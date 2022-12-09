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
            var post = await context.Posts.Include(p => p.Author).Where(p => p.Id == request.Id && p.Author.UserName == request.AuthorName).Include(p=>p.Tags).FirstOrDefaultAsync();
            if (post == null)
            {
                throw new KeyNotFoundException();
            }
            else
            {
                post.Title = request.Title;
                post.Content = request.Context;
                post.UpdatedAt = DateTime.UtcNow;

                var tags = await context.Tags.ToListAsync();

                var tagsToRemove = post.Tags.Where(t => !request.Tags.Contains(t.Name)).ToList();
                tagsToRemove.ForEach(tag =>
                {
                    tag.Posts.Remove(post);
                    if (tag.Posts.Count <= 0)
                    {
                        context.Tags.Remove(tag);
                    }
                    else
                    {
                        context.Tags.Update(tag);
                    }
                });

                request.Tags.ToList().ForEach(tag => {
                    if(!tags.Exists(t=>t.Name == tag)) {
                        var newTag = new Tag { Name = tag };
                        context.Tags.Add(newTag);
                        post.Tags.Add(newTag);
                    }
                });

                context.Posts.Update(post);
                await context.SaveChangesAsync();
            }
            return Unit.Value;
        }
    }
}
