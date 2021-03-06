﻿using System;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitMyFood.Models
{
    /// <summary>
    /// Personal settings of the app and of the user
    /// </summary>
    public class Settings
    {
        public int SettingsId { get; set; }

        public double ActualWeight { get; set; }
        public int Height { get; set; }
        /// <summary>
        /// Male=true, female=false
        /// </summary>
        public bool Sex { get; set; }
        public int Age { get; set; }
        /// <summary>
        /// 1-Sedentary
        /// 2-Moderately
        /// 3-Active
        /// </summary>
        public int Physical_activity { get; set; }
        /// <summary>
        /// kg/week
        /// </summary>
        public double WeeklyWeightChange { get; set; }
        public int DailyFatRatio { get; set; }
        public int DailyCarboRatio { get; set; }
        public int DailyProteinRatio { get; set; }
    }
}
