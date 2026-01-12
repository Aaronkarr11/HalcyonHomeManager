using HalcyonHomeManager.Entities;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalcyonHomeManager.DataLayer
{
    public class HalcyonHomeManagerDatabase
    {

        SQLiteAsyncConnection database;

        async Task Init()
        {
            if (database is not null)
                return;

            database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            var result = await database.CreateTableAsync<Project>();
        }

        public async Task<List<WorkItem>> GetItemsAsync()
        {
            await Init();
            return await database.Table<WorkItem>().ToListAsync();
        }

        //This is an example of using a query to return data
        //public async Task<List<WorkItem>> GetItemsNotDoneAsync()
        //{
        //    await Init();
        //    return await database.Table<WorkItem>().Where(t => t.Done).ToListAsync();

        //    // SQL queries are also possible
        //    //return await Database.QueryAsync<TodoItem>("SELECT * FROM [TodoItem] WHERE [Done] = 0");
        //}

        public async Task<WorkItem> GetItemAsync(int id)
        {
            await Init();
            return await database.Table<WorkItem>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public async Task<int> SaveItemAsync(WorkItem item)
        {
            await Init();
            if (item.ID != 0)
                return await database.UpdateAsync(item);
            else
                return await database.InsertAsync(item);
        }

        public async Task<int> DeleteItemAsync(WorkItem item)
        {
            await Init();
            return await database.DeleteAsync(item);
        }


    }
}
