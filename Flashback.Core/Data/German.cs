#if GERMAN
using System;

namespace Flashback.Core.Data
{
	/// <summary>
	/// The install SQL for the German language pack.
	/// </summary>
	public class German
	{
		public static string Sql()
		{
			return "BEGIN TRANSACTION;COMMIT;";
		}
	}
}
#endif
