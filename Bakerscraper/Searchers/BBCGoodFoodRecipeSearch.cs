using Bakerscraper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using System.Net.Http;
using HtmlAgilityPack;
using VDS.RDF;
using VDS.RDF.Parsing;

namespace Bakerscraper.Searchers
{
    public class BBCGoodFoodRecipeSearch : IRecipeSearch
    {

        private HttpClient httpClient;
        private const string baseUrl = "https://www.bbcgoodfood.com/";
        private readonly Uri schemaNameUri = new("http://schema.org/name");
        private readonly Uri schemaRecipeIngredientUri = new("http://schema.org/recipeIngredient");
        private readonly Uri schemaRecipeInstructionsUri = new("http://schema.org/recipeInstructions");

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
            var recipes = new List<Recipe>();
            var searchHTML = await GetGoodFoodSearchHTML(searchString);
            var recipeUris = GetGoodFoodRecipeUris(searchHTML);
            if (recipeUris.Count() == 0)
            {
                return recipes;
            }
            var recipeTasks = new List<Task<Recipe>>();
            foreach (var recipeUri in recipeUris)
            {
                recipeTasks.Add(GetRecipeFromLink(recipeUri));
            }
            Task.WaitAll(recipeTasks.ToArray());
            foreach (var recipeTask in recipeTasks)
            {
                recipes.Add(recipeTask.Result);
            }
            return recipes;
        }

        private async Task<string> GetGoodFoodSearchHTML(string searchString)
        {
            var response = await httpClient.GetAsync(baseUrl + "search/recipes?q=" + HttpUtility.UrlEncode(searchString));
            return await response.Content.ReadAsStringAsync();
        }

        private IEnumerable<Uri> GetGoodFoodRecipeUris(string searchHTML)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(searchHTML);

            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//a[@class='img-container img-container--square-thumbnail']");
            if (nodes != null)
            {
                return nodes.Select(node => new Uri(baseUrl + node.Attributes["href"].Value));
            }
            else
            {
                return new List<Uri>();
            }
        }

        private async Task<Recipe> GetRecipeFromLink(Uri recipeUri)
        {
            var recipe = new Recipe();

            var response = await httpClient.GetAsync(recipeUri);
            var doc = new HtmlDocument();
            doc.LoadHtml(await response.Content.ReadAsStringAsync());

            var jsonLdString = doc.DocumentNode.SelectSingleNode("//script[@type='application/ld+json']").InnerText;

            var jsonLdProcessor = new JsonLdParser();
            var recipeTripleStore = new ThreadSafeTripleStore();

            jsonLdProcessor.Load(recipeTripleStore, new StringReader(jsonLdString));

            var nameNode = (LiteralNode)recipeTripleStore.GetTriplesWithPredicate(schemaNameUri).First().Object;
            recipe.Name = nameNode.Value;

            return recipe;
        }

    }
}
