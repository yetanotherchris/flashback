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
		/// <summary>
		/// The unique Id of the category.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// The name of the category
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Whether the category comes with the Flashback application.
		/// </summary>
		public bool InBuilt { get; set; }

		/// <summary>
		/// Shortcut helper for saving a category.
		/// </summary>
		/// <param name="category"></param>
		/// <returns></returns>
		public static int Save(Category category)
		{
			return Repository.Default.SaveCategory(category);
		}

		/// <summary>
		/// Shortcut helper for getting a single category.
		/// </summary>
		/// <param name="category"></param>
		/// <returns></returns>
		public static Category Read(int id)
		{
			return Repository.Default.ReadCategory(id);
		}

		/// <summary>
		/// Shortcut helper for listing all categories.
		/// </summary>
		/// <param name="category"></param>
		/// <returns></returns>
		public static IList<Category> List()
		{
			return Repository.Default.ListCategories();
		}

		/// <summary>
		/// Shortcut helper for deleting a category.
		/// </summary>
		/// <param name="category"></param>
		/// <returns></returns>
		public static void Delete(int id)
		{
			Repository.Default.DeleteCategory(id);
		}
	}
}
