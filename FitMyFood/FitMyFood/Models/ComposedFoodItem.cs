using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace FitMyFood.Models
{
    /// <summary>
    /// Datasheet for a food with it's ingredients
    /// </summary>
    public class ComposedFoodItem : BaseModel
    {
        [ForeignKey(typeof(FoodItem))]
        public int OwnerFoodItemId { get; set; }

        [ForeignKey(typeof(FoodItem))]
        public int SubFoodItemId { get; set; }
        [OneToOne]
        public FoodItem SubFoodItem { get; set; }

        /// <summary>
        /// How many unit has?
        /// </summary>
        public double Quantity { get; set; }
    }
}