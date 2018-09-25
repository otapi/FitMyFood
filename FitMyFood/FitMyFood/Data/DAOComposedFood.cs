﻿using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using System.Threading.Tasks;
using FitMyFood.Models;

namespace FitMyFood.Data
{
    public class DAOComposedFood<T> : DAOGeneral<T> where T : Models.IBase, new()
    {
        public DAOComposedFood(DataStore datastore) : base(datastore)
        {

        }

        public async Task<List<FoodItem>> GetComposedFoodItemsAsync(FoodItem Owner)
        {
            if (!Owner.IsComposedFood) {
                throw new Exception("Query the composed items of a non-composed food");
            }
            return await database.QueryAsync<FoodItem>(@"
                SELECT FoodItem.*, ComposedFoodItem.Quantity FROM FoodItem
                JOIN ComposedFoodItem ON FoodItem.Id = ComposedFoodItem.FoodItemId
                WHERE ComposedFoodItem.OwnerFoodItemId = ?"
                , Owner.Id);
        }

        private async Task<ComposedFoodItem> FindComposedFoodItem(FoodItem owner, FoodItem foodItem)
        {
            var table = database.Table<ComposedFoodItem>();
            var query = from r
                        in table
                        where r.OwnerFoodItemId == owner.Id && r.FoodItemId == foodItem.Id
                        select r;
            ComposedFoodItem composedItem = await query.FirstAsync();
            return composedItem;
        }

        public async Task<int> UpdateItemAsync(FoodItem owner, FoodItem foodItem, double quantity)
        {
            ComposedFoodItem composedItem = await FindComposedFoodItem(owner, foodItem);
            if (composedItem == null)
            {
                throw new Exception("The owner - composed item relationship is missing from the ComposedFoodItem table. Could not update it.");
            }

            composedItem.Quantity = quantity;
            return await database.UpdateAsync(composedItem);
        }

        public async Task<int> AddItemAsync(FoodItem owner, FoodItem foodItem, double quantity)
        {
            if (!owner.IsComposedFood)
            {
                owner.IsComposedFood = true;
                await datastore.foodItems.SaveItemAsync(owner);
            }
            return await database.InsertAsync(
                new ComposedFoodItem
                {
                    OwnerFoodItemId = owner.Id,
                    FoodItemId = foodItem.Id,
                    Quantity = quantity
                });
        }
        public async Task<int> DeleteItemAsync(FoodItem owner, FoodItem foodItem)
        {
            ComposedFoodItem composedItem = await FindComposedFoodItem(owner, foodItem);
            if (composedItem == null)
            {
                throw new Exception("The owner - composed item relationship is missing from the ComposedFoodItem table. Could not delet it.");
            }
            if ((await GetComposedFoodItemsAsync(owner)).Count == 1)
            {
                owner.IsComposedFood = false;
                await datastore.foodItems.SaveItemAsync(owner);
            }
            return await database.DeleteAsync(composedItem);
        }
    }
}
