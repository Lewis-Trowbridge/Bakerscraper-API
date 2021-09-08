using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Bakerscraper.Enums;

namespace Bakerscraper.Models
{
    public class RecipeSearch
    {
        [Required]
        public string String { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [Required]
        public RecipeSearchType Type { get; set; }
    }
}
