using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
	/// <summary>
	/// The category for a question.
	/// </summary>
	public partial class Category : BaseDataObject<Category>
	{
		/// <summary>
		/// The name of the category
		/// </summary>
		public string Name { get; set; }

		public Category()
		{

		}
	}
}
