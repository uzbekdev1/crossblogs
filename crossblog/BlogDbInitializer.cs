using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using crossblog.Domain;
using Microsoft.EntityFrameworkCore;

namespace crossblog
{
    public sealed class BlogDbInitializer
    {
        private readonly CrossBlogDbContext _dbContext;

        public BlogDbInitializer(CrossBlogDbContext dbContext)
        {
            _dbContext = dbContext ;
        }

        public void Seed()
        {
            // Run Migrations
            //_dbContext.Database.Migrate();

            //Seed data
            //if (!_dbContext.Articles.Any() || !_dbContext.Comments.Any())
            //{
            //    _dbContext.SaveChanges();
            //}
        }
    }
}
