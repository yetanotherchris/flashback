#if GERMAN
using System;

namespace Flashback.Core.Data
{
	public class German
	{
		public static string Sql()
		{
			return "BEGIN TRANSACTION;COMMIT;";
		}
	}
}
#endif
