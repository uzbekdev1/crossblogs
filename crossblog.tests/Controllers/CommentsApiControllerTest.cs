using System;
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
    public class CommentsApiControllerTest : BaseApiControllerTest
    {

        [Fact]
        public async Task GetCommentsTest_RetursArticle()
        {
            // arrange 
            var mockArticleId = 1;
            var mockCommmentsList = new List<Comment>
            {
                new Comment {Title = "mock comment 1",ArticleId = mockArticleId},
                new Comment {Title = "mock comment 2",ArticleId = mockArticleId}
            }.ToAsyncDbSetMock();
            _commentsRepositoryMock.Setup(repo => repo.Query()).Returns(mockCommmentsList.Object);

            // act
            var fetch = await _commentsController.Get(mockArticleId);

            // assert
            var result = Assert.IsType<OkObjectResult>(fetch);
            Assert.NotNull(result);
            var response = (ApiResponse)result.Value;
            Assert.NotNull(response);
            Assert.True(response.IsSuccess);
            Assert.True(((CommentListModel)response.Result).Comments.Any());
        }

        [Fact]
        public async Task GetCommentTest_ReturnsNotFound_WhenCommentDoesNorExists()
        {
            // arrange 
            var mockCommentId = 3;
            var mockArticleId = 1;
            var mockCommmentsList = new List<Comment>
            {
                new Comment {ArticleId = mockArticleId,Id = 1,Title = "mock comment 1"},
                new Comment {ArticleId = mockArticleId,Id = 2,Title = "mock comment 2"}
            }.ToAsyncDbSetMock();
            _commentsRepositoryMock.Setup(repo => repo.Query()).Returns(mockCommmentsList.Object);

            // act
            var fetch = await _commentsController.Get(mockArticleId, mockCommentId);

            // assert
            var result = Assert.IsType<NotFoundObjectResult>(fetch);
            Assert.NotNull(result);
            var response = (ApiResponse)result.Value;
            Assert.NotNull(response);
            Assert.False(response.IsSuccess);
            Assert.NotEqual("", response.ErrorMessage);
        }

        [Fact]
        public async Task GetCommentTest_ReturnsComment_WhenCommentExists()
        {
            // arrange 
            var mockCommentId = 1;
            var mockArticleId = 1;
            var mockCommmentsList = new List<Comment>
            {
                new Comment {ArticleId = mockArticleId,Id = 1,Title = "mock comment 1"},
                new Comment {ArticleId = mockArticleId,Id = 2,Title = "mock comment 2"}
            }.ToAsyncDbSetMock();
            _commentsRepositoryMock.Setup(repo => repo.Query()).Returns(mockCommmentsList.Object);

            // act
            var fetch = await _commentsController.Get(mockArticleId, mockCommentId);

            // assert
            var result = Assert.IsType<OkObjectResult>(fetch);
            Assert.NotNull(result);
            var response = (ApiResponse)result.Value;
            Assert.NotNull(response);
            Assert.True(response.IsSuccess);
            Assert.Equal(mockArticleId, ((CommentModel)response.Result).ArticleId);
        }

        [Fact]
        public async Task AddCommentTest_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // arrange 
            var mockArticleId = 1;
            var mockComment = new CommentModel
            {
                Title = "mock comment",
                Date = DateTime.Now
            };
            _commentsController.ModelState.AddModelError("Content", "This field is required");

            // act
            var fetch = await _commentsController.Post(mockArticleId, mockComment);

            // assert
            var result = Assert.IsType<BadRequestObjectResult>(fetch);
            Assert.NotNull(result);
            var response = (ApiResponse)result.Value;
            Assert.NotNull(response);
            Assert.False(response.IsSuccess);
            Assert.Equal(_commentsController.ModelState.GetErrors(), response.ErrorMessage);
        }

        [Fact]
        public async Task AddCommentTest_ReturnsCommentSuccessfullyAdded()
        {
            // arrange
            var mockArticleId = 1;
            var mockComment = new Comment { Title = "mock comment", Content = "test" };
            _commentsRepositoryMock.Setup(repo => repo.InsertAsync(mockComment)).Returns(Task.CompletedTask);
            _commentsRepositoryMock.Setup(repo => repo.SaveChangesAsync()).Returns(Task.CompletedTask);

            // act
            var fetch = await _commentsController.Post(mockArticleId, mockComment.AsDto());

            // assert
            _commentsRepositoryMock.Verify(repo => repo.InsertAsync(It.IsAny<Comment>()), Times.Once);

            var result = Assert.IsType<OkObjectResult>(fetch);
            Assert.NotNull(result);
            var response = (ApiResponse)result.Value;
            Assert.NotNull(response);
            Assert.True(response.IsSuccess);
            Assert.Equal(mockComment.Title, ((CommentModel)response.Result).Title);
            Assert.Equal(mockComment.Content, ((CommentModel)response.Result).Content);
            Assert.Equal(mockArticleId, ((CommentModel)response.Result).ArticleId);
        }

    }
}