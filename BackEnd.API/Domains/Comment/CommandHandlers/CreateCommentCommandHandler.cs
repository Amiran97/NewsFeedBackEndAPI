﻿using BackEnd.API.Domains.Comment.Commands;
using BackEnd.Infrastructure.Context;
using BackEnd.Infrastructure.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BackEnd.API.Domains.Comment.CommandHandlers
{
    public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, Unit>
    {
        private readonly NewsFeedContext context;
        private readonly UserManager<User> userManager;

        public CreateCommentCommandHandler(NewsFeedContext context, UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<Unit> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            User author = await userManager.FindByNameAsync(request.AuthorName);
            Infrastructure.Models.Post post = await context.Posts.FindAsync(request.PostId);
            if(post != null && author != null)
            {
                Infrastructure.Models.Comment comment = new Infrastructure.Models.Comment
                {
                    Author = author,
                    Post = post,
                    Message = request.Message,
                    CreateAt = DateTime.UtcNow
                };
                context.Comments.Add(comment);
                await context.SaveChangesAsync();
            }
            return Unit.Value;
        }
    }
}
