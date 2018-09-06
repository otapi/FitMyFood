using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using System.Threading.Tasks;
using FitMyFood.Models;

namespace FitMyFood.Data
{
    public class DAODailyProfileMealVariation
    {
        readonly SQLiteAsyncConnection database;
        DataStore datastore;

        public DAODailyProfileMealVariation(DataStore datastore)
        {
            this.database = datastore.database;
            this.datastore = datastore;
            database.CreateTableAsync<ComposedFoodItem>().Wait();
        }

        public async Task<List<DailyProfileMealVariation>> GetVariationsAsync(string dailyProfileName, string mealName)
        {
            
            return await database.QueryAsync<DailyProfileMealVariation>(@"
                SELECT DailyProfileMealVariation.* FROM DailyProfileMealVariation
                INNER JOIN DailyProfile ON DailyProfile.Id = DailyProfileMealVariation.DailyProfile
                INNER JOIN Meal ON FoodItem.Id = DailyProfileMealVariation.Meal
                WHERE DailyProfile.Name = ?
                    AND  Meal.Name = ?
                    "
                , dailyProfileName, mealName);
        }

    }
}
