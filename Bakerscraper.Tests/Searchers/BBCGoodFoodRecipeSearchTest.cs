using Bakerscraper.Searchers;
using Bakerscraper.Tests.Searchers.Assets;
using Moq;
using Moq.Protected;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

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
            // Load test data
            var testSearchRequest = new HttpRequestMessage(HttpMethod.Get, "https://www.bbcgoodfood.com/search/recipes?q=test");
            var testMicrowaveRequest = new HttpRequestMessage(HttpMethod.Get, "https://www.bbcgoodfood.com//recipes/microwave-mug-cake");
            var testMeltRequest = new HttpRequestMessage(HttpMethod.Get, "https://www.bbcgoodfood.com//recipes/melt-middle-mug-cake");
            var testsearchHtml = File.ReadAllText("Searchers/Assets/testsearch.html");
            var testMicrowaveHtml = File.ReadAllText("Searchers/Assets/testmicrowave.html");
            var testMeltHtml = File.ReadAllText("Searchers/Assets/testmeltinthemiddle.html");
            var testSearchResponse = new HttpResponseMessage { Content = new StringContent(testsearchHtml) };
            var testMicrowaveResponse = new HttpResponseMessage { Content = new StringContent(testMicrowaveHtml) };
            var testMeltResponse = new HttpResponseMessage { Content = new StringContent(testMeltHtml) };

            // Set up mock HTTP responses
            var mockHandler = new Mock<HttpMessageHandler>();

            mockHandler
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.Is<HttpRequestMessage>(request => request.RequestUri == testSearchRequest.RequestUri),
                  ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(testSearchResponse);

            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.Is<HttpRequestMessage>(request => request.RequestUri == testMicrowaveRequest.RequestUri),
                  ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(testMicrowaveResponse);

            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.Is<HttpRequestMessage>(request => request.RequestUri == testMeltRequest.RequestUri),
                  ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(testMeltResponse);

            var mockClient = new HttpClient(mockHandler.Object);
            var testSearcher = new BBCGoodFoodRecipeSearch(mockClient);
            var testSearchString = "test";

            var expectedRecipes = BBCGoodFoodRecipeSearchTestHelper.GetBBCGoodFoodRecipes();

            var realRecipes = await testSearcher.Search(testSearchString);

            realRecipes.Should().BeEquivalentTo(expectedRecipes);
        }
    }
}
