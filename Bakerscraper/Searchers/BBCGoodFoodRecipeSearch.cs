using Bakerscraper.Models;
using Bakerscraper.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;
using System.Net.Http;
using HtmlAgilityPack;

namespace Bakerscraper.Searchers
{
    public class BBCGoodFoodRecipeSearch : IRecipeSearch
    {

        // Constants for HTML retrieval
        private static HttpClient httpClient;
        public const string baseUrl = "https://bbcgoodfood.com";

        // Constants for regex patterns
        private const string ingredientMatcherString = @"(\d*)?\s*(g|kg|ml|l|tsp|tbsp)?\s*(.*)";
        private static readonly Regex ingredientMatcher = new(ingredientMatcherString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public BBCGoodFoodRecipeSearch(HttpClient client)
        {
            httpClient = client;
        }

        public async Task<IEnumerable<Recipe>> Search(string searchString, int limit)
        {
            var recipes = new List<Recipe>();
            var recipeUris = await GetGoodFoodRecipeUris(searchString, limit);
            if (!recipeUris.Any())
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

        private async Task<IEnumerable<string>> GetGoodFoodRecipeUris(string searchString, int limit)
        {
            var e = $"/search/recipes?q={HttpUtility.UrlEncode(searchString)}";
            var response = await httpClient.GetAsync($"/search/recipes?q={HttpUtility.UrlEncode(searchString)}");
            var doc = new HtmlDocument();
            doc.Load(await response.Content.ReadAsStreamAsync());

            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//a[@class='img-container img-container--square-thumbnail']");
            if (nodes != null)
            {
                return nodes.Select(node => node.Attributes["href"].Value).Take(limit);
            }
            else
            {
                return new List<string>();
            }
        }

        private async Task<Recipe> GetRecipeFromLink(string recipeUri)
        {
            var recipe = new Recipe();

            var response = await httpClient.GetAsync(recipeUri);
            var doc = new HtmlDocument();
            doc.Load(await response.Content.ReadAsStreamAsync());

            recipe.Name = GenericRecipeSearchHelper.SanitiseString(doc.DocumentNode.SelectSingleNode("//div[contains(string(@class),'post-header__title')]//h1").InnerText);
            recipe.Steps = GetSteps(doc.DocumentNode);
            recipe.Ingredients = GetIngredients(doc.DocumentNode);
            recipe.Source = new RecipeSource
            {
                SourceType = RecipeSearchType.BBCGoodFood,
                SourceUrl = new Uri(baseUrl + recipeUri)
            };

            return recipe;
        }

        private static IEnumerable<RecipeIngredient> GetIngredients(HtmlNode documentNode)
        {
            var ingredientNodes = documentNode.SelectNodes("//section[contains(string(@class),'recipe__ingredients')]//li");
            var matches = ingredientNodes.Select(ingredientNode => ingredientMatcher.Match(ingredientNode.InnerText));
            return matches.Select(match => new RecipeIngredient
            {
                Quantity = double.TryParse(match.Groups[1].Value, out double quantity) ? quantity : null,
                Unit = GetIngredientUnitFromString(match.Groups[2].Value),
                Name = GenericRecipeSearchHelper.SanitiseString(match.Groups[3].Value)
            });
        }

        private static IEnumerable<RecipeStep> GetSteps(HtmlNode documentNode)
        {
            var stepNodes = documentNode.SelectNodes("//section[contains(string(@class), 'recipe__method-steps')]//li//p");
            return stepNodes.Select((node, index) => new RecipeStep
            {
                Number = index + 1,
                // Replace no break space with regular space, since this appears sometimes
                Text = GenericRecipeSearchHelper.NormaliseString(GenericRecipeSearchHelper.SanitiseString(node.InnerText))
            });
        }

        private static RecipeIngredientUnit GetIngredientUnitFromString(string unit)
        {
            switch (unit)
            {
                case "g":
                    return RecipeIngredientUnit.Grams;
                case "kg":
                    return RecipeIngredientUnit.Kilograms;
                case "l":
                    return RecipeIngredientUnit.Liters;
                case "ml":
                    return RecipeIngredientUnit.Milliliters;
                case "tsp":
                    return RecipeIngredientUnit.Teaspoons;
                case "tbsp":
                    return RecipeIngredientUnit.Tablespoons;
                case null:
                    return RecipeIngredientUnit.Unspecified;
                default:
                    return RecipeIngredientUnit.Unspecified;
            }
        }
    }
}
