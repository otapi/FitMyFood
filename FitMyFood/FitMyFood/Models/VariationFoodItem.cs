using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitMyFood.Models
{
    /// <summary>
    /// FoodItem for a given DailyProfile, Meal and Variaton
    /// </summary>
    public class VariationFoodItem : BaseModel
    {
        [ForeignKey(typeof(Variation))]
        public int VariationId { get; set; }

        [ForeignKey(typeof(FoodItem))]
        public int FoodItemId { get; set; }
        [OneToOne]
        public FoodItem FoodItem { get; set; }
        
        /// <summary>
        /// How many unit has?
        /// </summary>
        public double Quantity { get; set; }
    }
}
