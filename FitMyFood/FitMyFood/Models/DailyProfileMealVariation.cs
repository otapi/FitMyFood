using System;
using System.Collections.Generic;
using System.Text;

namespace FitMyFood.Models
{
    /// <summary>
    /// Variation for a given DailyProfile and Meal
    /// </summary>
    public class DailyProfileMealVariation : BaseModel
    {
        // Foreignkey
        public int DailyProfile { get; set; }
        // Foreignkey
        public int Meal { get; set; }
        public string VariationName { get; set; }
    }
}
