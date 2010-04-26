using System;
using System.IO;

namespace Flashback.Core
{
	public class Repository
	{
		private static IRepository _instance;

		public static IRepository Default
		{
			get
			{
				if (_instance == null)
					throw new NullReferenceException("Repository.SetInstance() has not been set");

				return _instance;
			}
		}

		public static void SetInstance(IRepository instance, string dbFilename)
		{
		}
	}
}
