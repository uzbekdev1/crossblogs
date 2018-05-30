using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using crossblog.Domain;
using crossblog.Model;

namespace crossblog.Extensions
{
    public static class MappingExtension
    {
        public static ArticleModel AsDto(this Article article)
        {
            return new ArticleModel
            {
                Content = article.Content,
                Title = article.Title,
                Id = article.Id,
                Date = article.Date,
                Published = article.Published
            };
        }

        public static CommentModel AsDto(this Comment comment)
        {
            return new CommentModel
            {
                Content = comment.Content,
                Title = comment.Title,
                Id = comment.Id,
                Date = comment.Date,
                Published = comment.Published,
                Email = comment.Email,
                ArticleId = comment.ArticleId
            };
        }

    }
}
