using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bakerscraper.Models
{
    public record Recipe
    {
        public string Name { get; set; }
        public IEnumerable<RecipeIngredient> Ingredients { get; set; }
        public IEnumerable<RecipeStep> Steps { get; set; }
        public RecipeSource Source { get; set; }
    }
}
