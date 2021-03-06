﻿using FitMyFood.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitMyFood.Models
{
    public class DefaultValues
    {
        public static List<Meal> MealSelectorItems = 
         new List<Meal>()
            {
                new Meal() { Name = "Breakfast", KcalRatio = 20 },
                new Meal() { Name = "1st Snack", KcalRatio = 10 },
                new Meal() { Name = "Lunch", KcalRatio = 35 },
                new Meal() { Name = "2nd Snack", KcalRatio = 10 },
                new Meal() { Name = "Dinner", KcalRatio = 25 }
            };
        
        public static List<DailyProfile> DailyProfileSelectorItems =
        new List<DailyProfile>()
            {
                new DailyProfile() { Name = "Normal", ExtraKcal = 0 },
                new DailyProfile() { Name = "Sport", ExtraKcal = 800 }
            };

        public static string VariationSelectorItem = "Variation A";

        public static Settings Settings()
        {
            return new Settings
            {
                ActualWeight = 82,
                Height = 171,
                Sex = true,
                Age = 39,
                Physical_activity = 1,
                WeeklyWeightChange = -0.5,
                DailyFatRatio = 20,
                DailyCarboRatio = 50,
                DailyProteinRatio = 30
            };
        }
    }
}
