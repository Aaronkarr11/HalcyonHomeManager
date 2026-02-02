using HalcyonHomeManager.Entities;
using SQLite;

namespace HalcyonHomeManager.DataLayer
{
    public class HouseHoldDatabase
    {

        SQLiteAsyncConnection database;

        async Task Init()
        {
            if (database is not null)
                return;

            database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            await database.CreateTableAsync<HouseHoldMember>();
        }

        public async Task<List<HouseHoldMember>> GetItemsAsync()
        {
            await Init();
            return await database.Table<HouseHoldMember>().ToListAsync();
        }

        public async Task<HouseHoldMember> GetItemAsync(int id)
        {
            await Init();
            return await database.Table<HouseHoldMember>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public async Task<int> SaveItemAsync(HouseHoldMember item)
        {
            await Init();
            if (item.ID != 0)
                return await database.UpdateAsync(item);
            else
                return await database.InsertAsync(item);
        }

        public async Task<int> DeleteItemAsync(HouseHoldMember item)
        {
            await Init();
            return await database.DeleteAsync(item);
        }


    }
}
