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
        public DbSet<Tag> Tags { get; set; }
        public DbSet<PostImage> PostImages { get; set; }

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

            builder.Entity<Post>().HasMany(p => p.Likes).WithMany(u => u.PostLikes).UsingEntity(t => t.ToTable("PostLikes"));

            builder.Entity<Post>().HasMany(p => p.Dislikes).WithMany(u => u.PostDislikes).UsingEntity(t => t.ToTable("PostDislikes"));

            builder.Entity<Post>().HasMany(p => p.Tags).WithMany(t => t.Posts).UsingEntity(t => t.ToTable("PostTags"));
            
            builder.Entity<Post>().HasMany(p => p.Images).WithOne(i => i.Post);

            // Comment
            builder.Entity<Comment>().ToTable("Comments");

            builder.Entity<Comment>().HasKey(c => c.Id);
            builder.Entity<Comment>().Property(c => c.Id).ValueGeneratedOnAdd();

            builder.Entity<Comment>().Property(c => c.Message).HasMaxLength(255).IsRequired();

            builder.Entity<Comment>().HasOne(c => c.Author).WithMany(u => u.Comments).HasForeignKey(c => c.AuthorId).OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Comment>().HasOne(c => c.Post).WithMany(p => p.Comments).HasForeignKey(c => c.PostId).OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Comment>().HasMany(c => c.Likes).WithMany(u => u.CommentLikes).UsingEntity(t => t.ToTable("CommentLikes"));

            builder.Entity<Comment>().HasMany(c => c.Dislikes).WithMany(u => u.CommentDislikes).UsingEntity(t => t.ToTable("CommentDislikes"));

            // Tag
            builder.Entity<Tag>().ToTable("Tags");

            builder.Entity<Tag>().HasKey(t => t.Id);
            builder.Entity<Tag>().Property(t => t.Id).ValueGeneratedOnAdd();

            builder.Entity<Tag>().Property(t => t.Name).HasMaxLength(32).IsRequired();

            builder.Entity<Tag>().HasMany(t => t.Posts).WithMany(p => p.Tags).UsingEntity(t => t.ToTable("PostTags"));

            base.OnModelCreating(builder);

            // PostImage
            builder.Entity<PostImage>().ToTable("PostImages");

            builder.Entity<PostImage>().HasKey(i => i.Id);
            builder.Entity<PostImage>().Property(i => i.Id).ValueGeneratedOnAdd();

            builder.Entity<PostImage>().Property(i => i.Path).HasMaxLength(255).IsRequired();

            builder.Entity<PostImage>().HasOne(i => i.Post).WithMany(p => p.Images).HasForeignKey(i => i.PostId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
