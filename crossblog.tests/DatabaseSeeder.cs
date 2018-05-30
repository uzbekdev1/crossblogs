using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using crossblog.Domain;
using crossblog.Model;
using Microsoft.EntityFrameworkCore;

namespace crossblog.tests
{
    internal sealed class DatabaseSeeder
    {

        private readonly CrossBlogDbContext _context;

        public DatabaseSeeder(CrossBlogDbContext context)
        {
            _context = context;
        }

        // Add all the predefined articles
        public static ArticleModel[] Articles = new[]
        {
            new ArticleModel
            {
                Id = 1,
                Title = "Article 1",
                Content = "Contents 1",
                Date = DateTime.Now
            },
            new ArticleModel
            {
                Id = 2,
                Title = "Article 2",
                Content = "Contents 2",
                Date = DateTime.Now
            }
        };

        public static CommentModel[] Comments = new[]
        {
            new CommentModel
            {
                Id = 1,
                ArticleId = 1,
                Title = "Comment 1",
                Content = "Contents 1",
                Date = DateTime.Now
            },
            new CommentModel
            {
                Id = 2,
                ArticleId = 2,
                Title = "Comment 2",
                Content = "Contents 2",
                Date = DateTime.Now
            }
        };

        public void Seed()
        {
            //articles
            foreach (var article in Articles)
            {
                var ent = new Article
                {
                    Date = article.Date,
                    Content = article.Content,
                    Title = article.Title 
                };

                _context.Entry(ent).State = EntityState.Added;
                _context.Articles.Add(ent);

            }
              _context.SaveChanges();

            //comments
            foreach (var comment in Comments)
            {
                var ent = new Comment
                {
                    Date = comment.Date,
                    Content = comment.Content,
                    Title = comment.Title,
                    ArticleId = comment.ArticleId
                };
                _context.Entry(ent).State = EntityState.Added;
                _context.Comments.Add(ent);
            }
              _context.SaveChanges();

        }
    }
}
