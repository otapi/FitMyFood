using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace FitMyFood.Models
{
    /// <summary>
    /// Entry of measured weight for the user per days
    /// </summary>
    public class WeightTrack : IBase
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        /// <summary>
        /// Kg
        /// </summary>
        public double Weight { get; set; }
    }
}
