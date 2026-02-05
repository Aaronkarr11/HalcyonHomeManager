using HalcyonHomeManager.Entities;
using SQLite;

namespace HalcyonHomeManager.DataLayer
{
    public class ProjectDatabase
    {

        SQLiteAsyncConnection database;

       public async Task Init()
        {
            if (database is not null)
                return;

            database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            await database.CreateTableAsync<Project>();
        }

        public async Task<List<Project>> GetProjectsAsync()
        {
            await Init();
            return await database.Table<Project>().ToListAsync();
        }

        public async Task<Project> GetProjectAsync(int id)
        {
            await Init();
            return await database.Table<Project>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public async Task<int> SaveProjectAsync(Project item)
        {
            await Init();
            if (item.ID != 0)
                return await database.UpdateAsync(item);
            else
                return await database.InsertAsync(item);
        }

        public async Task<int> DeleteProjectAsync(Project item)
        {
            await Init();
            return await database.DeleteAsync(item);
        }


    }
}
