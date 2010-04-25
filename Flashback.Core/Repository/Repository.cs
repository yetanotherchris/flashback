using System;
using System.IO;
using System.Collections.Generic;
using SQLite;

#if MONOTOUCH
//using Mono.Data.Sqlite;
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

		/// <summary>
		/// Creates the SQLite database if it doesn't already exist.
		/// </summary>
		public void CreateDatabase()
		{
			SQLiteConnection connection = new SQLiteConnection(Settings.DatabaseFile);
			connection.CreateTable<Category>();
			connection.CreateTable<Question>();
			
			return;
			/*
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
											"\"order\" INTEGER," +
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
							
							// Default category
							command.CommandText = "insert into categories (name) values ('german')";
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
			}*/
		}
	}
}
