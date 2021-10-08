using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bakerscraper.Enums;

namespace Bakerscraper.Models
{
    public record Recipe
    {
        public string Name { get; set; }
        public IEnumerable<RecipeIngredient> Ingredients { get; set; }
        public IEnumerable<RecipeStep> Steps { get; set; }
        public RecipeSearchType Source { get; set; }
    }
}
