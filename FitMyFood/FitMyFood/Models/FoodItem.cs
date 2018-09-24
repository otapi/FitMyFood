using System.Collections.Generic;
using FitMyFood.ViewModels;
using SQLiteNetExtensions.Attributes;

namespace FitMyFood.Models
{
    /// <summary>
    /// Datasheet for a food with it's ingredients
    /// </summary>
    public class FoodItem : BaseModel
    {
        [ManyToMany(typeof(Variation))]
        public List<Variation> Variations { get; set; }

        /// <summary>
        /// Default unit
        /// </summary>
        public string UnitDescription { get; set; }
        /// <summary>
        /// x gramm in a unit
        /// </summary>
        public double UnitWeight { get; set; }

        /// <summary>
        /// x gramm in 100g
        /// </summary>
        public double Fat { get; set; }
        /// <summary>
        /// x gramm in 100g
        /// </summary>
        public double Carbo { get; set; }
        /// <summary>
        /// x gramm in 100g
        /// </summary>
        public double Protein { get; set; }

        /// <summary>
        /// True if this is a composed food
        /// </summary>
        public bool IsComposedFood { get; set; }
        /// <summary>
        /// Sub-FoodItems if this is a composed item
        /// </summary>
        [OneToMany]
        public List<ComposedFoodItem> ComposedFoodItems { get; set; }
        /// <summary>
        /// How many unit has? Or what is the default quantity?
        /// </summary>
        public double Quantity { get; set; }
        /// <summary>
        /// Get Energy in kcal aka Calorie
        /// </summary>
        public double Energy
        {
            get
            {
                return (Protein * MainListFoodItemVM.ENERGYPROTEIN
                        + Carbo * MainListFoodItemVM.ENERGYCARBO
                        + Fat * MainListFoodItemVM.ENERGYFAT
                       ) * Weight / 100;
            }
        }
        /// <summary>
        /// Get weight in gramm
        /// </summary>
        public double Weight
        {
            get
            {
                return UnitWeight * Quantity;
            }
        }
        /// <summary>
        /// Item descriptor
        /// </summary>
        public string Descriptor
        {
            get
            {
                return $"{Quantity} {UnitDescription}(s) give {Energy} Calories with {Weight} gramm.";
            }
        }
    }
}