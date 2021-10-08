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
                    Name = "Cauliflower Cake",
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
                            Quantity = 0.5,
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
                },
                new Recipe
                {
                    Name = "My Rhubarb, Ginger & Pistachio Cake",
                    Ingredients = new List<RecipeIngredient>
                    {
                        new RecipeIngredient
                        {
                            Name = "Demerara sugar",
                            Quantity = 2,
                            Unit = RecipeIngredientUnit.Tablespoons
                        },
                        new RecipeIngredient
                        {
                            Name = "Handful shelled pistachios, crushed into crumbs",
                            Quantity = 1,
                            Unit = RecipeIngredientUnit.Unspecified
                        },
                        new RecipeIngredient
                        {
                            Name = "Extra butter for greasing",
                            Quantity = null,
                            Unit = RecipeIngredientUnit.Unspecified
                        }
                    },
                    Steps = new List<RecipeStep>
                    {
                        new RecipeStep
                        {
                            Number = 1,
                            Text = "Heat your oven to 180 (fan). Grease a circular cake tin all over with some softened butter."
                        },
                        new RecipeStep
                        {
                            Number = 2,
                            Text = "Add all of the dry ingredients into a large mixing bowl and fold together gently. In a separate bowl combine the wet ingredients (eggs, milk and cooled butter), mix together then add them in with the dry ingredients. Combine."
                        },
                        new RecipeStep
                        {
                            Number = 3,
                            Text = "Pour into your greased cake tin and using a spatula smooth it down evenly on top, then gently lay the pieces of rhubarb on top in your desired pattern (see picture below for my example), don't push them down into the batter, lay them right on top as they'll sink a bit during baking. Sprinkle the top with sugar all over."
                        },
                        new RecipeStep
                        {
                            Number = 4,
                            Text = "Bake in the oven for approx 45-50 mins or until a toothpick comes out from the centre clean. Leave to cool for a few minutes then remove carefully from the springform tin and leave to cool upon a wire rack. Once completely cooled, you can carefully lift it off the metal base. Sprinkle some pistachio nuts over the top, concentrating more in the centre. Serve up and enjoy! :)"
                        }
                    },
                    Source = RecipeSearchType.Cookpad
                }
            };
        }
    }
}
