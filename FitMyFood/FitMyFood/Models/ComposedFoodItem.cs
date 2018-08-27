using SQLite;
using System.Collections.Generic;

namespace FitMyFood.Models
{
    /// <summary>
    /// Datasheet for a food with it's ingredients
    /// </summary>
    public class ComposedFooodItem : DataStoreItemMaster
    {
        // ForeignKey
        public int OwnerFoodItem;
        // ForeignKey
        public int FoodItem;
    }
}