using Flashback.Core.iPhone.SQLite;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System;

namespace Flashback.Core.iPhone
{

	/// <summary>
	/// Legacy SQLite-net repository.
	/// Requires Question + Category being decorated with attributes
	/// </summary>
	public class SQLiteNetRepository : IRepository
	{
		#region Categories
		public int SaveCategory(Category category)
		{
			SQLiteConnection connection = new SQLiteConnection(Settings.DatabaseFile);

			if (category.Id < 1)
				return connection.Insert(category);
			else
				return connection.Update(category);
		}

		public IList<Category> ListCategories()
		{
			SQLiteConnection connection = new SQLiteConnection(Settings.DatabaseFile);
			return connection.Table<Category>().ToList();
		}

		public Category ReadCategory(int id)
		{
			SQLiteConnection connection = new SQLiteConnection(Settings.DatabaseFile);
			return connection.Table<Category>().FirstOrDefault(c => c.Id == id);
		}

		public void DeleteCategory(int id)
		{
			SQLiteConnection connection = new SQLiteConnection(Settings.DatabaseFile);
			connection.Delete<Category>(new Category { Id = id });
		}
		#endregion

		#region Questions
		public int SaveQuestion(Question question)
		{
			SQLiteConnection connection = new SQLiteConnection(Settings.DatabaseFile);

			if (question.Id < 1)
				return connection.Insert(question);
			else
				return connection.Update(question);
		}

		public Question ReadQuestion(int id)
		{
			SQLiteConnection connection = new SQLiteConnection(Settings.DatabaseFile);
			return connection.Table<Question>().FirstOrDefault(q => q.Id == id);
		}

		public IList<Question> ListQuestions()
		{
			SQLiteConnection connection = new SQLiteConnection(Settings.DatabaseFile);
			return connection.Table<Question>().ToList();
		}

		public IList<Question> QuestionsForCategory(Category category)
		{
			SQLiteConnection connection = new SQLiteConnection(Settings.DatabaseFile);
			connection.Trace = true;

			int id = category.Id; // This is a quirk with SQLite not getting the property value
			return connection.Table<Question>().Where(q => q.Category.Id == id).ToList();
		}

		public void DeleteQuestion(int id)
		{
			SQLiteConnection connection = new SQLiteConnection(Settings.DatabaseFile);
			connection.Delete<Question>(new Question { Id = id });
		}

		public void MoveQuestion(Question question, int newIndex)
		{
			throw new NotImplementedException();
		}
		#endregion

		public void CreateDatabase()
		{
			SQLiteConnection connection = new SQLiteConnection(Settings.DatabaseFile);
			connection.CreateTable<Category>();
			connection.CreateTable<Question>();
		}

		/// <summary>
		/// Deletes the SQlite database from disk.
		/// </summary>
		public void DeleteDatabase()
		{
			try
			{
				if (File.Exists(Settings.DatabaseFile))
					File.Delete(Settings.DatabaseFile);
			}
			catch (IOException ex)
			{
				Logger.Fatal("Unable to delete the database file {0}: \n{1}", Settings.DatabaseFile, ex);
				throw;
			}
		}
	}
}
