using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using Bakerscraper.Enums;

namespace Bakerscraper.Models
{
    public struct RecipeIngredient
    {
        public string Name { get; set; }
        public int? Quantity { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public RecipeIngredientUnit Unit { get; set; }
    }
}
