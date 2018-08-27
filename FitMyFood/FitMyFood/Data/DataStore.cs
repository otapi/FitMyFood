using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SQLite;
using System.Threading.Tasks;
using FitMyFood.Models;

namespace FitMyFood.Data
{
    public class DataStore
    {
        public readonly SQLiteAsyncConnection database;

        public ComposedFoodDAO composedFoodItems;
        public SimpleDAO<Models.FoodItem> foodItems;
        public SimpleDAO<Models.DailyProfile> dailyProfiles;
        public SimpleDAO<Models.Meal> meals;
        public SimpleDAO<Models.MealFood> mealFoods;
        public SimpleDAO<Models.WeightTrack> weightTracks;

        public SimpleDAO<Models.Settings> settings;

        public DataStore()
        {
            database = new SQLiteAsyncConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FitMyFoodSQLite.db3"));

            composedFoodItems = new ComposedFoodDAO(this);
            foodItems = new SimpleDAO<Models.FoodItem>(database);
            dailyProfiles = new SimpleDAO<Models.DailyProfile>(database);
            meals = new SimpleDAO<Models.Meal>(database);
            mealFoods = new SimpleDAO<Models.MealFood>(database);
            settings = new SimpleDAO<Models.Settings>(database);
            weightTracks = new SimpleDAO<Models.WeightTrack>(database);
        }

        
        
    }
}
