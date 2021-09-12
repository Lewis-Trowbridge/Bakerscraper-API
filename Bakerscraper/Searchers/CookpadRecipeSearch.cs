using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Bakerscraper.Models;

namespace Bakerscraper.Searchers
{
    public class CookpadRecipeSearch : IRecipeSearch
    {

        // Constants for HTML retrieval
        private HttpClient httpClient;
        private const string baseUrl = "https://cookpad.com";

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
            var recipeUris = await GetRecipeUris(searchString);
            if (!recipeUris.Any())
            {
                return recipes;
            }
            return recipes;
        }

        private async Task<IEnumerable<Uri>> GetRecipeUris(string searchString)
        {
            var searchUrl = $"{baseUrl}/uk/search/{Uri.EscapeDataString(searchString)}?event=search.typed_query";
            var response = await httpClient.GetAsync(searchUrl);
            var doc = new HtmlDocument();
            doc.Load(await response.Content.ReadAsStreamAsync());

            var recipeUriNodes = doc.DocumentNode.SelectNodes("//a[@class='block-link__main']");

            if (recipeUriNodes != null && recipeUriNodes.Any())
            {
                return recipeUriNodes.Select(recipeUriNode => new Uri(baseUrl + recipeUriNode.Attributes["href"].Value));
            }
            else
            {
                return new List<Uri>();
            }
        }
    }
}
