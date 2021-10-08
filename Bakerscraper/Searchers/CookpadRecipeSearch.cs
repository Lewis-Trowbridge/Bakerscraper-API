using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Bakerscraper.Models;
using Bakerscraper.Enums;

namespace Bakerscraper.Searchers
{
    public class CookpadRecipeSearch : IRecipeSearch
    {

        // Constants for HTML retrieval
        private readonly HttpClient httpClient;
        public const string baseUrl = "https://cookpad.com";

        private const string ingredientRegex = @"(\d*[\/]?\d*)\s?(gram[s]?|g|kilogram[s]?|kg|milliliter[s]?|ml|liter[s]?|l|teaspoon[s]?|tsp|tablespoon[s]?|tbsp|cup[s]?|)\s*(.*)";
        private readonly Regex ingredientMatcher = new(ingredientRegex, RegexOptions.Compiled | RegexOptions.IgnoreCase);

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
            var recipeTasks = new List<Task<Recipe>>();
            foreach (var uri in recipeUris)
            {
                recipeTasks.Add(GetRecipeFromUri(uri));
            }
            Task.WaitAll(recipeTasks.ToArray());

            return recipeTasks.Select(task => task.Result);
        }

        private async Task<IEnumerable<string>> GetRecipeUris(string searchString)
        {
            var searchUrl = $"/uk/search/{Uri.EscapeDataString(searchString)}?event=search.typed_query";
            var response = await httpClient.GetAsync(searchUrl);
            if (response.IsSuccessStatusCode)
            {
                var doc = new HtmlDocument();
                doc.Load(await response.Content.ReadAsStreamAsync());

                var recipeUriNodes = doc.DocumentNode.SelectNodes("//a[@class='block-link__main']");

                if (recipeUriNodes != null && recipeUriNodes.Any())
                {
                    return recipeUriNodes.Select(recipeUriNode => recipeUriNode.Attributes["href"].Value);
                }
            }
            return new List<string>();
        }

        private async Task<Recipe> GetRecipeFromUri(string recipeUri)
        {
            var recipe = new Recipe();
            var recipeResponse = await httpClient.GetAsync(recipeUri);
            if (recipeResponse.IsSuccessStatusCode)
            {
                var doc = new HtmlDocument();
                doc.Load(await recipeResponse.Content.ReadAsStreamAsync());

                recipe.Name = GenericRecipeSearchHelper.SanitiseString(doc.DocumentNode.SelectSingleNode("//h1[@itemprop='name']").InnerText);
                recipe.Source = RecipeSearchType.Cookpad;

                recipe.Ingredients = GetRecipeIngredients(doc.GetElementbyId("ingredients"));
                recipe.Steps = GetRecipeSteps(doc.GetElementbyId("steps"));
            }

            return recipe;
        }

        private IEnumerable<RecipeIngredient> GetRecipeIngredients(HtmlNode ingredientContainer)
        {
            var ingredientNodes = ingredientContainer.SelectNodes("//div[@itemprop='ingredients']");
            var matches = ingredientNodes.Select(node => ingredientMatcher.Match(node.InnerText.Trim()));
            return matches.Select(match => GetRecipeIngredientsFromMatch(match));
        }

        private RecipeIngredient GetRecipeIngredientsFromMatch(Match ingredientMatch)
        {
            return new RecipeIngredient
            {
                Name = GenericRecipeSearchHelper.SanitiseString(ingredientMatch.Groups[3].Value),
                Quantity = SimpleConvertStringToDouble(ingredientMatch.Groups[1].Value),
                Unit = GetIngredientUnitFromString(ingredientMatch.Groups[2].Value)
            };
        }

        private IEnumerable<RecipeStep> GetRecipeSteps(HtmlNode stepContainer)
        {
            var stepNodes = stepContainer.SelectNodes("//div[@itemprop='recipeInstructions']");
            return stepNodes.Select((node, index) => new RecipeStep { Number = index, Text = GenericRecipeSearchHelper.SanitiseString(node.InnerText) });
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
