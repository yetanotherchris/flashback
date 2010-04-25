using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

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
	public abstract class BaseDataObject<T> where T : BaseDataObject<T>, new()
	{
		/// <summary>
		/// Retrieves the id for the object (read-only).
		/// </summary>
		public virtual int Id { get; internal set; }

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
		/// Removes a single instance of the object based on the id.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static void Delete(int id)
		{
			T defaultInstance = new T();

			try
			{
				using (SqliteConnection connection = new SqliteConnection(Settings.DatabaseConnection))
				{
					connection.Open();
					using (SqliteCommand command = new SqliteCommand(connection))
					{
						command.CommandText = string.Format("DELETE FROM {0} WHERE id=@id", defaultInstance.TableName);

						SqliteParameter parameter = new SqliteParameter("@id", DbType.Int64);
						parameter.Value = id;
						command.Parameters.Add(parameter);

						command.ExecuteNonQuery();
					}
				}
			}
			catch (SqliteException e)
			{
				Logger.Warn("SqliteException occured with Delete({0}) for {1}: \n{2}", id, typeof(T).Name, e);
			}
		}

		/// <summary>
		/// Retrieves a single instance of the object based on the id.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static T Read(int id)
		{
			T defaultInstance = new T();

			try
			{
				using (SqliteConnection connection = new SqliteConnection(Settings.DatabaseConnection))
				{
					connection.Open();
					using (SqliteCommand command = new SqliteCommand(connection))
					{
						command.CommandText = string.Format("SELECT * FROM {0} WHERE id=@id", defaultInstance.TableName);

						SqliteParameter parameter = new SqliteParameter("@id", DbType.Int64);
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
			T defaultInstance = new T();

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

		/// <summary>
		/// Lists all instances of the object, using the filters provided, and using 'OR' or 'AND' in the where clause.
		/// </summary>
		/// <remarks>The filters can be passed like so: List(true,"@id",1,"@name",name) - the @ is optional but
		/// allows the callers to clearly see which is the parameter.</remarks>
		public static IList<T> List(bool useAnd,params object[] filters)
		{
			if (filters == null || filters.Length == 0)
				throw new ArgumentException("The filters parameter is null or zero length for List()");
			if (filters.Length % 2 != 0)
				throw new ArgumentException("Mismatch on the number of filters for List() - use Name/Value.");

			IList<T> list = new List<T>();
			T defaultInstance = new T();

			try
			{
				using (SqliteConnection connection = new SqliteConnection(Settings.DatabaseConnection))
				{
					connection.Open();
					using (SqliteCommand command = new SqliteCommand(connection))
					{
						// Save the key/values for the SqliteParameters
						Dictionary<string, object> columns = new Dictionary<string, object>();
						for (int i = 0; i < filters.Length; i += 2)
						{
							string name = filters[i].ToString();
							object value = filters[i + 1];

							// Support @Property syntax though it's not really needed,
							// it makes the filter easier to read.
							if (name.StartsWith("@"))
								name = name.Remove(0, 1);

							columns.Add(name, value);
						}

						// Make up the list of predicates
						List<string> statements = new List<string>();
						foreach (string key in columns.Keys)
						{
							object value = columns[key];
							statements.Add(string.Format("{0}=@{0}", key));

							SqliteParameter parameter = new SqliteParameter("@" + key, ToDbType(value));
							parameter.Value = value;
							command.Parameters.Add(parameter);
						}

						// Join up the statements
						string seperator = " OR ";
						if (useAnd)
							seperator = " AND ";
						string predicate = string.Join(seperator,statements.ToArray());

						string sql = string.Format("SELECT * FROM {0} WHERE {1}", defaultInstance.TableName,predicate);
						command.CommandText = sql;
						
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

		/// <summary>
		/// Converts string,int32,bool and double into their SQLite representation. Defaults to string.
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		private static DbType ToDbType(object o)
		{
			if (o is string)
				return DbType.String;
			else if (o is int || o is bool)
				return DbType.Int32;
			else if (o is double || o is float || o is decimal)
				return DbType.Double;

			return DbType.String;
		}
	}
}
