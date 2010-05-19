using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flashback.UI
{
	/// <summary>
	/// Application settings.
	/// </summary>
	public class Settings
	{
		public string Version { get; set; }

		/// <summary>
		/// Todo: save to settings
		/// </summary>
		public bool IsFirstRun { get; set; }

		#region Edition specific settings
		/// <summary>
		/// 
		/// </summary>
		public static bool IsFullVersion
		{
			get
			{
#if FULLVERSION
				return true;
#else
				return false;
#endif
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public static bool IsGerman
		{
			get
			{
#if GERMAN
				return true;
#else
				return false;
#endif
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public static bool IsSpanish
		{
			get
			{
#if SPANISH
				return true;
#else
				return false;
#endif
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public static bool IsFrench
		{
			get
			{
#if FRENCH
				return true;
#else
				return false;
#endif
			}
		}
		#endregion
	}
}
