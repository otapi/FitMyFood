using System;
using System.Collections.Generic;
using System.Text;

namespace FitMyFood.Models
{
    /// <summary>
    /// FoodItem for a given DailyProfile, Meal and Variaton
    /// </summary>
    public class DailyProfileMealVariationFoodItem : BaseModel
    {
        // Foreignkey
        public int DailyProfile { get; set; }
        // Foreignkey
        public int Meal { get; set; }
        // Foreignkey
        public int Variation { get; set; }
        // Foreignkey
        public int FoodItem { get; set; }
        /// <summary>
        /// How many unit has?
        /// </summary>
        public double Quantity { get; set; }
    }
}
