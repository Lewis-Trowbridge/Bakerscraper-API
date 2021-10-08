using Bakerscraper.Searchers;
using Bakerscraper.Tests.Searchers.Assets;
using Moq;
using Moq.Contrib.HttpClient;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
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
            var mockClient = mockHandler.CreateClient();
            mockHandler
                .SetupAnyRequest()
                .ReturnsResponse(HttpStatusCode.OK)
                .Verifiable();
            var testSearcher = new BBCGoodFoodRecipeSearch(mockClient);

            await testSearcher.Search(testSearchString, 2);

            mockHandler.VerifyRequest(expectedLink);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(1)]
        public async void BBCGoodFoodSearcher_GivenCorrectString_ReturnsCorrectOutput(int limit)
        {
            // Load test data
            var expectedSearchUrl = "https://www.bbcgoodfood.com/search/recipes?q=test";
            var expectedMicrowaveUrl = "https://www.bbcgoodfood.com//recipes/microwave-mug-cake";
            var expectedMeltUrl = "https://www.bbcgoodfood.com//recipes/melt-middle-mug-cake";
            var expectedSearchHtml = File.ReadAllText("Searchers/Assets/testsearch.html");
            var expectedMicrowaveHtml = File.ReadAllText("Searchers/Assets/testmicrowave.html");
            var expectedMeltHtml = File.ReadAllText("Searchers/Assets/testmeltinthemiddle.html");

            // Set up mock HTTP responses
            var mockHandler = new Mock<HttpMessageHandler>();
            var mockClient = mockHandler.CreateClient();

            mockHandler
                .SetupRequest(expectedSearchUrl)
                .ReturnsResponse(HttpStatusCode.OK, expectedSearchHtml);

            mockHandler
                .SetupRequest(expectedMicrowaveUrl)
                .ReturnsResponse(HttpStatusCode.OK, expectedMicrowaveHtml);

            mockHandler
                .SetupRequest(expectedMeltUrl)
                .ReturnsResponse(expectedMeltHtml);

            var testSearcher = new BBCGoodFoodRecipeSearch(mockClient);
            var testSearchString = "test";

            var expectedRecipes = BBCGoodFoodRecipeSearchTestHelper.GetBBCGoodFoodRecipes().Take(limit);

            var realRecipes = await testSearcher.Search(testSearchString, limit);

            realRecipes.Should().BeEquivalentTo(expectedRecipes);
        }
    }
}
