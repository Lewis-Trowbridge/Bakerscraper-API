using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Bakerscraper.Factories;
using Bakerscraper.Searchers;
using Xunit;
using FluentAssertions;

namespace Bakerscraper.Tests.Factories
{
    public class BakerscraperHttpClientFactoryTest
    {

        private readonly BakerscraperHttpClientFactory clientFactory;

        public BakerscraperHttpClientFactoryTest()
        {
            clientFactory = new BakerscraperHttpClientFactory();
        }

        [Fact]
        public void BakerscraperHttpClientFactory_GivenBBCGoodFood_ProducesHttpClientWithBaseAddress()
        {
            var actualClient = clientFactory.CreateClient(BakerscraperHttpClientFactory.BBCGoodFood);

            Assert.Equal(new Uri("https://bbcgoodfood.com/"), actualClient.BaseAddress);
        }

        [Fact]
        public void BakerscraperHttpClientFactory_GivenCookpad_ProducesHttpClientWithBaseAddress()
        {
            var actualClient = clientFactory.CreateClient(BakerscraperHttpClientFactory.Cookpad);

            Assert.Equal(new Uri("https://cookpad.com"), actualClient.BaseAddress);
        }

        [Fact]
        public void BakerscraperHttpClientFactory_GivenCookpad_ProducesHttpClientWithHost()
        {
            var actualClient = clientFactory.CreateClient(BakerscraperHttpClientFactory.Cookpad);

            Assert.Equal("cookpad.com", actualClient.DefaultRequestHeaders.Host);
        }

        [Fact]
        public void BakerscraperHttpClientFactory_GivenCookpad_ProducesHttpClientWithUserAgent()
        {
            var actualClient = clientFactory.CreateClient(BakerscraperHttpClientFactory.Cookpad);

            Assert.Contains(new ProductInfoHeaderValue("Bakerscraper-API", "1.0"), actualClient.DefaultRequestHeaders.UserAgent);
        }
    }
}
