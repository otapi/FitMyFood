using SQLite;
using System.Collections.Generic;

namespace FitMyFood.Models
{
    /// <summary>
    /// Datasheet for a food with it's ingredients + Quantity for composedItems
    /// </summary>
    public class FoodItemWithQuantity : FoodItem
    {
        /// <summary>
        /// How many unit has?
        /// </summary>
        public double Quantity { get; set; }
    }
}