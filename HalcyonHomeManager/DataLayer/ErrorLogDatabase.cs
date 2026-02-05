using HalcyonHomeManager.Entities;
using SQLite;

namespace HalcyonHomeManager.DataLayer
{
    public class ErrorLogDatabase
    {

        SQLiteAsyncConnection database;

       public async Task Init()
        {
            if (database is not null)
                return;

            database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            await database.CreateTableAsync<ErrorLog>();
        }

        public async Task<List<ErrorLog>> GetErrorLogsAsync()
        {
            await Init();
            return await database.Table<ErrorLog>().ToListAsync();
        }

        public async Task<ErrorLog> GetErrorLogAsync(int id)
        {
            await Init();
            return await database.Table<ErrorLog>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public async Task<int> SaveErrorLogAsync(ErrorLog item)
        {
            await Init();
            if (item.ID != 0)
                return await database.UpdateAsync(item);
            else
                return await database.InsertAsync(item);
        }

        public async Task<int> DeleteErrorLogAsync(ErrorLog item)
        {
            await Init();
            return await database.DeleteAsync(item);
        }


    }
}
