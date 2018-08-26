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

        public DataStoreDAO<Models.FoodItem> foodItems;

        public DataStore()
        {
            database = new SQLiteAsyncConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FitMyFoodSQLite.db3"));

            foodItems = new DataStoreDAO<Models.FoodItem>(database);
        }
    }
}
