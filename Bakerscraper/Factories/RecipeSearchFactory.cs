using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bakerscraper.Searchers;
using Bakerscraper.Factories;
using Bakerscraper.Enums;

namespace Bakerscraper.Factories
{
    public class RecipeSearchFactory : IRecipeSearchFactory
    {
        public IRecipeSearch CreateSearch(RecipeSearchType searchType)
        {
            return searchType switch
            {
                RecipeSearchType.BBCGoodFood => new BBCGoodFoodRecipeSearch(),
                RecipeSearchType.Cookpad => new CookpadRecipeSearch(),
                _ => throw new TypeLoadException(),
            };
        }
    }
}
