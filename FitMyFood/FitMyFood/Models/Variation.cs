using System;
using System.Collections.Generic;
using System.Text;
using SQLiteNetExtensionsAsync.Extensions;
using SQLiteNetExtensions.Attributes;

namespace FitMyFood.Models
{
    /// <summary>
    /// Variation for a given DailyProfile and Meal
    /// </summary>
    public class Variation : BaseModel
    {
        [ForeignKey(typeof(DailyProfile))]
        public int DailyProfileId { get; set; }
        [OneToOne]
        public DailyProfile DailyProfile { get; set; }

        [ForeignKey(typeof(Meal))]
        public int MealId { get; set; }
        [OneToOne]
        public Meal Meal { get; set; }

        [OneToMany]
        public List<VariationFoodItem> VariationFoodItems { get; set; }


    }
}
