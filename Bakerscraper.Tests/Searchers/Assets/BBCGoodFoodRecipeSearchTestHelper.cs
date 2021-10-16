using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bakerscraper.Models;
using Bakerscraper.Enums;

namespace Bakerscraper.Tests.Searchers.Assets
{
    class BBCGoodFoodRecipeSearchTestHelper
    {
        public static List<Recipe> GetBBCGoodFoodRecipes()
        {
            return new List<Recipe>
            {
                new Recipe
                {
                    Name = "Microwave mug cake",
                    Ingredients = new List<RecipeIngredient>
                    {
                        new RecipeIngredient
                        {
                            Name = "Self-raising flour",
                            Quantity = 4,
                            Unit = RecipeIngredientUnit.Tablespoons
                        },
                        new RecipeIngredient
                        {
                            Name = "Caster sugar",
                            Quantity = 4,
                            Unit = RecipeIngredientUnit.Tablespoons
                        },
                        new RecipeIngredient
                        {
                            Name = "Cocoa powder",
                            Quantity = 2,
                            Unit = RecipeIngredientUnit.Tablespoons
                        },
                        new RecipeIngredient
                        {
                            Name = "Medium egg",
                            Quantity = 1,
                            Unit = RecipeIngredientUnit.Unspecified
                        },
                        new RecipeIngredient
                        {
                            Name = "Milk",
                            Quantity = 3,
                            Unit = RecipeIngredientUnit.Tablespoons
                        },
                        new RecipeIngredient
                        {
                            Name = "Vegetable oil or sunflower oil",
                            Quantity = 3,
                            Unit = RecipeIngredientUnit.Tablespoons
                        },
                        new RecipeIngredient
                        {
                            Name = "A few drops of vanilla essence or other essence (orange or peppermint work well)",
                            Quantity = null,
                            Unit = RecipeIngredientUnit.Unspecified
                        },
                        new RecipeIngredient
                        {
                            Name = "Chocolate chips, nuts, or raisins etc (optional)",
                            Quantity = 2,
                            Unit = RecipeIngredientUnit.Tablespoons
                        }
                    },
                    Steps = new List<RecipeStep>
                    {
                        new RecipeStep
                        {
                            Number = 1,
                            Text = "Add 4 tbsp self-raising flour, 4 tbsp caster sugar and 2 tbsp cocoa powder to the largest mug you have (to stop it overflowing in the microwave) and mix."
                        },
                        new RecipeStep
                        {
                            Number = 2,
                            Text = "Add 1 medium egg and mix in as much as you can, but don't worry if there's still dry mix left."
                        },
                        new RecipeStep
                        {
                            Number = 3,
                            Text = "Add the 3 tbsp milk, 3 tbsp vegetable or sunflower oil and a few drops of vanilla essence and mix until smooth, before adding 2 tbsp chocolate chips, nuts, or raisins, if using, and mix again."
                        },
                        new RecipeStep
                        {
                            Number = 4,
                            Text = "Centre your mug in the middle of the microwave oven and cook on High for 1½ -2 mins, or until it has stopped rising and is firm to the touch."
                        }
                    },
                    Source = new RecipeSource
                    {
                        SourceType = RecipeSearchType.BBCGoodFood,
                        SourceUrl = new Uri("https://bbcgoodfood.com/recipes/microwave-mug-cake")
                    },
                },
                new Recipe
                {
                    Name = "Melt-in-the-middle mug cake",
                    Ingredients = new List<RecipeIngredient>
                    {
                        new RecipeIngredient
                        {
                            Name = "Sunflower oil",
                            Quantity = 2,
                            Unit = RecipeIngredientUnit.Tablespoons
                        },
                        new RecipeIngredient
                        {
                            Name = "Medium egg",
                            Quantity = 1,
                            Unit = RecipeIngredientUnit.Unspecified
                        },
                        new RecipeIngredient
                        {
                            Name = "Plain flour",
                            Quantity = 2,
                            Unit = RecipeIngredientUnit.Tablespoons
                        },
                        new RecipeIngredient
                        {
                            Name = "Light brown sugar",
                            Quantity = 3,
                            Unit = RecipeIngredientUnit.Tablespoons
                        },
                        new RecipeIngredient
                        {
                            Name = "Cocoa powder",
                            Quantity = 1,
                            Unit = RecipeIngredientUnit.Tablespoons
                        },
                        new RecipeIngredient
                        {
                            Name = "Dash of vanilla extract",
                            Quantity = null,
                            Unit = RecipeIngredientUnit.Unspecified
                        },
                        new RecipeIngredient
                        {
                            Name = "Boiling water",
                            Quantity = 1,
                            Unit = RecipeIngredientUnit.Tablespoons
                        },
                        new RecipeIngredient
                        {
                            Name = "Soft chocolate truffle of your choice (we used Lindt Lindor)",
                            Quantity = 1,
                            Unit = RecipeIngredientUnit.Unspecified
                        },
                        new RecipeIngredient
                        {
                            Name = "Ice cream or cream, to serve (optional)",
                            Quantity = null,
                            Unit = RecipeIngredientUnit.Unspecified
                        }
                    },
                    Steps = new List<RecipeStep>
                    {
                        new RecipeStep
                        {
                            Number = 1,
                            Text = "Using a fork, whisk all the ingredients, aside from the chocolate truffle, with a pinch of salt in a large mug (ours was 350ml)."
                        },
                        new RecipeStep
                        {
                            Number = 2,
                            Text = "Push the chocolate into the centre of the batter, and microwave the cake on high for 45 secs until cooked on the outside with a liquid molten centre. Serve hot with ice cream or cream, if you like."
                        }
                    },
                    Source = new RecipeSource
                    {
                        SourceType = RecipeSearchType.BBCGoodFood,
                        SourceUrl = new Uri("https://bbcgoodfood.com/recipes/melt-middle-mug-cake")
                    },
                }
            };
        }
    }
}
