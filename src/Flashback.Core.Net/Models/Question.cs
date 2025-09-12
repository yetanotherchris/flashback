using SQLite;

namespace Flashback.Core.Models
{
    [Table("Questions")]
    public class Question
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        
        public int CategoryId { get; set; }
        
        [Indexed]
        public string Text { get; set; }
        
        public string Answer { get; set; }
        
        public int OrderIndex { get; set; } = 0;
        
        public bool IsActive { get; set; } = true;
    }
}
