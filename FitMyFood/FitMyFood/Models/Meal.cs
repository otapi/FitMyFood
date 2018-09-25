using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace FitMyFood.Models
{
    /// <summary>
    /// DailyMails setting for user
    /// </summary>
    public class Meal : IBase
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public int KcalRatio { get; set; }
    }
}
