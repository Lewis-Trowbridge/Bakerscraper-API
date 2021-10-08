using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Bakerscraper.Factories;
using Bakerscraper.Enums;
using Bakerscraper.Searchers;
using Xunit;
using Moq;

namespace Bakerscraper.Tests.Factories
{
    public class RecipeSearchFactoryTest
    {

        private readonly Mock<IHttpClientFactory> clientFactoryMock;

        public RecipeSearchFactoryTest()
        {
            clientFactoryMock = new Mock<IHttpClientFactory>();
        }

        [Fact]
        public void SearchFactory_GivenBBCGoodFood_ReturnsBBCGoodFoodSearcher()
        {
            RecipeSearchFactory testFactory = new RecipeSearchFactory();

            var result = testFactory.CreateSearch(RecipeSearchType.BBCGoodFood, clientFactoryMock.Object);

            Assert.IsType<BBCGoodFoodRecipeSearch>(result);
        }

        [Fact]
        public void SearchFactory_GivenCookpad_ReturnsCookpadSearcher()
        {
            RecipeSearchFactory testFactory = new RecipeSearchFactory();

            var result = testFactory.CreateSearch(RecipeSearchType.Cookpad, clientFactoryMock.Object);

            Assert.IsType<CookpadRecipeSearch>(result);
        }
    }
}
