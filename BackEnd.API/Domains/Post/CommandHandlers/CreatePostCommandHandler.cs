using BackEnd.API.Domains.Post.Commands;
using BackEnd.API.Context;
using BackEnd.API.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using BackEnd.API.Models.Dtos;
using BackEnd.API.Utils.Mappers;
using BackEnd.API.Utils;

namespace BackEnd.API.Domains.Post.CommandHandlers
{
    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, PostResponse>
    {
        private readonly NewsFeedContext context;
        private readonly UserManager<User> userManager;

        public CreatePostCommandHandler(NewsFeedContext context, UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<PostResponse> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            User author = await userManager.FindByNameAsync(request.AuthorName);
            var newPost = new Models.Post() {
                Title = request.Title,
                Content = request.Context,
                Author = author,
                CreatedAt = DateTime.UtcNow,
                Tags = new List<Tag>()
            };
            newPost.UpdatedAt = newPost.CreatedAt;
            if(request.Tags != null && request.Tags.Count > 0)
            {
                var realTags = context.Tags.Where(t => request.Tags.Contains(t.Name)).ToList();
                realTags.ForEach(tag => newPost.Tags.Add(tag));
                var tagsToCreate = request.Tags.Where(t => !realTags.Exists(r => r.Name == t));
                foreach(var item in tagsToCreate)
                {
                    var tag = new Tag { Name = item };
                    context.Tags.Add(tag);
                    newPost.Tags.Add(tag);
                }
            }

            // Save Images
            if(request.Images != null)
            {
                var imagesRes = await ImageHelper.SaveImages(request.Images);
                imagesRes.ToList().ForEach(image =>
                {
                    newPost.Images.Add(new PostImage() { Path = image });
                });
            }

            context.Posts.Add(newPost);
            await context.SaveChangesAsync();
            return PostMapper.ToPostResponse(newPost);
        }
    }
}
