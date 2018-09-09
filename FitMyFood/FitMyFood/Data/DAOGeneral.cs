using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using System.Threading.Tasks;

namespace FitMyFood.Data
{
    public class DAOGeneral<T> where T : Models.BaseModel, new()
    {
        protected readonly SQLiteAsyncConnection database;
        protected readonly DataStore datastore;

        public DAOGeneral(DataStore datastore)
        {
            this.datastore = datastore;
            this.database = datastore.database;
            database.CreateTableAsync<T>().Wait();
        }

        public async Task<List<T>> GetItemsAsync()
        {
            return await database.Table<T>().ToListAsync();
        }

        public async Task<List<T>> GetItemsByNameAsync(string name)
        {
            return await database.Table<T>().Where(i => i.Name == name).ToListAsync();
        }

        public async Task<T> GetItemAsync(int Id)
        {
            return await database.Table<T>().Where(i => i.Id == Id).FirstOrDefaultAsync();
        }

        public async Task<T> GetFirstItemByNameAsync(string name)
        {
            return await database.Table<T>().Where(i => i.Name == name).FirstOrDefaultAsync();
        }

        public async Task<int> SaveItemAsync(T item)
        {
            if (await GetItemAsync(item.Id) == null)
            {
                // Add as new
                return await database.InsertAsync(item);
            }
            else
            {
                // Update the old one
                return await database.UpdateAsync(item);
            }
        }
        public async Task SaveItemsAsync(List<T> items)
        {
            foreach (var t in items)
            {
                await SaveItemAsync(t);
            }
        }
        public async Task<int> DeleteItemAsync(T item)
        {
            return await database.DeleteAsync(item);
        }
    }
}
