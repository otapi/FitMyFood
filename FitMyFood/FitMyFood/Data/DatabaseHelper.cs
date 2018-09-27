﻿using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using FitMyFood.Models;

namespace FitMyFood.Data
{
    public class DatabaseHelper
    {
        DatabaseContext context;

        public DatabaseHelper()
        {
            context = CrateContext();
        }
        // TODO: why: There are many strategies for handling the lifecycle of a context object. I prefer to create a context when I need one and then dispose it at the end of the operation. 
        protected DatabaseContext CrateContext()
        {
            DatabaseContext databaseContext = new DatabaseContext();
            databaseContext.Database.EnsureCreated();
            databaseContext.Database.Migrate();
            return databaseContext;
        }

        #region MainListFoodItem
        public async Task<List<Meal>> getMealsAsync()
        {
            List<Meal> meals;
            meals = await context.Meals
                                //.AsNoTracking()
                                .ToListAsync();
            if (meals.Count == 0)
            {
                meals.AddRange(DefaultValues.MealSelectorItems);
                await context.Meals.AddRangeAsync(meals);
                await context.SaveChangesAsync();
            }
            return meals;
        }

        public async Task<List<DailyProfile>> getDailyProfilesAsync()
        {
            List<DailyProfile> dailyProfiles;
            dailyProfiles = await context.DailyProfiles
                                //.AsNoTracking()
                                .ToListAsync();
            if (dailyProfiles.Count == 0)
            {
                dailyProfiles.AddRange(DefaultValues.DailyProfileSelectorItems);
                await context.DailyProfiles.AddRangeAsync(dailyProfiles);
                await context.SaveChangesAsync();
            }
            return dailyProfiles;
        }

        public async Task<List<Variation>> getVariationsAsync(DailyProfile dailyProfile, Meal meal)
        {
            List<Variation> variations;
            variations = await context.Variations
                                        .Where(v => v.Meal == meal && v.DailyProfile == dailyProfile)
                                        .ToListAsync();

            if (variations.Count == 0)
            {
                var variation = new Variation()
                {
                    Name = DefaultValues.VariationSelectorItem,
                    DailyProfile = dailyProfile,
                    Meal = meal
                };
                await context.Variations.AddAsync(variation);
                await context.SaveChangesAsync();
                variations.Add(variation);
            }
            return variations;
        }

        public async Task<List<VariationFoodItem>> getVariationFoodItemsNoTrackingAsync(Variation variation)
        {
            List<VariationFoodItem> variationFoodItems;
            variationFoodItems = await context.VariationFoodItems
                                        .AsNoTracking()
                                        .Where(v => v.Variation == variation)
                                        .ToListAsync();
            return variationFoodItems;
        }


        public async Task<VariationFoodItem> getVariationFoodItemAsync(FoodItem foodItem, Variation variation)
        {
            VariationFoodItem variationFoodItem;
            variationFoodItem = await context.VariationFoodItems
                                        .Where(v => v.FoodItemId == foodItem.FoodItemId && v.Variation == variation)
                                        .FirstAsync();
            return variationFoodItem;
        }
        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }

        public async Task updateQuantityOnVariationFoodItemAsync(VariationFoodItem variationFoodItem)
        {
            context.Attach(variationFoodItem);
            context.Entry(variationFoodItem).Property("Quantity").IsModified = true;
            await context.SaveChangesAsync();
        }

        public async Task removeVariationFoodItemAsync(VariationFoodItem variationFoodItem)
        {
            context.Remove(variationFoodItem);
            await context.SaveChangesAsync();
        }

        public async Task addNewVariationFoodItemAsync(double Quantity, Variation variation, FoodItem foodItem)
        {
            //var realFoodItem = await context.FoodItems
            //                        .Where(f => f.FoodItemId == foodItemNotTracked.FoodItemId)
            //                        .FirstAsync();

            var variationFoodItem = new VariationFoodItem()
            {
                Quantity = Quantity,
                Variation = variation,
                FoodItem = foodItem
            };
            await context.SaveChangesAsync();
        }
        #endregion
    }
}
