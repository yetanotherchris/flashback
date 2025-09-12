using SQLite;

namespace Flashback.Core.Models
{
    [Table("Categories")]
    public class Category
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        
        [Indexed]
        public required string Name { get; set; }
        
        public required string Description { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public int QuestionCount { get; set; } = 0;
    }
}
