using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bakerscraper.Models
{
    public record RecipeStep
    {
        public int Number { get; set; }
        public string Text { get; set; }
    }
}
