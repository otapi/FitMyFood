using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SQLite;
using System.Threading.Tasks;
using FitMyFood.Models;
using System.Linq;

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
            return await (
                from i in database.Table<DailyProfileMealVariation>()
                where i.DailyProfileId == dailyProfile.Id
                    && i.MealId == meal.Id
                select i
              ).ToListAsync();
            /*
            return await database.Table<DailyProfileMealVariation>().Where(
                i => i.DailyProfileId == dailyProfile.Id
                    && i.MealId == meal.Id).ToListAsync();*/
        }

        public async Task<List<FoodItem>> GetFoodItemsForMainList(DailyProfile dailyProfile, Meal meal, DailyProfileMealVariation variation)
        {
            var filteredFoodItemRefs = await (
                from i in database.Table<DailyProfileMealVariationFoodItem>()
                where i.DailyProfile == dailyProfile.Id
                    && i.Meal == meal.Id
                    && i.Variation == variation.Id
                select i
              ).ToListAsync();

            List<FoodItem> filteredFoodItems = new List<FoodItem>();
            foreach (var itemRef in filteredFoodItemRefs)
            {
                FoodItem foodItemQ = await foodItems.GetItemAsync(itemRef.FoodItem);
                if (foodItemQ == null)
                {
                    App.PrintWarning($"FoodItem {itemRef.FoodItem} not found in the foodItem table.");
                    continue;
                }
                foodItemQ.Quantity = itemRef.Quantity;
                filteredFoodItems.Add(foodItemQ);
            }
            return filteredFoodItems;
        }

        public async Task SaveFoodItemForVariation(DailyProfile dailyProfile, Meal meal, DailyProfileMealVariation variation, FoodItem foodItem)
        {
            await dailyProfileMealVariationFoodItem.SaveItemAsync(new DailyProfileMealVariationFoodItem()
            {
                DailyProfile = dailyProfile.Id,
                FoodItem = foodItem.Id,
                Meal = meal.Id,
                Quantity = foodItem.Quantity,
                Variation = variation.Id
            });
        }

        public async Task RemoveFoodItemsFromMainList(DailyProfile dailyProfile, Meal meal, DailyProfileMealVariation variation, FoodItem foodItem)
        {

            var filteredFoodItemRefs = await (
                from i in database.Table<DailyProfileMealVariationFoodItem>()
                where i.DailyProfile == dailyProfile.Id
                    && i.Meal == meal.Id
                    && i.Variation == variation.Id
                    && i.FoodItem == foodItem.Id
                select i
              ).ToListAsync();

            if (filteredFoodItemRefs.Count == 0)
            {
                App.PrintWarning($"You want to remove it, but the reference for food ({foodItem.Id}) is already missing from dailyProfile {dailyProfile.Id}, meal {meal.Id} and variation {variation.Id}.");
            }
            foreach (var itemRef in filteredFoodItemRefs)
            {
                await dailyProfileMealVariationFoodItem.DeleteItemAsync(itemRef);
            }
        }

    }
}
