using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


#if MONOTOUCH
using Mono.Data.Sqlite;
using System.Data;
using Flashback.Core.Data;
#endif

#if WINDOWS
using SqliteDataReader = System.Data.SQLite.SQLiteDataReader;
using SqliteCommand = System.Data.SQLite.SQLiteCommand;
using SqliteConnection = System.Data.SQLite.SQLiteConnection;
using SqliteParameter = System.Data.SQLite.SQLiteParameter;
using SqliteException = System.Data.SQLite.SQLiteException;
using System.Data;
using Flashback.Core.Data;
#endif



namespace Flashback.Core.iPhone
{
	/// <summary>
	/// A SQlite repository.
	/// </summary>
	public class SqliteRepository : IRepository
	{
		#region Categories
		public void DeleteCategory(int id)
		{
			try
			{
				using (SqliteConnection connection = new SqliteConnection(Settings.DatabaseConnection))
				{
					connection.Open();
					using (SqliteCommand command = new SqliteCommand(connection))
					{
						// The category
						command.CommandText = "DELETE FROM categories WHERE id=@id";
						SqliteParameter parameter = new SqliteParameter("@id", DbType.Int32);
						parameter.Value = id;
						command.Parameters.Add(parameter);

						command.ExecuteNonQuery();

						// The questions for the category
						command.CommandText = "DELETE FROM questions WHERE categoryid=@id";
						command.ExecuteNonQuery();
					}
				}
			}
			catch (SqliteException e)
			{
				Logger.Warn("SqliteException occurred with DeleteCategory({0}): \n{1}", id, e);
			}
		}

		public IList<Category> ListCategories()
		{
			IList<Category> list = new List<Category>();

			try
			{
				using (SqliteConnection connection = new SqliteConnection(Settings.DatabaseConnection))
				{
					connection.Open();
					using (SqliteCommand command = new SqliteCommand(connection))
					{
						command.CommandText = "SELECT id,name,inbuilt,active FROM categories";

						using (SqliteDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								Category category = new Category();
								category.Id = reader.GetInt32(0);
								category.Name = reader.GetString(1);
								category.InBuilt = reader.GetBoolean(2);
								category.Active = reader.GetBoolean(3);

								list.Add(category);
							}
						}
					}
				}
			}
			catch (SqliteException e)
			{
				Logger.Warn("SqliteException occurred while listing categories: \n{0}", e);
			}

			return list;
		}

		public Category ReadCategory(int id)
		{
			Category category = new Category();

			try
			{
				using (SqliteConnection connection = new SqliteConnection(Settings.DatabaseConnection))
				{
					connection.Open();
					using (SqliteCommand command = new SqliteCommand(connection))
					{
						command.CommandText = "SELECT * FROM categories WHERE id=@id";
						SqliteParameter parameter = new SqliteParameter("@id", DbType.Int32);
						parameter.Value = id;
						command.Parameters.Add(parameter);

						using (SqliteDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								category.Id = Convert.ToInt32(reader["categoryid"]);
								category.InBuilt = Convert.ToBoolean(reader["inbuilt"]);
								category.Name = (string)reader["categoryName"];
								category.Active = Convert.ToBoolean(reader["active"]);
							}
						}
					}
				}
			}
			catch (SqliteException e)
			{
				Logger.Warn("SqliteException occurred with ReadCategory({0}): \n{1}", id, e);
			}

			return category;
		}

		public int SaveCategory(Category category)
		{
			try
			{
				using (SqliteConnection connection = new SqliteConnection(Settings.DatabaseConnection))
				{
					bool updating = (category.Id > 0);

					connection.Open();
					using (SqliteCommand command = new SqliteCommand(connection))
					{
						SqliteParameter parameter;
						string sql = @"INSERT INTO categories (name,active) VALUES (@name,@active);SELECT last_insert_rowid();";

						if (updating)
						{
							sql = @"UPDATE categories SET name=@name,active=@active WHERE id=@id";

							parameter = new SqliteParameter("@id", DbType.Int32);
							parameter.Value = category.Id;
							command.Parameters.Add(parameter);
						}

						parameter = new SqliteParameter("@name", DbType.String);
						parameter.Value = category.Name;
						command.Parameters.Add(parameter);
						
						parameter = new SqliteParameter("@active", DbType.Boolean);
						parameter.Value = category.Active;
						command.Parameters.Add(parameter);

						command.CommandText = sql;
						int result;

						if (updating)
						{
							command.ExecuteNonQuery();
							result = category.Id;
							
							// If result isn't the categoryid, then ignore it
						}
						else
						{
							Int64 newId = (Int64)command.ExecuteScalar();
							result = Convert.ToInt32(newId);
							category.Id = result;
						}
						
						return result;
					}
				}
			}
			catch (SqliteException e)
			{
				Logger.Warn("SqliteException occurred with SaveCategory({0}): \n{1}", category.Id, e);
			}

			return 0;
		}
		#endregion

		#region Questions
		public void DeleteQuestion(int id)
		{
			try
			{
				using (SqliteConnection connection = new SqliteConnection(Settings.DatabaseConnection))
				{
					connection.Open();
					using (SqliteCommand command = new SqliteCommand(connection))
					{
						command.CommandText = "DELETE FROM questions WHERE id=@id";
						SqliteParameter parameter = new SqliteParameter("@id", DbType.Int32);
						parameter.Value = id;
						command.Parameters.Add(parameter);

						command.ExecuteNonQuery();
					}
				}
			}
			catch (SqliteException e)
			{
				Logger.Warn("SqliteException occurred with DeleteQuestion({0}): \n{1}", id, e);
			}
		}
		public IList<Question> ListQuestions()
		{
			IList<Question> list = new List<Question>();

			try
			{
				using (SqliteConnection connection = new SqliteConnection(Settings.DatabaseConnection))
				{
					connection.Open();
					using (SqliteCommand command = new SqliteCommand(connection))
					{
						command.CommandText = "SELECT q.*,c.Name as categoryName,c.inbuilt as inbuilt,c.active as active FROM "+
												"questions q, categories c WHERE c.id = q.categoryid";

						using (SqliteDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								Question question = new Question();
								question.Id = Convert.ToInt32(reader["id"]);
								question.Title = (string)reader["title"];

								question.Category = new Category();
								question.Category.Id = Convert.ToInt32(reader["categoryid"]);
								question.Category.InBuilt = Convert.ToBoolean(reader["inbuilt"]);
								question.Category.Active = Convert.ToBoolean(reader["active"]);
								question.Category.Name = (string)reader["categoryName"];
								
								question.Answer = (string)reader["answer"];
								question.AskCount = Convert.ToInt32(reader["askcount"]);
								question.EasinessFactor = Convert.ToDouble(reader["easinessfactor"]);
								question.Interval = Convert.ToInt32(reader["interval"]);
								question.LastAsked = new DateTime(Convert.ToInt64(reader["lastasked"]));
								question.NextAskOn = new DateTime(Convert.ToInt64(reader["nextaskon"]));
								question.Order = Convert.ToInt32(reader["order"]);
								question.PreviousInterval = Convert.ToInt32(reader["previousinterval"]);
								question.ResponseQuality = Convert.ToInt32(reader["responsequality"]);							

								list.Add(question);
							}
						}
					}
				}
			}
			catch (SqliteException e)
			{
				Logger.Warn("SqliteException occurred with ListQuestions(): \n{0}", e);
			}

			return list;
		}

		public IList<Question> QuestionsForCategory(Category category)
		{
			IList<Question> list = new List<Question>();

			try
			{
				using (SqliteConnection connection = new SqliteConnection(Settings.DatabaseConnection))
				{
					connection.Open();
					using (SqliteCommand command = new SqliteCommand(connection))
					{
						command.CommandText = "SELECT * FROM questions WHERE categoryid = @categoryid";
						SqliteParameter parameter = new SqliteParameter("@categoryid", DbType.Int32);
						parameter.Value = category.Id;
						command.Parameters.Add(parameter);

						using (SqliteDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								Question question = new Question();
								question.Id = Convert.ToInt32(reader["id"]);
								question.Title = (string)reader["title"];

								question.Category = category;

								question.Answer = (string)reader["answer"];
								question.AskCount = Convert.ToInt32(reader["askcount"]);
								question.EasinessFactor = Convert.ToDouble(reader["easinessfactor"]);
								question.Interval = Convert.ToInt32(reader["interval"]);
								question.LastAsked = new DateTime(Convert.ToInt64(reader["lastasked"]));
								question.NextAskOn = new DateTime(Convert.ToInt64(reader["nextaskon"]));
								question.Order = Convert.ToInt32(reader["order"]);
								question.PreviousInterval = Convert.ToInt32(reader["previousinterval"]);
								question.ResponseQuality = Convert.ToInt32(reader["responsequality"]);

								list.Add(question);
							}
						}
					}
				}
			}
			catch (SqliteException e)
			{
				Logger.Warn("SqliteException occurred with QuestionsForCategory({0}): \n{1}", category.Id,e);
			}

			return list;
		}

		public Question ReadQuestion(int id)
		{
			Question question = new Question();

			try
			{
				using (SqliteConnection connection = new SqliteConnection(Settings.DatabaseConnection))
				{
					connection.Open();
					using (SqliteCommand command = new SqliteCommand(connection))
					{
						command.CommandText = "SELECT q.*,c.Name as categoryName,c.inbuilt as inbuilt,c.active as active"+
											  "FROM questions q, categories c WHERE c.id = q.categoryid AND id=@id";
						SqliteParameter parameter = new SqliteParameter("@id", DbType.Int32);
						parameter.Value = id;
						command.Parameters.Add(parameter);

						using (SqliteDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								question.Id = Convert.ToInt32(reader["id"]);
								question.Title = (string)reader["title"];

								question.Category = new Category();
								question.Category.Id = Convert.ToInt32(reader["categoryid"]);
								question.Category.InBuilt = Convert.ToBoolean(reader["inbuilt"]);
								question.Category.Active = Convert.ToBoolean(reader["active"]);
								question.Category.Name = (string)reader["categoryName"];

								question.Answer = (string)reader["answer"];
								question.AskCount = Convert.ToInt32(reader["askcount"]);
								question.EasinessFactor = Convert.ToDouble(reader["easinessfactor"]);
								question.Interval = Convert.ToInt32(reader["interval"]);
								question.LastAsked = new DateTime(Convert.ToInt64(reader["lastasked"]));
								question.NextAskOn = new DateTime(Convert.ToInt64(reader["nextaskon"]));
								question.Order = Convert.ToInt32(reader["order"]);
								question.PreviousInterval = Convert.ToInt32(reader["previousinterval"]);
								question.ResponseQuality = Convert.ToInt32(reader["responsequality"]);
							}
						}
					}
				}
			}
			catch (SqliteException e)
			{
				Logger.Warn("SqliteException occurred with ReadQuestion({0}): \n{1}", id,e);
			}

			return question;
		}
		public int SaveQuestion(Question question)
		{
			try
			{
				using (SqliteConnection connection = new SqliteConnection(Settings.DatabaseConnection))
				{
					bool updating = (question.Id > 0);

					connection.Open();
					using (SqliteCommand command = new SqliteCommand(connection))
					{
						SqliteParameter parameter;
						string sql = @"INSERT INTO questions (answer,askcount,categoryid,easinessfactor,interval,lastasked,nextaskon,[order],previousinterval,responsequality,title) ";
						sql += "VALUES (@answer,@askcount,@categoryid,@easinessfactor,@interval,@lastasked,@nextaskon,@order,@previousinterval,@responsequality,@title);SELECT last_insert_rowid();";

						if (updating)
						{
							sql = @"UPDATE questions SET answer=@answer,askcount=@askcount,categoryid=@categoryid,easinessfactor=@easinessfactor,interval=@interval,";
							sql += "lastasked=@lastasked,nextaskon=@nextaskon,[order]=@order,previousinterval=@previousinterval,responsequality=@responsequality,title=@title ";
							sql += "WHERE id=@id";

							parameter = new SqliteParameter("@id", DbType.Int32);
							parameter.Value = question.Id;
							command.Parameters.Add(parameter);
						}

						parameter = new SqliteParameter("@title", DbType.String);
						parameter.Value = question.Title;
						command.Parameters.Add(parameter);

						parameter = new SqliteParameter("@answer", DbType.String);
						parameter.Value = question.Answer;
						command.Parameters.Add(parameter);

						parameter = new SqliteParameter("@askcount", DbType.Int32);
						parameter.Value = question.AskCount;
						command.Parameters.Add(parameter);

						parameter = new SqliteParameter("@categoryid", DbType.Int32);
						parameter.Value = question.Category.Id;
						command.Parameters.Add(parameter);

						parameter = new SqliteParameter("@easinessfactor", DbType.Double);
						parameter.Value = question.EasinessFactor;
						command.Parameters.Add(parameter);

						parameter = new SqliteParameter("@interval", DbType.Int32);
						parameter.Value = question.Interval;
						command.Parameters.Add(parameter);

						parameter = new SqliteParameter("@lastasked", DbType.Int64);
						parameter.Value = question.LastAsked.Ticks;
						command.Parameters.Add(parameter);

						parameter = new SqliteParameter("@nextaskon", DbType.Int64);
						parameter.Value = question.NextAskOn.Ticks;
						command.Parameters.Add(parameter);

						parameter = new SqliteParameter("@order", DbType.Int32);
						if (updating)
						{
							parameter.Value = question.Order;
						}
						else
						{
							parameter.Value = MaxQuestionOrder(question.Category) +1;
						}
						command.Parameters.Add(parameter);

						parameter = new SqliteParameter("@previousinterval", DbType.Int32);
						parameter.Value = question.PreviousInterval;
						command.Parameters.Add(parameter);

						parameter = new SqliteParameter("@responsequality", DbType.Int32);
						parameter.Value = question.ResponseQuality;
						command.Parameters.Add(parameter);

						command.CommandText = sql;

						int result;

						if (updating)
						{
							command.ExecuteNonQuery();
							result = question.Id;
						}
						else
						{
							Int64 newId = (Int64)command.ExecuteScalar();
							result = Convert.ToInt32(newId);
						}

						return result;
					}
				}
			}
			catch (SqliteException e)
			{
				Logger.Warn("SqliteException occurred with SaveQuestion({0}): \n{1}", question.Id, e);
			}

			return 0;
		}

		public int MaxQuestionOrder(Category category)
		{
			int result = 0;

			try
			{
				using (SqliteConnection connection = new SqliteConnection(Settings.DatabaseConnection))
				{
					connection.Open();
					using (SqliteCommand command = new SqliteCommand(connection))
					{
						command.CommandText = @"SELECT MAX([order]) from questions WHERE categoryid=@categoryid";

						SqliteParameter parameter = new SqliteParameter("@categoryid", DbType.Int32);
						parameter.Value = category.Id;
						command.Parameters.Add(parameter);

						object val = command.ExecuteScalar();
						if (val != DBNull.Value)
							result = Convert.ToInt32(val);
					}
				}
			}
			catch (SqliteException ex)
			{
				Logger.Fatal("Unable to perform MaxQuestionOrder: \n{0}", ex);
			}

			return result;
		}

		public void MoveQuestion(Question question, int newIndex)
		{
			int currentIndex = 0; // not the order, the list index.

			// All questions, find the question's index
			IList<Question> questions = ListQuestions().OrderBy(q => q.Order).ToList();
			for (int i = 0; i < questions.Count; i++)
			{
				if (questions[i].Id == question.Id)
				{
					currentIndex = i;
					break;
				}
			}

			// Move all items after the new index up one
			var after = questions.Where(q => q.Order >= newIndex).ToList();
			for (int i = 0; i < after.Count; i++)
			{
				after[i].Order += 1;
			}

			questions[currentIndex].Order = newIndex;

			// Re-order
			var ordered = questions.OrderBy(q => q.Order).ToList();
			for (int i = 0; i < ordered.Count; i++)
			{
				ordered[i].Order = i;
			}

			try
			{
				using (SqliteConnection connection = new SqliteConnection(Settings.DatabaseConnection))
				{
					connection.Open();
					
					using (SqliteCommand command = new SqliteCommand(connection))
					{
						// TODO: put inside a transaction (not WinSQLite friendly)
						command.CommandText = @"UPDATE questions SET [order]=@order WHERE id=@id";

						foreach (Question questionItem in ordered)
						{
							SqliteParameter parameter = new SqliteParameter("@id", DbType.Int32);
							parameter.Value = questionItem.Id;
							command.Parameters.Add(parameter);
							
							parameter = new SqliteParameter("@order", DbType.Int32);
							parameter.Value = questionItem.Order;
							command.Parameters.Add(parameter);

							command.ExecuteNonQuery();
						}
					}
				}
			}
			catch (SqliteException ex)
			{
				Logger.Fatal("Unable to MoveQuestion: \n{0}", ex);
				throw;
			}
		}
		#endregion

		/// <summary>
		/// This is run each time the app runs, but won't do anything after the first run. 
		/// </summary>
		public void CreateDatabase()
		{
			try
			{
				if (!File.Exists(Settings.DatabaseFile))
				{
					string categoriesSql = "CREATE TABLE \"categories\" ("+
						"\"id\" INTEGER PRIMARY KEY AUTOINCREMENT," +
						"\"name\" TEXT NOT NULL," +
						"\"active\" INTEGER DEFAULT 0,"+
						"\"inbuilt\" INTEGER DEFAULT 0)";

					string questionsSql = "CREATE TABLE questions (" +
											"\"id\" INTEGER PRIMARY KEY AUTOINCREMENT," +
											"\"categoryid\" INTEGER," +
											"\"title\" TEXT," +
											"\"answer\" TEXT," +
											"\"order\" INTEGER DEFAULT 0," +
											"\"lastasked\" INTEGER DEFAULT 0," +
											"\"nextaskon\" INTEGER DEFAULT 0," +
											"\"previousinterval\" INTEGER DEFAULT 0," +
											"\"interval\" INTEGER DEFAULT 0," +
											"\"askcount\" INTEGER DEFAULT 0," +
											"\"responsequality\" INTEGER DEFAULT 0," +
											"\"easinessfactor\" REAL DEFAULT 0)";

					// Create the file
					SqliteConnection.CreateFile(Settings.DatabaseFile);

					// And the schema
					using (SqliteConnection connection = new SqliteConnection(Settings.DatabaseConnection))
					{
						connection.Open();
						using (SqliteCommand command = new SqliteCommand(connection))
						{
							command.CommandText = categoriesSql;
							command.ExecuteNonQuery();
							
							command.CommandText = questionsSql;
							command.ExecuteNonQuery();

							// Default category
							command.CommandText = Default.Sql();
							command.ExecuteNonQuery();
							
#if GERMAN
							command.CommandText = German.Sql();
							command.ExecuteNonQuery();				
#elif SPANISH
							command.CommandText = Spanish.Sql();
							command.ExecuteNonQuery();
#elif FRENCH
							command.CommandText = French.Sql();
							command.ExecuteNonQuery();
#endif
						}
					}
				}
			}
			catch (IOException ex)
			{
				Logger.Fatal("Unable to delete the database file {0}: \n{1}", Settings.DatabaseFile, ex);
				throw;
			}
			catch (SqliteException ex)
			{
				Logger.Fatal("Unable to create the database: \n{0}", ex);
				throw;
			}
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
