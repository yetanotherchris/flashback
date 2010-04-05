using System;
using Vici.CoolStorage;
using System.IO;

namespace Flashback.Core
{
	public class Settings
	{
		public static void ConfigureDatabase()
		{
			string dbName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "mydb.db3");

			// The following line will tell CoolStorage where the database is,
			// create it if it does not exist, and call a delegate which
			// creates the necessary tables (only if the database file was
			// created new)

			CSConfig.SetDB(dbName, SqliteOption.CreateIfNotExists,() =>
			{
				CSDatabase.ExecuteNonQuery("CREATE TABLE \"categories\" ("+
                                  "pkid\" INTEGER PRIMARY KEY," +
								  "\"id\" GUID,"+
                                  "\"name\" TEXT NOT NULL)");


				CSDatabase.ExecuteNonQuery("CREATE TABLE questions ("+
										"\"pkid\" INTEGER PRIMARY KEY," +
										"\"id\" GUID," +
										"\"categoryid\" GUID," +
										"\"title\" TEXT," +
										"\"answer\" TEXT," +
										"\"order\" REAL," +
										"\"lastasked\" INTEGER," +
										"\"nextaskon\" INTEGER," +
										"\"interval\" INTEGER," +
										"\"askcount\" INTEGER," +
										"\"responsequality\" INTEGER," +
										"\"easinessfactor\" double)");

			});
		}
	}
}
