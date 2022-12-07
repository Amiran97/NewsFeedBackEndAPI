﻿using BackEnd.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BackEnd.Infrastructure.Context
{
    public class NewsFeedContext : IdentityDbContext<User>
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostLike> PostLikes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CommentLike> CommentsLikes { get; set; }

        public NewsFeedContext(DbContextOptions options) : base(options)
        {

        }
    }
}
