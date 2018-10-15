using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using FitMyFood.Models;
using Xamarin.Forms;

namespace FitMyFood.Data
{
    public class Database : DbContext
    {
        public DbSet<ComposedFoodItem> ComposedFoodItems { get; set; }
        public DbSet<FoodItem> FoodItems { get; set; }
        public DbSet<DailyProfile> DailyProfiles { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<Variation> Variations { get; set; }
        public DbSet<VariationFoodItem> VariationFoodItems { get; set; }
        public DbSet<WeightTrack> WeightTracks { get; set; }
        public DbSet<Settings> Settings { get; set; }

        // TODO: why: There are many strategies for handling the lifecycle of a context object. I prefer to create a context when I need one and then dispose it at the end of the operation. 

        public static Database Create()
        {
            var databasePath = DependencyService.Get<Data.IFileHelper>().GetLocalFilePath("fitmyfood.db");
            App.PrintNote($"Database file: {databasePath}");
            var dbContext = new Database(databasePath);
            //databaseContext.Database.EnsureDeleted();
            //if (Device.RuntimePlatform == Device.Android)
            //{
                //databaseContext.Database.EnsureDeleted();
            //}
            dbContext.Database.EnsureCreated();
            dbContext.Database.Migrate();
            return dbContext;
        }


        #region MainList
        public async Task<List<Meal>> GetMealsAsync()
        {
            List<Meal> meals;
            meals = await Meals
                                //.AsNoTracking()
                                .ToListAsync();
            if (meals.Count == 0)
            {
                meals.AddRange(DefaultValues.MealSelectorItems);
                await Meals.AddRangeAsync(meals);
                await SaveChangesAsync();
            }
            return meals;
        }

        public async Task<List<DailyProfile>> GetDailyProfilesAsync()
        {
            List<DailyProfile> dailyProfiles;
            dailyProfiles = await DailyProfiles
                                //.AsNoTracking()
                                .ToListAsync();
            if (dailyProfiles.Count == 0)
            {
                dailyProfiles.AddRange(DefaultValues.DailyProfileSelectorItems);
                await DailyProfiles.AddRangeAsync(dailyProfiles);
                await SaveChangesAsync();
            }
            return dailyProfiles;
        }

        public async Task<List<Variation>> GetVariationsAsync(DailyProfile dailyProfile, Meal meal)
        {
            List<Variation> variations;
            variations = await Variations
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
                await Variations.AddAsync(variation);
                await SaveChangesAsync();
                variations.Add(variation);
            }
            return variations;
        }

        public async Task<List<VariationFoodItem>> GetVariationFoodItemsIncludeFoodItem(Variation variation)
        {
            List<VariationFoodItem> variationFoodItems;
            variationFoodItems = await VariationFoodItems
                                        .Include(v => v.FoodItem)
                                        .Where(v => v.Variation == variation)
                                        .ToListAsync();
            return variationFoodItems;
        }


        public async Task<VariationFoodItem> GetVariationFoodItemAsync(FoodItem foodItem, Variation variation)
        {
            VariationFoodItem variationFoodItem;
            variationFoodItem = await VariationFoodItems
                                        .Where(v => v.FoodItemId == foodItem.FoodItemId && v.Variation == variation)
                                        .FirstAsync();
            return variationFoodItem;
        }
        public void SaveChangesNoWait()
        {
            var t = SaveChangesAsync();
        }

        public async Task UpdateQuantityOnVariationFoodItemAsync(VariationFoodItem variationFoodItem)
        {
            Attach(variationFoodItem);
            Entry(variationFoodItem).Property("Quantity").IsModified = true;
            await SaveChangesAsync();
        }

        public async Task RemoveVariationFoodItemAsync(VariationFoodItem variationFoodItem)
        {
            Remove(variationFoodItem);
            await SaveChangesAsync();
        }

        public async Task<VariationFoodItem> AddNewVariationFoodItemAsync(double Quantity, Variation variation, FoodItem foodItem)
        {
            
            //var realFoodItem = await FoodItems
            //                        .Where(f => f.FoodItemId == foodItemNotAttached.FoodItemId)
            //                        .FirstAsync();

            var variationFoodItem = new VariationFoodItem()
            {
                Quantity = Quantity,
                Variation = variation,
                FoodItem = foodItem
            };
            await AddAsync(variationFoodItem);
            await SaveChangesAsync();
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
                foodItems = await FoodItems
                                        .OrderBy(v => v.Name)
                                        .ToListAsync();
            }
            else
            {
                filterForTerm = filterForTerm.ToLower();
                foodItems = await FoodItems
                                        .Where(v => v.Name.ToLower().Contains(filterForTerm))
                                        .OrderBy(v => v.Name)
                                        .ToListAsync();
            }
            return foodItems;
        }

        public async Task<Settings> GetSettings()
        {
            App.PrintNote($"[{this.GetType().Name}/{System.Reflection.MethodBase.GetCurrentMethod().Name}] start");

            var settings = (await Settings.ToListAsync());
            if (settings.Count == 0)
            {
                App.PrintNote($"[{this.GetType().Name}/{System.Reflection.MethodBase.GetCurrentMethod().Name}] default pre");

                var set = DefaultValues.Settings();
                App.PrintNote($"[{this.GetType().Name}/{System.Reflection.MethodBase.GetCurrentMethod().Name}] default after");

                await Settings.AddAsync(set);
                App.PrintNote($"[{this.GetType().Name}/{System.Reflection.MethodBase.GetCurrentMethod().Name}] add pre");

                await SaveChangesAsync();
                    App.PrintNote($"[{this.GetType().Name}/{System.Reflection.MethodBase.GetCurrentMethod().Name}] save after");

                return set;
            } else
            {
                return settings[0];
            }
        }

        public async Task<List<WeightTrack>> GetWeightTracks()
        {
            return await WeightTracks
                        .OrderByDescending(w=> w.Date) 
                        .ToListAsync();
        }

        public async Task SetWeightTrack(WeightTrack weightTrack)
        {
            var thisWeightTrack = await WeightTracks
                        .Where(w => w.Date == weightTrack.Date)
                        .ToListAsync();

            if (thisWeightTrack.Count == 0)
            {
                await WeightTracks.AddAsync(weightTrack);
            } else
            {
                thisWeightTrack[0].Weight = weightTrack.Weight;
                weightTrack = thisWeightTrack[0];
            }
        }
        public async Task<FoodItem> GetFoodItemAsTracked(FoodItem fakeFoodItem)
        {
            var realFoodItem = await FoodItems
                                        .Where(v => v.FoodItemId == fakeFoodItem.FoodItemId)
                                        .FirstAsync();
            return realFoodItem;
        }
        public async Task<FoodItem> GetFoodItemAsNoTracked(FoodItem foodItem)
        {
            var realFoodItem = await FoodItems
                                        .AsNoTracking()
                                        .Where(v => v.FoodItemId == foodItem.FoodItemId)
                                        .FirstAsync();
            return realFoodItem;
        }
        #endregion

        #region Private implementation

        protected string DatabasePath { get; set; }

        protected Database(string databasePath)
        {
            DatabasePath = databasePath;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={DatabasePath}");
        }
        #endregion
    }
}
