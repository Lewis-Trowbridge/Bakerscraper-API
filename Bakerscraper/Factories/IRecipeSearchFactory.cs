using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bakerscraper.Searchers;
using Bakerscraper.Enums;

namespace Bakerscraper.Factories
{
    public interface IRecipeSearchFactory
    {
        public IRecipeSearch CreateSearch(RecipeSearchType searchType);
    }
}
