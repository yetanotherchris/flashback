using System;
using System.IO;
using Mono.Data.Sqlite;
using System.Collections.Generic;

namespace Flashback.Core
{
	public class Repository
	{
		#region Singleton
		private static Repository _current = new Repository();

		public static Repository Current
	    {
	        get
	        {
				return _current;
	        }
	    }

		Repository()
		{
		}
		#endregion

		/// <summary>
		/// Creates the SQLite database if it doesn't already exist.
		/// </summary>
		public void CreateDatabase()
		{
			try
			{
				if (!File.Exists(Settings.DatabaseFile))
				{
					string categoriesSql = "CREATE TABLE \"categories\" (\"id\" INTEGER PRIMARY KEY AUTOINCREMENT,\"name\" TEXT NOT NULL)";
					string questionsSql = "CREATE TABLE questions (" +
											"\"id\" INTEGER PRIMARY KEY AUTOINCREMENT," +
											"\"categoryid\" INTEGER," +
											"\"title\" TEXT," +
											"\"answer\" TEXT," +
											"\"order\" REAL," +
											"\"lastasked\" INTEGER," +
											"\"nextaskon\" INTEGER," +
											"\"previousinterval\" INTEGER," +
											"\"interval\" INTEGER," +
											"\"askcount\" INTEGER," +
											"\"responsequality\" INTEGER," +
											"\"easinessfactor\" REAL)";

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
						}
					}
				}
			}
			catch (IOException ex)
			{
				Logger.Fatal("Unable to delete the database file {0}: \n{1}", Settings.DatabaseFile,ex);
				throw;
			}
			catch (SqliteException ex)
			{
				Logger.Fatal("Unable to create the database: \n{0}", ex);
				throw;
			}
		}

		public IList<Category> Categories()
		{
			return null;
		}
	}
}
