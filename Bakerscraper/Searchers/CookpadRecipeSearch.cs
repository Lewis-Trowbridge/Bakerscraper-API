using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Bakerscraper.Models;
using Bakerscraper.Enums;

namespace Bakerscraper.Searchers
{
    public class CookpadRecipeSearch : IRecipeSearch
    {

        // Constants for HTML retrieval
        private HttpClient httpClient;
        private const string baseUrl = "https://cookpad.com";

        private const string ingredientRegex = @"(\d*[\/]?\d*)\s?(gram[s]?|g|kilogram[s]?|kg|milliliter[s]?|ml|liter[s]?|l|teaspoon[s]?|tsp|tablespoon[s]?|tbsp|cup[s]?|)\s*(.*)";
        private readonly Regex ingredientMatcher = new(ingredientRegex, RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public CookpadRecipeSearch()
        {
            this.httpClient = new HttpClient();
        }

        public CookpadRecipeSearch(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IEnumerable<Recipe>> Search(string searchString)
        {
            var recipes = new List<Recipe>();
            var recipeUris = await GetRecipeUris(searchString);
            if (!recipeUris.Any())
            {
                return recipes;
            }
            var recipeTasks = recipeUris.Select(uri => GetRecipeFromUri(uri));
            Task.WaitAll(recipeTasks.ToArray());

            return recipeTasks.Select(task => task.Result);
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

        private async Task<Recipe> GetRecipeFromUri(Uri recipeUri)
        {
            var recipe = new Recipe();
            var recipeResponse = await httpClient.GetAsync(recipeUri);
            var doc = new HtmlDocument();
            doc.Load(await recipeResponse.Content.ReadAsStreamAsync());

            recipe.Name = GenericRecipeSearchHelper.SanitiseString(doc.DocumentNode.SelectSingleNode("//h1[@itemprop='name']").InnerText);
            recipe.Source = RecipeSearchType.Cookpad;

            recipe.Ingredients = GetRecipeIngredients(doc.GetElementbyId("ingredients"));
            recipe.Steps = GetRecipeSteps(doc.GetElementbyId("steps"));

            return recipe;
        }

        private IEnumerable<RecipeIngredient> GetRecipeIngredients(HtmlNode ingredientContainer)
        {
            var ingredientNodes = ingredientContainer.SelectNodes("//div[@itemprop='ingredients']");
            var matches = ingredientNodes.Select(node => ingredientMatcher.Match(node.InnerText.Trim()));
            return matches.Select(match => GetRecipeIngredientsFromMatchCollection(match));
        }

        private RecipeIngredient GetRecipeIngredientsFromMatchCollection(Match collection)
        {
            return new RecipeIngredient
            {
                Name = GenericRecipeSearchHelper.SanitiseString(collection.Groups[3].Value),
                Quantity = SimpleConvertStringToDouble(collection.Groups[1].Value),
                Unit = GetIngredientUnitFromString(collection.Groups[2].Value)
            };
        }

        private IEnumerable<RecipeStep> GetRecipeSteps(HtmlNode stepNode)
        {
            return new List<RecipeStep>();
        }

        private static RecipeIngredientUnit GetIngredientUnitFromString(string unit)
        {
            return unit switch
            {
                "g" or "gram" or "grams" => RecipeIngredientUnit.Grams,
                "kg" or "kilogram" or "kilograms" => RecipeIngredientUnit.Kilograms,
                "l" or "liter" or "liters" => RecipeIngredientUnit.Liters,
                "ml" or "millilter" or "milliliters" => RecipeIngredientUnit.Milliliters,
                "tsp" or "teaspoon" or "teaspoons" => RecipeIngredientUnit.Teaspoons,
                "tbsp" or "tablespoon" or "tablespoons" => RecipeIngredientUnit.Tablespoons,
                _ => RecipeIngredientUnit.Unspecified,
            };
        }

        // Not intended as an exhaustive conversion of every fraction but as an easy and quick conversion of commonly-used fractions
        private static double? SimpleConvertStringToDouble(string doubleString)
        {
            return doubleString switch
            {
                "1/2" => 0.5,
                "1/3" => 0.33,
                "1/4" => 0.25,
                "1/8" => 0.125,
                _ => double.TryParse(doubleString, out double quantity) ? quantity : null
            };
        }
    }
}
