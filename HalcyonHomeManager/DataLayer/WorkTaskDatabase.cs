using HalcyonHomeManager.Entities;
using SQLite;

namespace HalcyonHomeManager.DataLayer
{
    public class WorkTaskDatabase
    {

        SQLiteAsyncConnection database;

        public async Task Init()
        {
            if (database is not null)
                return;

            database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            await database.CreateTableAsync<WorkTask>();
        }

        public async Task<List<WorkTask>> GetWorkTasksAsync()
        {
            await Init();
            return await database.Table<WorkTask>().ToListAsync();
        }

        public async Task<WorkTask> GetWorkTaskAsync(int id)
        {
            await Init();
            return await database.Table<WorkTask>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public async Task<int> SaveWorkTaskAsync(WorkTask item)
        {
            await Init();
            item.TimeStamp = DateTime.Now;
            if (item.ID != 0)
                return await database.UpdateAsync(item);
            else
                return await database.InsertAsync(item);
        }

        public async Task<int> DeleteWorkTaskAsync(WorkTask item)
        {
            await Init();
            return await database.DeleteAsync(item);
        }


    }
}
