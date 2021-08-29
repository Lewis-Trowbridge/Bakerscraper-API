using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bakerscraper.Models
{
    public class Recipe
    {
        public string Name { get; set; }
        public List<RecipeIngredient> Ingredients { get; set; }
        public List<RecipeStep> Steps { get; set; }
    }
}
