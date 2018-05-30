using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace crossblog.Domain
{
    public partial class CrossBlogDbContext : DbContext
    {

        public CrossBlogDbContext(DbContextOptions<CrossBlogDbContext> options) : base(options)
        {
        }

        public DbSet<Article> Articles { get; set; }

        public DbSet<Comment> Comments { get; set; }
         
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Article>().HasMany(c => c.Comments).WithOne().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
