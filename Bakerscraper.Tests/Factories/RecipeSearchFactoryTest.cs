using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bakerscraper.Factories;
using Bakerscraper.Enums;
using Bakerscraper.Searchers;
using Xunit;

namespace Bakerscraper.Tests.Factories
{
    public class RecipeSearchFactoryTest
    {
        [Fact]
        public void SearchFactory_GivenBBCGoodFood_ReturnsBBCGoodFoodSearcher()
        {
            RecipeSearchFactory testFactory = new RecipeSearchFactory();

            var result = testFactory.CreateSearch(RecipeSearchType.BBCGoodFood);

            Assert.IsType<BBCGoodFoodRecipeSearch>(result);
        }

        [Fact]
        public void SearchFactory_GivenCookpad_ReturnsCookpadFoodSearcher()
        {
            RecipeSearchFactory testFactory = new RecipeSearchFactory();

            var result = testFactory.CreateSearch(RecipeSearchType.Cookpad);

            Assert.IsType<CookpadRecipeSearch>(result);
        }
    }
}
