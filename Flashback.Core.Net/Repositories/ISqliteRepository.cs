using System.Collections.Generic;
using System.Threading.Tasks;
using Flashback.Core.Models;

namespace Flashback.Core.Net.Repositories
{
    public interface ISqliteRepository
    {
        Task InitializeAsync();
        Task<List<Category>> GetCategoriesAsync();
        Task<Category> GetCategoryAsync(int id);
        Task SaveCategoryAsync(Category category);
        Task DeleteCategoryAsync(int id);
        
        Task<List<Question>> GetQuestionsAsync(int categoryId);
        Task<Question> GetQuestionAsync(int id);
        Task SaveQuestionAsync(Question question);
        Task DeleteQuestionAsync(int id);
        
        Task<bool> ImportDatabaseAsync(string filePath);
        Task ExportDatabaseAsync(string filePath);
    }
}
