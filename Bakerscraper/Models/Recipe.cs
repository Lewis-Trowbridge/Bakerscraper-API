using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bakerscraper.Enums;

namespace Bakerscraper.Models
{
    public struct Recipe
    {
        public string Name { get; set; }
        public List<RecipeIngredient> Ingredients { get; set; }
        public List<RecipeStep> Steps { get; set; }
        public RecipeSearchType Source { get; set; }

        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Recipe o = (Recipe)obj;
                return (Name == o.Name) && (Ingredients.SequenceEqual(o.Ingredients)) && (Steps.SequenceEqual(o.Steps) && (Source == o.Source));
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Ingredients, Steps, Source);
        }
    }
}
