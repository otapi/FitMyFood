using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitMyFood.Models
{
    /// <summary>
    /// Variation for a given DailyProfile and Meal
    /// </summary>
    public class Variation
    {
        public int VariationId { get; set; }
        public string Name { get; set; }

        public DailyProfile DailyProfile { get; set; }
        public Meal Meal { get; set; }

        public ICollection<VariationFoodItem> VariationFoodItems { get; } = new List<VariationFoodItem>();


    }
}
