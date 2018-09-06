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

        public DAOComposedFood<ComposedFoodItem> composedFoodItems;
        public DAOGeneral<FoodItem> foodItems;
        public DAOGeneral<DailyProfile> dailyProfiles;
        public DAOGeneral<Meal> meals;
        public DAOGeneral<DailyProfileMealVariation> dailyProfileMealVariation;
        public DAOGeneral<DailyProfileMealVariationFoodItem> dailyProfileMealVariationFoodItem;
        public DAOGeneral<WeightTrack> weightTracks;

        public DAOGeneral<Settings> settings;

        public DataStore()
        {
            database = new SQLiteAsyncConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FitMyFoodSQLite.db3"));

            composedFoodItems = new DAOComposedFood<ComposedFoodItem>(this);

            foodItems = new DAOGeneral<FoodItem>(this);
            dailyProfiles = new DAOGeneral<DailyProfile>(this);
            meals = new DAOGeneral<Meal>(this);
            dailyProfileMealVariation = new DAOGeneral<DailyProfileMealVariation>(this);
            dailyProfileMealVariationFoodItem = new DAOGeneral<DailyProfileMealVariationFoodItem>(this);
            settings = new DAOGeneral<Settings>(this);
            weightTracks = new DAOGeneral<WeightTrack>(this);
        }

        // Custom DAOs
        public async Task<List<DailyProfileMealVariation>> GetVariationsAsync(DailyProfile dailyProfile, Meal meal)
        {
            return await database.Table<DailyProfileMealVariation>().Where(
                i => i.DailyProfileId == dailyProfile.Id
                    && i.MealId == meal.Id).ToListAsync();
        }


    }
}
