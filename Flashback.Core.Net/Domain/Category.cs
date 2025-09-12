using System;
using System.Collections.Generic;

namespace Flashback.Core
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool InBuilt { get; set; }
        public bool Active { get; set; }

        public static int Save(Category category)
        {
            return Repository.Default.SaveCategory(category);
        }

        public static Category Read(int id)
        {
            return Repository.Default.ReadCategory(id);
        }

        public static IList<Category> List()
        {
            return Repository.Default.ListCategories();
        }

        public static void Delete(int id)
        {
            Repository.Default.DeleteCategory(id);
        }
    }
}
