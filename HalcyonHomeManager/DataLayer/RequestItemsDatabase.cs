using HalcyonHomeManager.Entities;
using SQLite;

namespace HalcyonHomeManager.DataLayer
{
    public class RequestItemsDatabase
    {

        SQLiteAsyncConnection database;

        public async Task Init()
        {
            if (database is not null)
                return;

            database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            await database.CreateTableAsync<RequestItems>();
        }

        public async Task<List<RequestItems>> GetRequestItemsAsync()
        {
            await Init();
            return await database.Table<RequestItems>().ToListAsync();
        }

        //This is an example of using a query to return data
        //
        //public async Task<List<RequestItem>> GetItemsNotDoneAsync()
        //{
        //    await Init();
        //    return await database.Table<RequestItem>().Where(t => t.Done).ToListAsync();

        //    // SQL queries are also possible
        //    //return await Database.QueryAsync<TodoItem>("SELECT * FROM [TodoItem] WHERE [Done] = 0");
        //}

        public async Task<RequestItems> GetRequestItemAsync(int id)
        {
            await Init();
            return await database.Table<RequestItems>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public async Task<int> SaveRequestItemAsync(RequestItems item)
        {
            await Init();
            if (item.ID != 0)
                return await database.UpdateAsync(item);
            else
                return await database.InsertAsync(item);
        }

        public async Task<int> DeleteRequestItemAsync(RequestItems item)
        {
            await Init();
            return await database.DeleteAsync(item);
        }


    }
}
