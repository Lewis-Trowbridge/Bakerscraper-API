﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using Bakerscraper.Enums;

namespace Bakerscraper.Models
{
    public class RecipeIngredient
    {
        public string Name { get; set; }
        public double? Quantity { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public RecipeIngredientUnit Unit { get; set; }

        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                RecipeIngredient o = (RecipeIngredient)obj;

                return (Name == o.Name) && (Quantity == o.Quantity) && (Unit == o.Unit);
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Quantity, Unit);
        }

        public static bool operator ==(RecipeIngredient left, RecipeIngredient right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(RecipeIngredient left, RecipeIngredient right)
        {
            return !(left == right);
        }
    }
}
