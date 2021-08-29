using Bakerscraper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Net.Http;

namespace Bakerscraper.Searchers
{
    public class BBCGoodFoodRecipeSearch : IRecipeSearch
    {

        private HttpClient httpClient;
        private const string baseUrl = "https://www.bbcgoodfood.com/";

        public BBCGoodFoodRecipeSearch()
        {
            httpClient = new HttpClient();
        }

        public BBCGoodFoodRecipeSearch(HttpClient client)
        {
            httpClient = client;
        }

        public async Task<List<Recipe>> Search(string searchString)
        {
            var searchHTML = await GetGoodFoodHTML(searchString);
            return new List<Recipe>();
        }

        private async Task<string> GetGoodFoodHTML(string searchString)
        {
            var response = await httpClient.GetAsync(baseUrl + "search/recipes?q=" + HttpUtility.UrlEncode(searchString));
            return await response.Content.ReadAsStringAsync();
        }

    }
}
