using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bakerscraper.Models;

namespace Bakerscraper.Searchers
{
    public interface IRecipeSearch
    {
        public abstract Task<IEnumerable<Recipe>> Search(string searchString);
    }
}
