using BackEnd.API.Domains.Like.Commands;
using BackEnd.Infrastructure.Context;
using BackEnd.Infrastructure.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.API.Domains.Like.CommandHandlers
{
    public class PostLikeCommandHandler : IRequestHandler<PostLikeCommand, Unit>
    {
        private readonly NewsFeedContext context;
        private readonly UserManager<User> userManager;

        public PostLikeCommandHandler(NewsFeedContext context, UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<Unit> Handle(PostLikeCommand request, CancellationToken cancellationToken)
        {
            User author = await userManager.FindByNameAsync(request.AuthorName);
            Infrastructure.Models.Post post = await context.Posts.FindAsync(request.Id);

            if(author != null && post != null) {
                PostLike postLike = await context.PostLikes.Include(pl => pl.Post).Include(pl=>pl.Author)
                    .FirstOrDefaultAsync(pl => pl.Post.Id == request.Id && pl.Author.UserName == request.AuthorName);
                if(postLike != null)
                {
                    context.PostLikes.Remove(postLike);
                    await context.SaveChangesAsync();
                } else
                {
                    postLike = new PostLike { Author = author, Post = post };
                    context.PostLikes.Add(postLike);
                    await context.SaveChangesAsync();
                }
            }

            return Unit.Value;
        }
    }
}
