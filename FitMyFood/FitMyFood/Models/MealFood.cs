using System;
using System.Collections.Generic;
using System.Text;

namespace FitMyFood.Models
{
    /// <summary>
    /// Set of foods for a meal
    /// </summary>
    class MealFood : DataStoreItemMaster
    {
        // Foreignkey
        public int DailyProfile { get; set; }
        // Foreignkey
        public int Meal { get; set; }
        // Foreignkey
        public int FoodItem { get; set; }
        /// <summary>
        /// How many unit has?
        /// </summary>
        public double Quantity { get; set; }
    }
}
