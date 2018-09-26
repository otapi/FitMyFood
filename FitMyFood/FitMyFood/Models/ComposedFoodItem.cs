
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitMyFood.Models
{
    /// <summary>
    /// Datasheet for a food with it's ingredients
    /// </summary>
    public class ComposedFoodItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ComposedFoodItemId { get; set; }

        // Subfooditem        
        public int? FoodItemId { get; set; }
        public FoodItem FoodItem { get; set; }

        /// <summary>
        /// How many unit has?
        /// </summary>
        public double Quantity { get; set; }
    }
}