using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SQLite;
using Flashback.Core.Models;

namespace Flashback.Core.Net.Repositories
{
    public class SqliteRepository : ISqliteRepository
    {
        private static SqliteRepository _instance;
        private SQLiteAsyncConnection _database;
        private static readonly object _lock = new object();

        public static SqliteRepository Instance
        {
            get
            {
                lock (_lock)
                {
                    return _instance ??= new SqliteRepository();
                }
            }
            set
            {
                lock (_lock)
                {
                    _instance = value;
                }
            }
        }

        public SqliteRepository()
        {
        }

        public async Task InitializeAsync()
        {
            if (_database != null)
                return;

            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "flashback.db3");
            _database = new SQLiteAsyncConnection(dbPath);
            
            await _database.CreateTableAsync<Category>();
            await _database.CreateTableAsync<Question>();
            
            // Create indexes for better performance
            await _database.CreateIndexAsync<Category>(c => c.Name);
            await _database.CreateIndexAsync<Question>(q => q.CategoryId);
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            await InitializeAsync();
            return await _database.Table<Category>().ToListAsync();
        }

        public async Task<Category> GetCategoryAsync(int id)
        {
            await InitializeAsync();
            return await _database.Table<Category>().Where(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task SaveCategoryAsync(Category category)
        {
            await InitializeAsync();
            if (category.Id == 0)
            {
                await _database.InsertAsync(category);
            }
            else
            {
                await _database.UpdateAsync(category);
            }
        }

        public async Task DeleteCategoryAsync(int id)
        {
            await InitializeAsync();
            var category = await GetCategoryAsync(id);
            if (category != null)
            {
                await _database.DeleteAsync(category);
                
                // Also delete associated questions
                var questions = await _database.Table<Question>().Where(q => q.CategoryId == id).ToListAsync();
                await _database.DeleteAllAsync(questions);
            }
        }

        public async Task<List<Question>> GetQuestionsAsync(int categoryId)
        {
            await InitializeAsync();
            return await _database.Table<Question>()
                .Where(q => q.CategoryId == categoryId)
                .OrderBy(q => q.OrderIndex)
                .ToListAsync();
        }

        public async Task<Question> GetQuestionAsync(int id)
        {
            await InitializeAsync();
            return await _database.Table<Question>().Where(q => q.Id == id).FirstOrDefaultAsync();
        }

        public async Task SaveQuestionAsync(Question question)
        {
            await InitializeAsync();
            if (question.Id == 0)
            {
                await _database.InsertAsync(question);
            }
            else
            {
                await _database.UpdateAsync(question);
            }
        }

        public async Task DeleteQuestionAsync(int id)
        {
            await InitializeAsync();
            var question = await GetQuestionAsync(id);
            if (question != null)
            {
                await _database.DeleteAsync(question);
            }
        }

        public async Task<bool> ImportDatabaseAsync(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    return false;

                await InitializeAsync();
                
                // Backup current database
                var backupPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"flashback_backup_{DateTime.Now:yyyyMMdd_HHmmss}.db3");
                if (File.Exists(_database.DatabasePath))
                {
                    File.Copy(_database.DatabasePath, backupPath, true);
                }

                // Copy new database
                File.Copy(filePath, _database.DatabasePath, true);
                
                // Reinitialize connection
                _database = new SQLiteAsyncConnection(_database.DatabasePath);
                
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task ExportDatabaseAsync(string filePath)
        {
            await InitializeAsync();
            if (File.Exists(_database.DatabasePath))
            {
                File.Copy(_database.DatabasePath, filePath, true);
            }
        }
    }
}
