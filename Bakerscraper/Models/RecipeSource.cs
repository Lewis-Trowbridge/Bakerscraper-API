using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bakerscraper.Enums;

namespace Bakerscraper.Models
{
    public record RecipeSource
    {
        public RecipeSearchType SourceType { get; set; }
        public Uri SourceUrl { get; set; }
    }
}
