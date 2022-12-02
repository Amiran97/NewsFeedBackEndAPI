using BackEnd.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BackEnd.Infrastructure.Context
{
    public class NewsFeedContext : IdentityDbContext<User>
    {
        public DbSet<Post> Posts { get; set; }

        public NewsFeedContext(DbContextOptions options) : base(options)
        {

        }
    }
}
