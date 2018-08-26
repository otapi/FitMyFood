﻿using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using System.Threading.Tasks;

namespace FitMyFood.Services
{
    public class DataStoreDAO<T> where T : Models.DataStoreItemMaster, new()
    {
        readonly SQLiteAsyncConnection database;

        public DataStoreDAO(SQLiteAsyncConnection database)
        {
            this.database = database;
            database.CreateTableAsync<T>().Wait();
        }

        public async Task<List<T>> GetItemsAsync()
        {
            return await database.Table<T>().ToListAsync();
        }
        
        public async Task<T> GetItemAsync(int Id)
        {
            return await database.Table<T>().Where(i => i.Id == Id).FirstOrDefaultAsync();
        }

        public async Task<int> UpdateItemAsync(T item)
        {
            return await database.UpdateAsync(item);
        }

        public async Task<int> AddItemAsync(T item)
        {
            return await database.InsertAsync(item);
        }

        public async Task<int> DeleteItemAsync(T item)
        {
            return await database.DeleteAsync(item);
        }
    }
}
