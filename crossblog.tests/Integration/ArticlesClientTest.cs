using System;
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
    public class ArticlesClientTest : BaseClientFixtureTest
    {

        [Fact]
        public async Task GetArticles_ReturnsArticlesList()
        {
            // act
            var title = DatabaseSeeder.Articles[0].Title;
            var request = await _client.GetAsync($"/articles/search?title={title}");

            // assert
            request.EnsureSuccessStatusCode();

            var response = JsonConvert.DeserializeObject<ApiResponse>(await request.Content.ReadAsStringAsync());
            Assert.NotNull(response);
            Assert.True(response.IsSuccess);

            var result = JsonConvert.DeserializeObject<ArticleListModel>(response.Result.ToString());
            Assert.NotStrictEqual(DatabaseSeeder.Articles, result.Articles);

        }

        [Fact]
        public async Task GetArticle_ReturnsSpecifiedArticle()
        {
            // act
            var id = DatabaseSeeder.Articles[0].Id;
            var request = await _client.GetAsync($"/articles/{id}");

            // assert
            request.EnsureSuccessStatusCode();

            var response = JsonConvert.DeserializeObject<ApiResponse>(await request.Content.ReadAsStringAsync());
            Assert.NotNull(response);
            Assert.True(response.IsSuccess);

            var result = JsonConvert.DeserializeObject<ArticleModel>(response.Result.ToString());
            Assert.NotStrictEqual(DatabaseSeeder.Articles[0], result);
        }

        [Fact]
        public async Task AddArticle_ReturnsAddedArticle()
        {
            // arrange
            var article = new ArticleModel
            {
                Title = "mock title",
                Content = "mock contents"
            };

            // act
            var contents = new StringContent(JsonConvert.SerializeObject(article, Formatting.Indented), Encoding.UTF8, "application/json");
            var request = await _client.PostAsync($"/articles", contents);

            // assert
            request.EnsureSuccessStatusCode();

            var response = JsonConvert.DeserializeObject<ApiResponse>(await request.Content.ReadAsStringAsync());
            Assert.NotNull(response);
            Assert.True(response.IsSuccess);

            var result = JsonConvert.DeserializeObject<ArticleModel>(response.Result.ToString());
            Assert.True(result.Id > 0);
            Assert.Equal(article.Title, result.Title);
            Assert.Equal(article.Content, result.Content);
        }

        [Fact]
        public async Task PutArticle_ReturnsUpdateArticle()
        {
            // arrange
            var id = DatabaseSeeder.Articles[0].Id;
            var article = new ArticleModel
            {
                Title = "mock title",
                Content = "mock contents"
            };

            // act
            var contents = new StringContent(JsonConvert.SerializeObject(article), Encoding.UTF8, "application/json");
            var request = await _client.PutAsync($"/articles/{id}", contents);

            // assert
            request.EnsureSuccessStatusCode();

            var response = JsonConvert.DeserializeObject<ApiResponse>(await request.Content.ReadAsStringAsync());
            Assert.NotNull(response);
            Assert.True(response.IsSuccess);
            Assert.Equal("", response.ErrorMessage);

        }

        [Fact]
        public async Task DeleteConfirmation_RedirectsToList_AfterDeletingArticle()
        {
            // arrange
            var id = DatabaseSeeder.Articles[0].Id;

            // act 
            var request = await _client.DeleteAsync($"/articles/{id}");

            // assert
            request.EnsureSuccessStatusCode();

            var response = JsonConvert.DeserializeObject<ApiResponse>(await request.Content.ReadAsStringAsync());
            Assert.NotNull(response);
            Assert.True(response.IsSuccess);
            Assert.Equal("", response.ErrorMessage);
        }
    }
}