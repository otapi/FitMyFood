using System;
using System.Collections.Generic;
using System.Text;

namespace FitMyFood.Models
{
    /// <summary>
    /// DailyMails setting for user
    /// </summary>
    public class Meal : BaseModel
    {
        public String Name { get; set; }
        public int KcalRatio { get; set; }
    }
}
