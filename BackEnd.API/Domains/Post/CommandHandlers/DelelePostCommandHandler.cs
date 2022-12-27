using BackEnd.API.Domains.Post.Commands;
using BackEnd.API.Context;
using BackEnd.API.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net.Mime;
using BackEnd.API.Utils;

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
            var post = await context.Posts.Include(p => p.Author).Where(p => p.Id == request.Id && p.Author.UserName == request.AuthorName).Include(p=>p.Tags).Include(p=>p.Images).FirstOrDefaultAsync();
            if (post == null)
            {
                throw new KeyNotFoundException();
            } else
            {
                post.Tags.ToList().ForEach(tag => {
                    tag.Posts.Remove(post);
                    if(tag.Posts.Count <= 0)
                    {
                        context.Tags.Remove(tag);
                    } else
                    {
                        context.Tags.Update(tag);
                    }
                });
                post.Comments.ToList().ForEach(comment => context.Comments.Remove(comment));
                post.Images.ToList().ForEach(image =>
                {
                    ImageHelper.RemoveImage(image.Path);
                    context.PostImages.Remove(image);
                });
                context.Posts.Remove(post);
                await context.SaveChangesAsync();
            }
            return Unit.Value;
        }
    }
}
