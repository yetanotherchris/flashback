using System;
using System.IO;
namespace Flashback.Core
{
	/// <summary>
	/// Contains all settings for the Core namespace.
	/// </summary>
	public class Settings
	{
		/// <summary>
		/// The path and filename of the database file.
		/// </summary>
		public static string DatabaseFile
		{
			get
			{
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "flashback.db");
			}
		}

		/// <summary>
		/// The full connection string for the database file.
		/// </summary>
		public static string DatabaseConnection
		{
			get
			{
				return string.Format("Data Source={0};Version=3;", DatabaseFile);
			}
		}

		/// <summary>
		/// The log file to write to, which is disabled for release mode.
		/// </summary>
		public static string LogFile
		{
			get
			{
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "flashback.log");
			}
		}
	}
}
