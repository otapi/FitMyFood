using System;
using System.Collections.Generic;
using System.Text;

namespace FitMyFood.Models
{
    /// <summary>
    /// Entry of measured weight for the user per days
    /// </summary>
    public class WeightTrack : BaseModel
    {
        public DateTime Date { get; set; }
        /// <summary>
        /// Kg
        /// </summary>
        public double Weight { get; set; }
    }
}
