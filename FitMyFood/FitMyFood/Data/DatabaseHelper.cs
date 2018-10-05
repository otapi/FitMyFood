using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using FitMyFood.Models;
using Xamarin.Forms;

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
            //databaseContext.Database.EnsureDeleted();
            if (Device.RuntimePlatform == Device.Android)
            {
                databaseContext.Database.EnsureDeleted();
            }
            databaseContext.Database.EnsureCreated();
            databaseContext.Database.Migrate();
            return databaseContext;
        }

        #region MainList
        public async Task<List<Meal>> GetMealsAsync()
        {
            List<Meal> meals;
            meals = await context.Meals
                                //.AsNoTracking()
                                .ToListAsync();
            if (meals.Count == 0)
            {
                meals.AddRange(DefaultValues.MealSelectorItems);
                await context.Meals.AddRangeAsync(meals);
                await SaveChangesAsync();
            }
            return meals;
        }

        public async Task<List<DailyProfile>> GetDailyProfilesAsync()
        {
            List<DailyProfile> dailyProfiles;
            dailyProfiles = await context.DailyProfiles
                                //.AsNoTracking()
                                .ToListAsync();
            if (dailyProfiles.Count == 0)
            {
                dailyProfiles.AddRange(DefaultValues.DailyProfileSelectorItems);
                await context.DailyProfiles.AddRangeAsync(dailyProfiles);
                await SaveChangesAsync();
            }
            return dailyProfiles;
        }

        public async Task<List<Variation>> GetVariationsAsync(DailyProfile dailyProfile, Meal meal)
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
                await SaveChangesAsync();
                variations.Add(variation);
            }
            return variations;
        }

        public async Task<List<VariationFoodItem>> GetVariationFoodItemsIncludeFoodItem(Variation variation)
        {
            List<VariationFoodItem> variationFoodItems;
            variationFoodItems = await context.VariationFoodItems
                                        .Include(v => v.FoodItem)
                                        .Where(v => v.Variation == variation)
                                        .ToListAsync();
            return variationFoodItems;
        }


        public async Task<VariationFoodItem> GetVariationFoodItemAsync(FoodItem foodItem, Variation variation)
        {
            VariationFoodItem variationFoodItem;
            variationFoodItem = await context.VariationFoodItems
                                        .Where(v => v.FoodItemId == foodItem.FoodItemId && v.Variation == variation)
                                        .FirstAsync();
            return variationFoodItem;
        }
        public void SaveChangesNoWait()
        {
            var t = SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }

        public async Task UpdateQuantityOnVariationFoodItemAsync(VariationFoodItem variationFoodItem)
        {
            context.Attach(variationFoodItem);
            context.Entry(variationFoodItem).Property("Quantity").IsModified = true;
            await SaveChangesAsync();
        }

        public async Task RemoveVariationFoodItemAsync(VariationFoodItem variationFoodItem)
        {
            context.Remove(variationFoodItem);
            await SaveChangesAsync();
        }

        public async Task<VariationFoodItem> AddNewVariationFoodItemAsync(double Quantity, Variation variation, FoodItem foodItem)
        {
            
            //var realFoodItem = await context.FoodItems
            //                        .Where(f => f.FoodItemId == foodItemNotAttached.FoodItemId)
            //                        .FirstAsync();

            var variationFoodItem = new VariationFoodItem()
            {
                Quantity = Quantity,
                Variation = variation,
                FoodItem = foodItem
            };
            await context.AddAsync(variationFoodItem);
            await context.SaveChangesAsync();
            return variationFoodItem;
        }

        public async Task ChangeFoodItemOnVariationFoodItemAsync(double Quantity, VariationFoodItem variationFoodItem, FoodItem newFoodItem)
        {
            variationFoodItem.FoodItem = newFoodItem;
            variationFoodItem.Quantity = Quantity;
            await SaveChangesAsync();
        }

        public async Task<List<FoodItem>> GetOrderedFoodItemsAsync(string filterForTerm)
        {
            List<FoodItem> foodItems;
            if (filterForTerm == null)
            {
                foodItems = await context.FoodItems
                                        .OrderBy(v => v.Name)
                                        .ToListAsync();
            }
            else
            {
                filterForTerm = filterForTerm.ToLower();
                foodItems = await context.FoodItems
                                        .Where(v => v.Name.ToLower().Contains(filterForTerm))
                                        .OrderBy(v => v.Name)
                                        .ToListAsync();
            }
            return foodItems;
        }

        public async Task<Settings> GetSettings()
        {
            var settings = (await context.Settings.ToListAsync());
            if (settings.Count == 0)
            {
                var set = DefaultValues.Settings();
                await context.Settings.AddAsync(set);

                await SaveChangesAsync();
                return set;
            } else
            {
                return settings[0];
            }
        }

        public async Task<List<WeightTrack>> GetWeightTracks()
        {
            return await context.WeightTracks
                        .OrderByDescending(w=> w.Date) 
                        .ToListAsync();
        }

        public async Task SetWeightTrack(WeightTrack weightTrack)
        {
            var thisWeightTrack = await context.WeightTracks
                        .Where(w => w.Date == weightTrack.Date)
                        .ToListAsync();

            if (thisWeightTrack.Count == 0)
            {
                await context.WeightTracks.AddAsync(weightTrack);
            } else
            {
                thisWeightTrack[0].Weight = weightTrack.Weight;
                weightTrack = thisWeightTrack[0];
            }
        }
        #endregion
    }
}
