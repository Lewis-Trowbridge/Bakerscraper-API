using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Bakerscraper.Models;
using Bakerscraper.Searchers;

namespace Bakerscraper.Searchers
{
    public class CookpadRecipeSearch : IRecipeSearch
    {

        // Constants for HTML retrieval
        private HttpClient httpClient;
        private const string baseUrl = "https://www.bbcgoodfood.com/";

        public CookpadRecipeSearch()
        {
            this.httpClient = new HttpClient();
        }

        public CookpadRecipeSearch(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<Recipe>> Search(string searchString)
        {
            var recipes = new List<Recipe>();
            return recipes;
        }
    }
}
