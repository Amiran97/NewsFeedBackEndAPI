using BackEnd.API.Domains.Post.Commands;
using BackEnd.API.Context;
using BackEnd.API.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BackEnd.API.Models.Dtos;
using BackEnd.API.Utils.Mappers;

namespace BackEnd.API.Domains.Post.CommandHandlers
{
    public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, PostResponse>
    {
        private readonly NewsFeedContext context;

        public UpdatePostCommandHandler(NewsFeedContext context, UserManager<User> userManager)
        {
            this.context = context;
        }

        public async Task<PostResponse> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
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

                if(request.Images != null && request.Images.Count > 0)
                {
                    post.Images.ToList().ForEach(image =>
                    {
                        var folderName = Path.Combine("wwwroot", "Images");
                        var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                        var fullPath = Path.Combine(pathToSave, image.Path);
                        if (File.Exists(fullPath))
                        {
                            File.Delete(fullPath);
                        }
                        context.PostImages.Remove(image);
                    });

                    post.Images.Clear();

                    foreach(var file in request.Images)
                    {
                        var newName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        var folderName = Path.Combine("wwwroot", "Images");
                        var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                        var fullPath = Path.Combine(pathToSave, newName);
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                            post.Images.Add(new PostImage() { Path = newName });
                        }
                    }
                }

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
            return PostMapper.ToPostResponse(post);
        }
    }
}
