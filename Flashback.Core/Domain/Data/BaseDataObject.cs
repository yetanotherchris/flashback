using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Data.Sqlite;
using System.Data;

namespace Flashback.Core
{
	public abstract class BaseDataObject<T> where T : BaseDataObject<T>
	{
		/// <summary>
		/// Retrieves the id for the object (read-only).
		/// </summary>
		public virtual int Id { get; protected set; }

		/// <summary>
		/// The table name for the domain object (read-only).
		/// </summary>
		protected abstract string TableName { get; }

		/// <summary>
		/// Creates a new instance of the object and fills its properties  using the reader.
		/// </summary>
		/// <param name="reader"></param>
		/// <returns></returns>
		protected abstract T GetRow(SqliteDataReader reader);

		/// <summary>
		/// Inserts/updates an instance of the object, executing with the command.
		/// </summary>
		/// <param name="command"></param>
		/// <returns>The id of the object</returns>
		protected abstract int Save(SqliteCommand command,bool updating);

		/// <summary>
		/// Saves the instance by either adding it to the database, or updating it.
		/// </summary>
		public void Save()
		{
			bool updating = (Id > 0);

			try
			{
				using (SqliteConnection connection = new SqliteConnection(Settings.DatabaseConnection))
				{
					connection.Open();
					using (SqliteCommand command = new SqliteCommand(connection))
					{
						Id = Save(command, updating);
					}
				}
			}
			catch (SqliteException e)
			{
				Logger.Warn("SqliteException occured while Saving an instance of {0} (updating={1}): \n{2}", typeof(T).Name, updating, e);
			}
		}

		/// <summary>
		/// Retrieves a single instance of the object based on the id.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static T Read(int id)
		{
			T defaultInstance = default(T);

			try
			{
				using (SqliteConnection connection = new SqliteConnection(Settings.DatabaseConnection))
				{
					connection.Open();
					using (SqliteCommand command = new SqliteCommand(connection))
					{
						command.CommandText = string.Format("SELECT * FROM {0} WHERE id=@id", defaultInstance.TableName);

						SqliteParameter parameter = new SqliteParameter("@id", DbType.Int32);
						parameter.Value = id;
						command.Parameters.Add(parameter);

						using (SqliteDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								defaultInstance = defaultInstance.GetRow(reader);
							}
						}
					}
				}
			}
			catch (SqliteException e)
			{
				Logger.Warn("SqliteException occured with Read({0}) for {1}: \n{2}", id,typeof(T).Name, e);
			}

			return defaultInstance;
		}

		/// <summary>
		/// Lists all objects in the database.
		/// </summary>
		/// <returns></returns>
		public static IList<T> List()
		{
			IList<T> list = new List<T>();
			T defaultInstance = default(T);

			try
			{
				using (SqliteConnection connection = new SqliteConnection(Settings.DatabaseConnection))
				{
					connection.Open();
					using (SqliteCommand command = new SqliteCommand(connection))
					{
						command.CommandText = string.Format("SELECT * FROM {0}", defaultInstance.TableName);

						using (SqliteDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								T instance = defaultInstance.GetRow(reader);
								list.Add(instance);
							}
						}
					}
				}
			}
			catch (SqliteException e)
			{
				Logger.Warn("SqliteException occured while getting a List<{0}>: \n{1}", typeof(T).Name, e);
			}

			return list;
		}
	}
}
