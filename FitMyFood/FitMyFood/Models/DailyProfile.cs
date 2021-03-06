﻿using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitMyFood.Models
{
    /// <summary>
    /// Settings of a daily profile. Basically what is the extra KCal to be consumed
    /// </summary>
    public class DailyProfile
    {
        public int DailyProfileId { get; set; }
        public string Name { get; set; }
        public int ExtraKcal { get; set; }
    }
}
