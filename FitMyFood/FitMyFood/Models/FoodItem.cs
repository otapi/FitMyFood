﻿using System.Collections.Generic;

namespace FitMyFood.Models
{
    /// <summary>
    /// Datasheet for a food with it's ingredients
    /// </summary>
    public class FoodItem : BaseModel
    {
        /// <summary>
        /// Default unit
        /// </summary>
        public string UnitDescription { get; set; }
        /// <summary>
        /// x gram in a unit
        /// </summary>
        public double UnitWeight { get; set; }

        /// <summary>
        /// x gram in 100g
        /// </summary>
        public double Fat { get; set; }
        /// <summary>
        /// x gram in 100g
        /// </summary>
        public double Carbo { get; set; }
        /// <summary>
        /// x gram in 100g
        /// </summary>
        public double Protein { get; set; }

        /// <summary>
        /// True if this is a composed food
        /// </summary>
        public bool IsComposedFood { get; set; }
        /// <summary>
        /// How many unit has? Or what is the default quantity?
        /// </summary>
        public double Quantity { get; set; }
    }
}