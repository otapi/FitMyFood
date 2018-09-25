using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace FitMyFood.Models
{
    /// <summary>
    /// Settings of a daily profile. Basically what is the extra KCal to be consumed
    /// </summary>
    public class DailyProfile : IBase
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public int ExtraKcal { get; set; }
    }
}
