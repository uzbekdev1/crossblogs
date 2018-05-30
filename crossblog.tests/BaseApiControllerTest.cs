using crossblog.Controllers;
using crossblog.Repositories;
using Moq;

namespace crossblog.tests
{
    public abstract class BaseApiControllerTest
    {
        protected readonly Mock<IArticleRepository> _articlesRepositoryMock;
        protected readonly Mock<ICommentRepository> _commentsRepositoryMock;
        protected readonly ArticlesController _articlesController;
        protected readonly CommentsController _commentsController;

        protected BaseApiControllerTest()
        {

            //articles
            _articlesRepositoryMock = new Mock<IArticleRepository>();
            _articlesController = new ArticlesController(_articlesRepositoryMock.Object);

            //coments
            _commentsRepositoryMock = new Mock<ICommentRepository>();
            _commentsController = new CommentsController(_commentsRepositoryMock.Object);
        }

    }
}