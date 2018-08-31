using SQLite;
using System.Collections.Generic;

namespace FitMyFood.Models
{
    /// <summary>
    /// Datasheet for a food with it's ingredients
    /// </summary>
    public class ComposedFoodItem : BaseModel
    {
        // ForeignKey
        public int OwnerFoodItemId { get; set; }
        // ForeignKey
        public int FoodItemId { get; set; }

        /// <summary>
        /// How many unit has?
        /// </summary>
        public double Quantity { get; set; }
    }
}