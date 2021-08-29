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
        [Fact]
        public void BBCGoodFoodSearcher_GivenString_CallsHttpClientCorrectly()
        {
            var testSearchString = "test";
            var expectedLink = "https://www.bbcgoodfood.com/search/recipes?q=test";
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler
                .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>());
            var mockClient = new HttpClient(mockHandler.Object);
            var testSearcher = new BBCGoodFoodRecipeSearch(mockClient);

            testSearcher.Search(testSearchString);
            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req => req.RequestUri.ToString() == expectedLink),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public void BBCGoodFoodSearcher_GivenCorrectString_ReturnsCorrectOutput()
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

            var realRecipes = testSearcher.Search(testSearchString);

            Assert.Equal(expectedRecipes, realRecipes);
        }
    }
}
