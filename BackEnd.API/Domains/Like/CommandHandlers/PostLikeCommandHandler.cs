using BackEnd.API.Domains.Like.Commands;
using BackEnd.API.Context;
using BackEnd.API.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BackEnd.API.Utils.Mappers;
using BackEnd.API.Models.Dtos;

namespace BackEnd.API.Domains.Like.CommandHandlers
{
    public class PostLikeCommandHandler : IRequestHandler<PostLikeCommand, PostResponse>
    {
        private readonly NewsFeedContext context;
        private readonly UserManager<User> userManager;

        public PostLikeCommandHandler(NewsFeedContext context, UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<PostResponse> Handle(PostLikeCommand request, CancellationToken cancellationToken)
        {
            User author = await userManager.FindByNameAsync(request.AuthorName);
            Models.Post post = await context.Posts.Where(p=>p.Id == request.Id).Include(p=>p.Author).FirstOrDefaultAsync();

            if (author != null && post != null)
            {
                User postLike = post.Likes.ToList().Find(u => u.UserName == request.AuthorName);
                
                if (postLike != null)
                {
                    post.Likes.Remove(postLike);
                    context.Posts.Update(post);
                    await context.SaveChangesAsync();
                }
                else
                {
                    User postDislike = post.Dislikes.ToList().Find(u => u.UserName == request.AuthorName);
                    if (postDislike != null)
                    {
                        post.Dislikes.Remove(postDislike);
                    }

                    post.Likes.Add(author);
                    context.Posts.Update(post);
                    await context.SaveChangesAsync();
                }
            }

            return PostMapper.ToPostResponse(post);
        }
    }
}
