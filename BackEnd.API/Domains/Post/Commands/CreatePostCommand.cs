﻿using MediatR;

namespace BackEnd.API.Domains.Post.Commands
{
    public class CreatePostCommand : IRequest<Unit>
    {
        public string Title { get; set; }
        public string Context { get; set; }
        public string AuthorName { get; set; }
    }
}
