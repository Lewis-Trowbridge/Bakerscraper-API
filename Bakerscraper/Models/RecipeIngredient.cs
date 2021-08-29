using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bakerscraper.Enums;

namespace Bakerscraper.Models
{
    public struct RecipeIngredient
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public RecipeIngredientUnit Unit { get; set; }
    }
}
