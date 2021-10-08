using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bakerscraper.Searchers;
using Bakerscraper.Factories;
using Bakerscraper.Enums;
using System.Net.Http;

namespace Bakerscraper.Factories
{
    public class RecipeSearchFactory : IRecipeSearchFactory
    {
        public IRecipeSearch CreateSearch(RecipeSearchType searchType, IHttpClientFactory clientFactory)
        {
            return searchType switch
            {
                RecipeSearchType.BBCGoodFood => new BBCGoodFoodRecipeSearch(),
                RecipeSearchType.Cookpad => new CookpadRecipeSearch(clientFactory.CreateClient(BakerscraperHttpClientFactory.Cookpad)),
                _ => throw new TypeLoadException(),
            };
        }
    }
}
