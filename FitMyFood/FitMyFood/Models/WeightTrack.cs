using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitMyFood.Models
{
    /// <summary>
    /// Entry of measured weight for the user per days
    /// </summary>
    public class WeightTrack
    {
        [Key]
        public DateTime Date { get; set; }
        /// <summary>
        /// Kg
        /// </summary>
        public double Weight { get; set; }
    }
}
