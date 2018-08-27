using System;
using System.Collections.Generic;
using System.Text;

namespace FitMyFood.Models
{
    /// <summary>
    /// Settings of a daily profile. Basically what is the extra KCal to be consumed
    /// </summary>
    class DailyProfile : DataStoreItemMaster
    {
        public String Name { get; set; }
        public int ExtraKcal { get; set; }
    }
}
