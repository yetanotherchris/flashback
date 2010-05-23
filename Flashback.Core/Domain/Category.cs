using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flashback.Core
{
	/// <summary>
	/// A category for a set of questions.
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
		/// Whether the category is active. Inactive categories don't appear on the iPhone badge count. 
		/// </summary>
		public bool Active { get; set; }

		/// <summary>
		/// Saves the category to the default repository, returning its id.
		/// </summary>
		public static int Save(Category category)
		{
			return Repository.Default.SaveCategory(category);
		}

		/// <summary>
		/// Retrieves a category from the database repository based on the id provided.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static Category Read(int id)
		{
			return Repository.Default.ReadCategory(id);
		}

		/// <summary>
		/// Retrieves all categories from the database repository.
		/// </summary>
		/// <returns></returns>
		public static IList<Category> List()
		{
			return Repository.Default.ListCategories();
		}

		/// <summary>
		/// Deletes the category and all its questions using the provided id.
		/// </summary>
		/// <param name="id"></param>
		public static void Delete(int id)
		{
			Repository.Default.DeleteCategory(id);
		}
	}
}
