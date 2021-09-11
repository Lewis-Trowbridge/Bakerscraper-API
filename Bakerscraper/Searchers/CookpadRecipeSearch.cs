using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Text;
using System.Threading.Tasks;
using Bakerscraper.Models;

namespace Bakerscraper.Searchers
{
    public class CookpadRecipeSearch : IRecipeSearch
    {

        // Constants for HTML retrieval
        private HttpClient httpClient;
        private const string baseUrl = "https://cookpad.com/uk";

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
            var recipeUris = await GetRecipeUris(searchString);
            var recipes = new List<Recipe>();
            return recipes;
        }

        private async Task<List<Uri>> GetRecipeUris(string searchString)
        {
            var searchUrl = $"{baseUrl}/search/{Uri.EscapeDataString(searchString)}?event=search.typed_query";
            var response = await httpClient.GetAsync(searchUrl);
            return new List<Uri>();
        }
    }
}
