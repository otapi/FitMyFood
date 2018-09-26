using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FitMyFood.Models;
using System.Linq;
using SQLite;

using SQLiteNetExtensionsAsync.Extensions;

namespace FitMyFood.Data
{
    public class DataStore
    {
        // https://blog.infeeny.com/2016/05/30/xamarin-using-sqlite-net-async-in-pcl/
        // https://www.dsibinski.pl/2017/05/sqlite-net-extensions-one-to-many-relationships/
        static async public Task<SQLiteAsyncConnection> GetDataStore()
        {
            string file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FitMyFoodSQLite.db3");
            SQLiteAsyncConnection connection = new SQLiteAsyncConnection(file);
            await connection.CreateTableAsync<ComposedFoodItem>();
            await connection.CreateTableAsync<FoodItem>();
            await connection.CreateTableAsync<DailyProfile>();
            await connection.CreateTableAsync<Meal>();
            await connection.CreateTableAsync<Variation>();
            await connection.CreateTableAsync<VariationFoodItem>();
            await connection.CreateTableAsync<WeightTrack>();
            await connection.CreateTableAsync<Settings>();
            
            return connection;
        }
    }
}
