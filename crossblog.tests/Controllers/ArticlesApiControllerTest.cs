using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using crossblog.Domain;
using crossblog.Dto;
using crossblog.Extensions;
using crossblog.Model;
using crossblog.tests.Extensions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace crossblog.tests.Controllers
{
    public   class ArticlesApiControllerTest:BaseApiControllerTest
    {

        [Fact]
        public async Task GetArticlesTest_RetursArticlesList()
        {
            // arrange 
            var mockArticlesList = new List<Article>
            {
                new Article {Title = "mock article 1"},
                new Article {Title = "mock article 2"}
            }.ToAsyncDbSetMock();
            _articlesRepositoryMock.Setup(repo => repo.Query()).Returns(mockArticlesList.Object);

            // act
            var fetch = await _articlesController.Search("article");


            // assert
            var result = Assert.IsType<OkObjectResult>(fetch);
            Assert.NotNull(result);
            var response = (ApiResponse)result.Value;
            Assert.NotNull(response);
            Assert.True(response.IsSuccess);
            Assert.True(((ArticleListModel)response.Result).Articles.Any());
            Assert.Equal(((ArticleListModel)response.Result).Articles.Count(), mockArticlesList.Object.Count());
            Assert.All(((ArticleListModel)response.Result).Articles,a => mockArticlesList.Object.All(b=>b.Title==a.Title));
        }

        [Fact]
        public async Task GetArticleTest_ReturnsNotFound_WhenArticleDoesNorExists()
        {

            // arrange
            var mockId = 1;
            _articlesRepositoryMock.Setup(repo => repo.GetAsync(mockId)).Returns(Task.FromResult<Article>(null));

            // act
            var fetch = await _articlesController.Get(mockId);

            // assert
            var result = Assert.IsType<NotFoundObjectResult>(fetch);
            Assert.NotNull(result);
            var response = (ApiResponse)result.Value;
            Assert.NotNull(response);
            Assert.False(response.IsSuccess);
            Assert.NotNull(response.ErrorMessage);
        }

        [Fact]
        public async Task GetArticleTest_ReturnsArticle_WhenArticleExists()
        {
            // arrange
            var mockId = 1;
            var mockArticle = new Article
            {
                Title = "mock article"
            };
            _articlesRepositoryMock.Setup(repo => repo.GetAsync(mockId)).Returns(Task.FromResult(mockArticle));

            // act
            var fetch = await _articlesController.Get(mockId);

            // assert
            var result = Assert.IsType<OkObjectResult>(fetch);
            Assert.NotNull(result);
            var response = (ApiResponse)result.Value;
            Assert.NotNull(response);
            Assert.True(response.IsSuccess);
            Assert.Equal(mockArticle.AsDto().Title, ((ArticleModel)response.Result).Title);
        }

        [Fact]
        public async Task AddArticleTest_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // arrange 
            var mockArticle = new ArticleModel
            {
                Title = "mock article"
            };
            _articlesController.ModelState.AddModelError("Content", "This field is required");

            // act
            var fetch = await _articlesController.Post(mockArticle);

            // assert
            var result = Assert.IsType<BadRequestObjectResult>(fetch);
            Assert.NotNull(result);
            var response = (ApiResponse)result.Value;
            Assert.NotNull(response);
            Assert.False(response.IsSuccess);
            Assert.Equal(_articlesController.ModelState.GetErrors(), response.ErrorMessage);
        }

        [Fact]
        public async Task AddArticleTest_ReturnsArticleSuccessfullyAdded()
        {
            // arrange
            var mockArticle = new Article { Title = "mock article", Content = "test" };
            _articlesRepositoryMock.Setup(repo => repo.InsertAsync(mockArticle)).Returns(Task.CompletedTask);
            _articlesRepositoryMock.Setup(repo => repo.SaveChangesAsync()).Returns(Task.CompletedTask);

            // act
            var fetch = await _articlesController.Post(mockArticle.AsDto());

            // assert
            _articlesRepositoryMock.Verify(repo => repo.InsertAsync(It.IsAny<Article>()), Times.Once);

            var result = Assert.IsType<OkObjectResult>(fetch);
            Assert.NotNull(result);
            var response = (ApiResponse)result.Value;
            Assert.NotNull(response);
            Assert.True(response.IsSuccess);
            Assert.Equal(mockArticle.Title, ((ArticleModel)response.Result).Title);
            Assert.Equal(mockArticle.Content, ((ArticleModel)response.Result).Content);
        }

        [Fact]
        public async Task UpdateArticleTest_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // arrange
            var mockId = 1;
            var mockArticle = new Article
            {
                Title = "mock article"
            };
            _articlesRepositoryMock.Setup(repo => repo.GetAsync(mockId)).Returns(Task.FromResult(mockArticle));

            // act
            _articlesController.ModelState.AddModelError("Content", "This field is required");
            var fetch = await _articlesController.Put(mockId, mockArticle.AsDto());

            // assert
            var result = Assert.IsType<BadRequestObjectResult>(fetch);
            Assert.NotNull(result);
            var response = (ApiResponse)result.Value;
            Assert.NotNull(response);
            Assert.False(response.IsSuccess);
            Assert.Equal(_articlesController.ModelState.GetErrors(), response.ErrorMessage);
        }

        [Fact]
        public async Task UpdateArticleTest_RetursArticleSuccessFullyUpdated()
        {
            // arrange

            var mockId = 1;
            var mockArticle = new Article { Title = "mock article", Content = "test" };
            _articlesRepositoryMock.Setup(repo => repo.GetAsync(mockId)).Returns(Task.FromResult(mockArticle));
            _articlesRepositoryMock.Setup(repo => repo.UpdateAsync(mockArticle)).Returns(Task.CompletedTask);
            _articlesRepositoryMock.Setup(repo => repo.SaveChangesAsync()).Returns(Task.CompletedTask);

            // act
            var fetch = await _articlesController.Put(mockId, mockArticle.AsDto());

            // assert
            _articlesRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Article>()), Times.Once);

            var result = Assert.IsType<OkObjectResult>(fetch);
            Assert.NotNull(result);
            var response = (ApiResponse)result.Value;
            Assert.NotNull(response);
            Assert.True(response.IsSuccess);
        }

        [Fact]
        public async Task DeleteArticleTest_ReturnsNotFound_WhenArticleDoesNorExists()
        {
            // arrange
            var mockId = 1;
            _articlesRepositoryMock.Setup(repo => repo.GetAsync(mockId)).Returns(Task.FromResult<Article>(null));

            // act
            var fetch = await _articlesController.Delete(mockId);

            // assert
            var result = Assert.IsType<NotFoundObjectResult>(fetch);
            Assert.NotNull(result);
            var response = (ApiResponse)result.Value;
            Assert.NotNull(response);
            Assert.False(response.IsSuccess); 
        }

        [Fact]
        public async Task DeleteArticleTest_ReturnsSuccessCode_AfterRemovingArticleFromRepository()
        {
            // arrange 
            var mockId = 1;
            var mockArticle = new Article { Title = "mock article" };
            _articlesRepositoryMock.Setup(repo => repo.GetAsync(mockId)).Returns(Task.FromResult(mockArticle));
            _articlesRepositoryMock.Setup(repo => repo.DeleteAsync(mockArticle)).Returns(Task.CompletedTask);
            _articlesRepositoryMock.Setup(repo => repo.SaveChangesAsync()).Returns(Task.CompletedTask);

            // act
            var fetch = await _articlesController.Delete(mockId);

            // assert
            _articlesRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<Article>()), Times.Once);

            var result = Assert.IsType<OkObjectResult>(fetch);
            Assert.NotNull(result);
            var response = (ApiResponse)result.Value;
            Assert.NotNull(response);
            Assert.True(response.IsSuccess); 
        }

    }
}