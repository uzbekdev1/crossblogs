using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using crossblog.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace crossblog
{
    public class BlogContextFactory : IDesignTimeDbContextFactory<CrossBlogDbContext>
    {
        public CrossBlogDbContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var connectionString = config.GetConnectionString("CrossBlog");
            var builder = new DbContextOptionsBuilder<CrossBlogDbContext>();

            builder.UseMySql(connectionString);

            return new CrossBlogDbContext(builder.Options);
        }
    }
}
