using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bakerscraper.Models
{
    public class RecipeStep
    {
        public int Number { get; set; }
        public string Text { get; set; }

        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                RecipeStep o = (RecipeStep)obj;
                return (Number == o.Number) && (Text == o.Text);
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Number, Text);
        }
    }
}
