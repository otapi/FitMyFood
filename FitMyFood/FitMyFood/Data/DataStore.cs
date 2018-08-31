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
        public BaseDAO<Models.FoodItem> foodItems;
        public BaseDAO<Models.DailyProfile> dailyProfiles;
        public BaseDAO<Models.Meal> meals;
        public BaseDAO<Models.MealFood> mealFoods;
        public BaseDAO<Models.WeightTrack> weightTracks;

        public BaseDAO<Models.Settings> settings;

        public DataStore()
        {
            database = new SQLiteAsyncConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FitMyFoodSQLite.db3"));

            composedFoodItems = new ComposedFoodDAO(this);
            foodItems = new BaseDAO<Models.FoodItem>(database);
            dailyProfiles = new BaseDAO<Models.DailyProfile>(database);
            meals = new BaseDAO<Models.Meal>(database);
            mealFoods = new BaseDAO<Models.MealFood>(database);
            settings = new BaseDAO<Models.Settings>(database);
            weightTracks = new BaseDAO<Models.WeightTrack>(database);
        }

        
        
    }
}
