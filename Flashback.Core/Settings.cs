using System;
using System.IO;
namespace Flashback.Core
{
	public class Settings
	{
		public static string DatabaseFile
		{
			get
			{
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "flashback.db");
			}
		}

		public static string DatabaseConnection
		{
			get
			{
				return string.Format("Data Source={0};Version=3;", DatabaseFile);
			}
		}

		public static string LogFile
		{
			get
			{
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "flashback.log");
			}
		}

	}
}
