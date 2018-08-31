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

        public DAOComposedFood composedFoodItems;
        public DAOGeneral<Models.FoodItem> foodItems;
        public DAOGeneral<Models.DailyProfile> dailyProfiles;
        public DAOGeneral<Models.Meal> meals;
        public DAOGeneral<Models.MealFood> mealFoods;
        public DAOGeneral<Models.WeightTrack> weightTracks;

        public DAOGeneral<Models.Settings> settings;

        public DataStore()
        {
            database = new SQLiteAsyncConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FitMyFoodSQLite.db3"));

            composedFoodItems = new DAOComposedFood(this);
            foodItems = new DAOGeneral<Models.FoodItem>(database);
            dailyProfiles = new DAOGeneral<Models.DailyProfile>(database);
            meals = new DAOGeneral<Models.Meal>(database);
            mealFoods = new DAOGeneral<Models.MealFood>(database);
            settings = new DAOGeneral<Models.Settings>(database);
            weightTracks = new DAOGeneral<Models.WeightTrack>(database);
        }

        
        
    }
}
