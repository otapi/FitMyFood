using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SQLite;

namespace FitMyFood.Services
{
    public class DataStore
    {
        readonly SQLiteAsyncConnection database;

        public DataStoreDAO<Models.ComposedFooodItem> composedFoodItems;
        public DataStoreDAO<Models.FoodItem> foodItems;
        public DataStoreDAO<Models.DailyProfile> dailyProfiles;
        public DataStoreDAO<Models.Meal> meals;
        public DataStoreDAO<Models.MealFood> mealFoods;
        public DataStoreDAO<Models.WeightTrack> weightTracks;

        public DataStoreDAO<Models.Settings> settings;

        public DataStore()
        {
            database = new SQLiteAsyncConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FitMyFoodSQLite.db3"));

            composedFoodItems = new DataStoreDAO<Models.ComposedFooodItem>(database);
            foodItems = new DataStoreDAO<Models.FoodItem>(database);
            dailyProfiles = new DataStoreDAO<Models.DailyProfile>(database);
            meals = new DataStoreDAO<Models.Meal>(database);
            mealFoods = new DataStoreDAO<Models.MealFood>(database);
            settings = new DataStoreDAO<Models.Settings>(database);
            weightTracks = new DataStoreDAO<Models.WeightTrack>(database);
        }
    }
}
