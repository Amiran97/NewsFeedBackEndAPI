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
    public class PostDislikeCommandHandler : IRequestHandler<PostDislikeCommand, PostResponse>
    {
        private readonly NewsFeedContext context;
        private readonly UserManager<User> userManager;

        public PostDislikeCommandHandler(NewsFeedContext context, UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<PostResponse> Handle(PostDislikeCommand request, CancellationToken cancellationToken)
        {
            User author = await userManager.FindByNameAsync(request.AuthorName);
            Models.Post post = await context.Posts.Where(p => p.Id == request.Id).Include(p => p.Author).FirstOrDefaultAsync();

            if (author != null && post != null)
            {
                User postDislike = post.Dislikes.ToList().Find(u => u.UserName == request.AuthorName);

                if (postDislike != null)
                {
                    post.Dislikes.Remove(postDislike);
                    context.Posts.Update(post);
                    await context.SaveChangesAsync();
                }
                else
                {
                    User postLike = post.Likes.ToList().Find(u => u.UserName == request.AuthorName);
                    if (postLike != null)
                    {
                        post.Likes.Remove(postLike);
                    }

                    post.Dislikes.Add(author);
                    context.Posts.Update(post);
                    await context.SaveChangesAsync();
                }
            }

            return PostMapper.ToPostResponse(post);
        }
    }
}
