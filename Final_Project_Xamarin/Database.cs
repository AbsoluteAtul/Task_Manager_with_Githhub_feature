using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project_Xamarin
{
    class Database
    {
        readonly SQLiteAsyncConnection _database;

        public Database(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<User>().Wait();
            _database.CreateTableAsync<Task>().Wait();

        }
        public Task<List<User>> GetUsersAsync()
        {
            return _database.Table<User>().ToListAsync();
        }

        public Task<int> SaveUserAsync(User user)
        {
            return _database.InsertAsync(user);

        }
        public Task<User> GetUser(string username)
        {
            // For getting 1 user
            return _database.Table<User>().FirstOrDefaultAsync(item => item.Username == username);
        }
        public Task<int> DeleteUserAsync(User user)
        {
            // for deleteling one userz
            return _database.DeleteAsync(user);
        }

        public Task<List<Task>> GetTasksAsync()
        {
            return _database.Table<Task>().ToListAsync();
        }
        public Task<int> SaveTasksAsync(Task tsk)
        {
            return _database.InsertAsync(tsk);
        }

        public Task<int> DeleteTaskAsync(Task tsk)
        {
            return _database.DeleteAsync(tsk);

        }
        public Task<int> EditTaskAsync(Task tsk)
        {
            return _database.UpdateAsync(tsk);

        }
    }
}
