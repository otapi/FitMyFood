using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitMyFood.Models
{
    /// <summary>
    /// FoodItem for a given DailyProfile, Meal and Variaton
    /// </summary>
    public class VariationFoodItem
    { 
        public int VariationFoodItemId {get; set; }
        public int VariationId { get; set; }
        public Variation Variation { get; set; }

        public int FoodItemId { get; set; }
        public FoodItem FoodItem { get; set; }

        /// <summary>
        /// How many unit has?
        /// </summary>
        public double Quantity { get; set; }
    }
}
