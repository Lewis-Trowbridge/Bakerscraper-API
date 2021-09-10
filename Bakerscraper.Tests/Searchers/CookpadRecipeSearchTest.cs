using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using Moq;
using Moq.Contrib.HttpClient;
using Xunit;
using Bakerscraper.Searchers;

namespace Bakerscraper.Tests.Searchers
{
    public class CookpadRecipeSearchTest
    {
        [Theory]
        [InlineData("test", "test")]
        [InlineData("space test", "space test")]
        public async void CookpadSearcher_GivenString_CallsHttpClientCorrectly(string testString, string expectedString)
        {
            string expectedLink = $"https://cookpad.com/uk/search/{expectedString}?event=search.typed_query";
            var mockHandler = new Mock<HttpMessageHandler>();

            var mockClient = mockHandler.CreateClient();
            mockHandler.SetupAnyRequest().Verifiable();

            var testSearcher = new CookpadRecipeSearch(mockClient);

            await testSearcher.Search(testString);

            mockHandler.VerifyRequest(expectedLink);
        }
    }
}
    
