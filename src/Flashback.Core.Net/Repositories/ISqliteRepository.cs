using System.Collections.Generic;
using System.Threading.Tasks;
// Use fully-qualified model types in method signatures to avoid ambiguity with domain types

namespace Flashback.Core.Net.Repositories
{
    public interface ISqliteRepository
    {
        Task InitializeAsync();
    Task<List<Flashback.Core.Models.Category>> GetCategoriesAsync();
    Task<Flashback.Core.Models.Category> GetCategoryAsync(int id);
    Task SaveCategoryAsync(Flashback.Core.Models.Category category);
        Task DeleteCategoryAsync(int id);
        
    Task<List<Flashback.Core.Models.Question>> GetQuestionsAsync(int categoryId);
    Task<Flashback.Core.Models.Question> GetQuestionAsync(int id);
    Task SaveQuestionAsync(Flashback.Core.Models.Question question);
        Task DeleteQuestionAsync(int id);
        
        Task<bool> ImportDatabaseAsync(string filePath);
        Task ExportDatabaseAsync(string filePath);
    }
}
