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
using VDS.RDF;
using VDS.RDF.Parsing;
using Soltys.ChangeCase;

namespace Bakerscraper.Searchers
{
    public class BBCGoodFoodRecipeSearch : IRecipeSearch
    {

        // Constants for HTML retrieval
        private HttpClient httpClient;
        public const string baseUrl = "https://www.bbcgoodfood.com/";

        // Constants for RDF traversal/filtering
        private readonly Uri schemaNameUri = new("http://schema.org/name");
        private readonly Uri schemaTextUri = new("http://schema.org/text");
        private readonly Uri schemaRecipeIngredientUri = new("http://schema.org/recipeIngredient");
        private readonly Uri schemaRecipeInstructionsUri = new("http://schema.org/recipeInstructions");

        // Constants for regex patterns
        private const string ingredientMatcherString = @"(\d*)?\s*(g|kg|ml|l|tsp|tbsp)?\s*(.*)";
        private readonly Regex ingredientMatcher = new(ingredientMatcherString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public BBCGoodFoodRecipeSearch(HttpClient client)
        {
            httpClient = client;
        }

        public async Task<IEnumerable<Recipe>> Search(string searchString, int limit)
        {
            var recipes = new List<Recipe>();
            var recipeUris = await GetGoodFoodRecipeUris(searchString, limit);
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

        private async Task<IEnumerable<Uri>> GetGoodFoodRecipeUris(string searchString, int limit)
        {
            var response = await httpClient.GetAsync(baseUrl + "search/recipes?q=" + HttpUtility.UrlEncode(searchString));
            var doc = new HtmlDocument();
            doc.Load(await response.Content.ReadAsStreamAsync());

            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//a[@class='img-container img-container--square-thumbnail']");
            if (nodes != null)
            {
                return nodes.Select(node => new Uri(baseUrl + node.Attributes["href"].Value)).Take(limit);
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
            recipe.Steps = GetStepsFromTripleStore(recipeTripleStore).ToList();
            recipe.Ingredients = GetIngredientsFromTripleStore(recipeTripleStore).ToList();
            recipe.Source = RecipeSearchType.BBCGoodFood;

            return recipe;
        }

        private IEnumerable<RecipeIngredient> GetIngredientsFromTripleStore(TripleStore tripleStore)
        {
            var ingredients = new List<RecipeIngredient>();
            var ingredientTriples = tripleStore.GetTriplesWithPredicate(schemaRecipeIngredientUri);
            foreach (var ingredientTriple in ingredientTriples)
            {
                var ingredientString = ((LiteralNode)ingredientTriple.Object).Value;
                var matchCollection = ingredientMatcher.Match(ingredientString);
                int? quantity = null;
                try
                {
                    quantity = Int32.Parse(matchCollection.Groups[1].Value);
                }
                catch (FormatException)
                {
                    // Do nothing, as quantity is already null
                }
                ingredients.Add(new RecipeIngredient
                {
                    Quantity = quantity,
                    Unit = GetIngredientUnitFromString(matchCollection.Groups[2].Value),
                    Name = GenericRecipeSearchHelper.SanitiseString(matchCollection.Groups[3].Value)

                });
            }
            return ingredients;
        }

        private IEnumerable<RecipeStep> GetStepsFromTripleStore(TripleStore tripleStore)
        {
            var factory = new NodeFactory();
            var instructionTriples = tripleStore.GetTriplesWithPredicate(schemaRecipeInstructionsUri);
            var steps = new List<RecipeStep>();
            var count = 1;
            var doc = new HtmlDocument();
            foreach (var instructionTriple in instructionTriples)
            {
                var textNode = tripleStore.GetTriplesWithSubjectPredicate(instructionTriple.Object, factory.CreateUriNode(schemaTextUri)).First();
                var textHtml = ((LiteralNode)textNode.Object).Value;
                doc.LoadHtml(textHtml);
                steps.Add(new RecipeStep
                {
                    Number = count,
                    // Replace no break space with regular space, since this appears sometimes
                    Text = doc.DocumentNode.InnerText.Replace("\u00A0", " ")
                });
                count++;
            }
            return steps;
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
