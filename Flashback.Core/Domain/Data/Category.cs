using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

#if MONOTOUCH
using Mono.Data.Sqlite;
#endif

#if WINDOWS
using SqliteDataReader = System.Data.SQLite.SQLiteDataReader;
using SqliteCommand = System.Data.SQLite.SQLiteCommand;
using SqliteConnection = System.Data.SQLite.SQLiteConnection;
using SqliteParameter = System.Data.SQLite.SQLiteParameter;
using SqliteException = System.Data.SQLite.SQLiteException;
#endif

namespace Flashback.Core
{
	public partial class Category : BaseDataObject<Category>
	{
		protected override string TableName
		{
			get
			{
				return "categories";
			}
		}

		protected override Category GetRow(SqliteDataReader reader)
		{
			Category category = new Category();
			category.Id = reader.GetInt32(0);
			category.Name = reader.GetString(1);

			return category;
		}

		protected override int Save(SqliteCommand command, bool updating)
		{
			SqliteParameter parameter;
			string sql = @"INSERT INTO categories (name) VALUES (@name);SELECT last_insert_rowid();";

			if (updating)
			{
				sql = @"UPDATE categories SET name=@name WHERE id=@id";

				parameter = new SqliteParameter("@id", DbType.Int64);
				parameter.Value = Id;
				command.Parameters.Add(parameter);
			}

			parameter = new SqliteParameter("@name", DbType.String);
			parameter.Value = Name;
			command.Parameters.Add(parameter);

			command.CommandText = sql;

			if (updating)
			{
				command.ExecuteNonQuery();
				return Id;
			}
			else
			{
				Int64 newId = (Int64)command.ExecuteScalar();
				return Convert.ToInt32(newId);
			}
		}
		
		/// <summary>
		/// Removes the Category and all questions for it
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static new void Delete(int id)
		{
			try
			{
				using (SqliteConnection connection = new SqliteConnection(Settings.DatabaseConnection))
				{
					connection.Open();
					using (SqliteCommand command = new SqliteCommand(connection))
					{
						command.CommandText = "DELETE FROM categories WHERE id=@id";
						
						SqliteParameter parameter = new SqliteParameter("@id", DbType.Int64);
						parameter.Value = id;
						command.Parameters.Add(parameter);
						
						command.ExecuteNonQuery();
						
						command.CommandText = "DELETE FROM questions WHERE categoryid=@id";
						command.ExecuteNonQuery();
					}
				}
			}
			catch (SqliteException e)
			{
				Logger.Warn("SqliteException occured with Delete({0}) for Category: \n{2}", id, e);
			}
		}
	}
}
