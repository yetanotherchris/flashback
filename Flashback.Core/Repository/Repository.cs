using System;
using System.IO;

namespace Flashback.Core
{
	/// <summary>
	/// A singleton for the currently loaded <see cref="IRepository"/> implementing class.
	/// </summary>
	public class Repository
	{
		private static IRepository _instance;

		/// <summary>
		/// The currently loaded repository. This should be set first using <see cref="Repository.SetInstance"/>
		/// </summary>
		public static IRepository Default
		{
			get
			{
				if (_instance == null)
					throw new NullReferenceException("Repository.SetInstance() has not been set");

				return _instance;
			}
		}

		/// <summary>
		/// Sets the current repository instance.
		/// </summary>
		/// <param name="instance"></param>
		public static void SetInstance(IRepository instance)
		{
			_instance = instance;
		}
	}
}
