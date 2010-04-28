using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flashback.Core
{
	/// <summary>
	/// The category for a question.
	/// </summary>
	public class Category
	{
		public int Id { get; set; }

		/// <summary>
		/// The name of the category
		/// </summary>
		public string Name { get; set; }

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
