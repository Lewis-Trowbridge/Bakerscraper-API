using Bakerscraper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

namespace Bakerscraper.Searchers
{
    public class BBCGoodFoodRecipeSearch : IRecipeSearch
    {

        private HttpClient httpClient;

        public BBCGoodFoodRecipeSearch()
        {
            httpClient = new HttpClient();
        }

        public BBCGoodFoodRecipeSearch(HttpClient client)
        {
            httpClient = client;
        }

        public List<Recipe> Search(string searchString)
        {
            throw new NotImplementedException();
        }
    }
}
