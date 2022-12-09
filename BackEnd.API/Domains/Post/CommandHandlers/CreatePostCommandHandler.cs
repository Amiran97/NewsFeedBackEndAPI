using BackEnd.API.Domains.Post.Commands;
using BackEnd.API.Context;
using BackEnd.API.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BackEnd.API.Domains.Post.CommandHandlers
{
    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, Unit>
    {
        private readonly NewsFeedContext context;
        private readonly UserManager<User> userManager;

        public CreatePostCommandHandler(NewsFeedContext context, UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<Unit> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            User author = await userManager.FindByNameAsync(request.AuthorName);
            var newPost = new Models.Post() {
                Title = request.Title,
                Content = request.Context,
                Author = author,
                CreatedAt = DateTime.UtcNow,
            };
            newPost.UpdatedAt = newPost.CreatedAt;
            context.Posts.Add(newPost);
            await context.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
