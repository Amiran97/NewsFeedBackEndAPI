using BackEnd.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Reflection.Emit;

namespace BackEnd.API.Context
{
    public class NewsFeedContext : IdentityDbContext<User>
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public NewsFeedContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Post
            builder.Entity<Post>().ToTable("Posts");

            builder.Entity<Post>().HasKey(p => p.Id);
            builder.Entity<Post>().Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Entity<Post>().Property(p => p.Content).HasMaxLength(255).IsRequired();

            builder.Entity<Post>().Property(p => p.Title).HasMaxLength(100).IsRequired();

            builder.Entity<Post>().HasOne(p => p.Author).WithMany(u => u.Posts).HasForeignKey(p => p.AuthorId).OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Post>().HasMany(p => p.Comments).WithOne(c => c.Post);

            builder.Entity<Post>().HasMany(p => p.Likes).WithMany(c => c.PostLikes).UsingEntity(t => t.ToTable("PostLikes"));

            // Comment
            builder.Entity<Comment>().ToTable("Comments");

            builder.Entity<Comment>().HasKey(c => c.Id);
            builder.Entity<Comment>().Property(c => c.Id).ValueGeneratedOnAdd();

            builder.Entity<Comment>().Property(c => c.Message).HasMaxLength(255).IsRequired();

            builder.Entity<Comment>().HasOne(c => c.Author).WithMany(u => u.Comments).HasForeignKey(c => c.AuthorId).OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Comment>().HasOne(c => c.Post).WithMany(p => p.Comments).HasForeignKey(c => c.PostId).OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Comment>().HasMany(c => c.Likes).WithMany(u => u.CommentLikes).UsingEntity(t => t.ToTable("CommentLikes"));

            base.OnModelCreating(builder);
        }
    }
}
