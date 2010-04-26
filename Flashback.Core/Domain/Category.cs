using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;

namespace Flashback.Core
{
	/// <summary>
	/// The category for a question.
	/// </summary>
	public class Category
	{
		[PrimaryKey,AutoIncrement]
		public int Id { get; set; }
		
		/// <summary>
		/// The name of the category
		/// </summary>
		[MaxLength(100)]
		public string Name { get; set; }
		
		public Category()
		{

		}
		
		public static int Save(Category category)
		{
			SQLiteConnection connection = new SQLiteConnection(Settings.DatabaseFile);
			
			if (category.Id < 1)	
				return connection.Insert(category);
			else
				return connection.Update(category);
		}
		
		public static IList<Category> List()
		{
			SQLiteConnection connection = new SQLiteConnection(Settings.DatabaseFile);
			return connection.Table<Category>().ToList();
		}
		
		public static Category Read(int id)
		{
			SQLiteConnection connection = new SQLiteConnection(Settings.DatabaseFile);
			return connection.Table<Category>().FirstOrDefault(c => c.Id == id);
		}
		
		public static void Delete(int id)
		{
			SQLiteConnection connection = new SQLiteConnection(Settings.DatabaseFile);
			connection.Delete<Category>(new Category { Id=id });
		}
	}
}
