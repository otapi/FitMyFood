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
        public int DailyProfileId { get; set; }
        // Foreignkey
        public int MealId { get; set; }

    }
}
