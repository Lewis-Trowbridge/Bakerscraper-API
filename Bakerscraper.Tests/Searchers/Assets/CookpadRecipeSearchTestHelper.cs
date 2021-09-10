using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bakerscraper.Models;
using Bakerscraper.Enums;

namespace Bakerscraper.Tests.Searchers.Assets
{
    class CookpadRecipeSearchTestHelper
    {
        public static List<Recipe> GetCookpadRecipes()
        {
            return new List<Recipe>
            {
                new Recipe
                {
                    Name = "Cauliflower cake",
                    Ingredients = new List<RecipeIngredient>
                    {
                        new RecipeIngredient
                        {
                            Name = "Medium red onion, peeled",
                            Quantity = 1,
                            Unit = RecipeIngredientUnit.Unspecified
                        },
                        new RecipeIngredient
                        {
                            Name = "Olive oil",
                            Quantity = 30,
                            Unit = RecipeIngredientUnit.Milliliters
                        },
                        new RecipeIngredient
                        {
                            Name = "Hot chilli flakes (optional)",
                            Quantity = 0.33,
                            Unit = RecipeIngredientUnit.Teaspoons
                        },
                        new RecipeIngredient
                        {
                            Name = "Melted unsalted butter, for brushing (can also use olive oil spray)",
                            Quantity = null,
                            Unit = RecipeIngredientUnit.Unspecified
                        },
                        new RecipeIngredient
                        {
                            Name = "Salt and black pepper",
                            Quantity = null,
                            Unit = RecipeIngredientUnit.Unspecified
                        },
                        new RecipeIngredient
                        {
                            Name = "Baking powder",
                            Quantity = 1.5,
                            Unit = RecipeIngredientUnit.Teaspoons
                        },
                        new RecipeIngredient
                        {
                            Name = "Basil leaves, chopped",
                            Quantity = 0.5,
                            Unit = RecipeIngredientUnit.Cups
                        },
                        new RecipeIngredient
                        {
                            Name = "All-purpose flour, sifted",
                            Quantity = 200,
                            Unit = RecipeIngredientUnit.Grams
                        }
                    },
                    Steps = new List<RecipeStep>
                    {
                        new RecipeStep
                        {
                            Number = 1,
                            Text = "Preheat the oven to 180ºC",
                        },
                        new RecipeStep
                        {
                            Number = 2,
                            Text = "Transfer the onion to a large bowl, add the eggs and basil, whisk well, and then add the flour, baking powder, turmeric, parmesan, 1/2 tsp salt and plenty of pepper.",
                        },
                        new RecipeStep
                        {
                            Number = 3,
                            Text = "It needs to be served just warm, rather that hot or room temperature. Enjoy."
                        }
                    },
                    Source = RecipeSearchType.Cookpad
                }
            };
        }
    }
}
