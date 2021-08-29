using Bakerscraper.Searchers;
using Bakerscraper.Tests.Searchers.Assets;
using Moq;
using Moq.Protected;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Bakerscraper.Tests.Searchers
{
    public class BBCGoodFoodRecipeSearchTest
   {
        [Theory]
        [InlineData("test", "test")]
        [InlineData("space test", "space+test")]
        public async void BBCGoodFoodSearcher_GivenString_CallsHttpClientCorrectly(string testSearchString, string expectedSearchString)
        {
            var expectedLink = "https://www.bbcgoodfood.com/search/recipes?q=" + expectedSearchString;
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler
                .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(new HttpResponseMessage());
            var mockClient = new HttpClient(mockHandler.Object);
            var testSearcher = new BBCGoodFoodRecipeSearch(mockClient);

            await testSearcher.Search(testSearchString);

            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req => req.RequestUri.ToString() == expectedLink),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async void BBCGoodFoodSearcher_GivenCorrectString_ReturnsCorrectOutput()
        {
           
            var testSearchString = "test";
            var testHtml = File.ReadAllText("Searchers/Assets/testsearch.html");
            var testResponse = new HttpResponseMessage { Content = new StringContent(testHtml) };
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler
                .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(testResponse);
            var mockClient = new HttpClient(mockHandler.Object);
            var testSearcher = new BBCGoodFoodRecipeSearch(mockClient);

            var expectedRecipes = BBCGoodFoodRecipeSearchTestHelper.GetBBCGoodFoodRecipes();

            var realRecipes = await testSearcher.Search(testSearchString);

            Assert.Equal(expectedRecipes, realRecipes);
        }
    }
}
