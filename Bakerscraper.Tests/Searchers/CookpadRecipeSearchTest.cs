using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.IO;
using Moq;
using Moq.Contrib.HttpClient;
using Xunit;
using Bakerscraper.Searchers;
using Bakerscraper.Tests.Searchers.Assets;

namespace Bakerscraper.Tests.Searchers
{
    public class CookpadRecipeSearchTest
    {
        [Theory]
        [InlineData("test", "test")]
        [InlineData("space test", "space%20test")]
        public async void CookpadSearcher_GivenString_CallsHttpClientCorrectly(string testString, string expectedString)
        {
            var expectedLink = $"https://cookpad.com/uk/search/{expectedString}?event=search.typed_query";
            var mockHandler = new Mock<HttpMessageHandler>();

            var mockClient = mockHandler.CreateClient();
            mockHandler
                .SetupAnyRequest()
                .ReturnsResponse(HttpStatusCode.OK)
                .Verifiable();

            var testSearcher = new CookpadRecipeSearch(mockClient);

            await testSearcher.Search(testString);

            mockHandler.VerifyRequest(expectedLink);
        }

        [Fact]
        public async void CookpadSearcher_GivenString_ReturnsCorrectRecipes()
        {
            var testSearchString = "cake";
            var expectedSearchUrl = "https://cookpad.com/uk/search/cake?event=search.typed_query";
            var expectedCauliflowerCakeUrl = "";
            var expectedResult = CookpadRecipeSearchTestHelper.GetCookpadRecipes();


            var mockHandler = new Mock<HttpMessageHandler>();
            var mockClient = mockHandler.CreateClient();
            mockHandler
                .SetupRequest(expectedSearchUrl)
                .ReturnsResponse(HttpStatusCode.OK, File.ReadAllText("Searchers/Assets/testcookpadsearch.html"));
            mockHandler
                .SetupRequest(expectedCauliflowerCakeUrl)
                .ReturnsResponse(HttpStatusCode.OK, File.ReadAllText("Searchers/Assets/testcauliflowercake.html"));

            var testSearcher = new CookpadRecipeSearch(mockClient);

            var actualResult = await testSearcher.Search(testSearchString);

            Assert.Equal(expectedResult, actualResult);
        }
    }
}
    
