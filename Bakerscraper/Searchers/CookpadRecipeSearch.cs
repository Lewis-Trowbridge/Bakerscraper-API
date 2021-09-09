using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bakerscraper.Models;
using Bakerscraper.Searchers;

namespace Bakerscraper.Searchers
{
    public class CookpadRecipeSearch : IRecipeSearch
    {
        public Task<List<Recipe>> Search(string searchString)
        {
            throw new NotImplementedException();
        }
    }
}
