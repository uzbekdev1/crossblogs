using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using crossblog.Domain;
using crossblog.Dto;
using crossblog.Model;
using Newtonsoft.Json;
using Xunit;

namespace crossblog.tests.Integration
{
    public class CommentsClientTest : BaseClientFixtureTest
    {

        [Fact]
        public async Task GetComments_ReturnsCommentsList()
        {
            // act
            var articleId = DatabaseSeeder.Articles[0].Id;
            var request = await _client.GetAsync($"/articles/{articleId}/comments");

            // assert
            request.EnsureSuccessStatusCode();

            var response = JsonConvert.DeserializeObject<ApiResponse>(await request.Content.ReadAsStringAsync());
            Assert.NotNull(response);
            Assert.True(response.IsSuccess);

            var result = JsonConvert.DeserializeObject<CommentListModel>(response.Result.ToString());
            Assert.NotNull(result);
            Assert.True(result.Comments.Any());

        }

        [Fact]
        public async Task GetComment_ReturnsSpecifiedArticle()
        {
            // act
            var articleId = DatabaseSeeder.Articles[0].Id;
            var commentId = DatabaseSeeder.Comments[0].Id;
            var request = await _client.GetAsync($"/articles/{articleId}/comments/{commentId}");

            // assert
            request.EnsureSuccessStatusCode();

            var response = JsonConvert.DeserializeObject<ApiResponse>(await request.Content.ReadAsStringAsync());
            Assert.NotNull(response);
            Assert.True(response.IsSuccess);

            var result = JsonConvert.DeserializeObject<CommentModel>(response.Result.ToString());
            Assert.NotNull(result);
            Assert.Equal(commentId, result.ArticleId);

        }

        [Fact]
        public async Task AddComment_ReturnsAddedComment()
        {
            // arrange
            var articleId = DatabaseSeeder.Articles[0].Id;
            var article = new CommentModel
            {
                Title = "mock title",
                Content = "mock contents"
            };

            // act
            var contents = new StringContent(JsonConvert.SerializeObject(article, Formatting.Indented), Encoding.UTF8, "application/json");
            var request = await _client.PostAsync($"/articles/{articleId}/comments", contents);

            // assert
            request.EnsureSuccessStatusCode();

            var response = JsonConvert.DeserializeObject<ApiResponse>(await request.Content.ReadAsStringAsync());
            Assert.NotNull(response);
            Assert.True(response.IsSuccess);

            var result = JsonConvert.DeserializeObject<CommentModel>(response.Result.ToString());
            Assert.True(result.Id > 0);
            Assert.Equal(article.Title, result.Title);
            Assert.Equal(article.Content, result.Content);
        }

    }
}