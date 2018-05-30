using System.Collections.Generic;

namespace crossblog.Model
{
    public class ArticleListModel
    {
        public IEnumerable<ArticleModel> Articles { get; set; }

        public ArticleListModel() : this(new ArticleModel[0])
        {
        }

        public ArticleListModel(IEnumerable<ArticleModel> articles)
        {
            Articles = articles;
        }
    }
}