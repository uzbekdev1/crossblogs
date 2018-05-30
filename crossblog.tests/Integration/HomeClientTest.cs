using System.Threading.Tasks;
using Xunit;

namespace crossblog.tests.Integration
{
    public class HomeClientTest : BaseClientFixtureTest
    {
        [Fact]
        public async Task Index_ReturnsMessage()
        {
            // act
            var txt="Blog Api";
            var response = await _client.GetAsync($"/home/index");

            // assert
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains(txt, responseString);
        }

        [Fact]
        public async Task Eror_Code_ReturnsMessage()
        {
            // act
            var errorCode = 404;
            var response = await _client.GetAsync($"/home/error/{errorCode}");

            // assert
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("404", responseString);
        }
    }
}