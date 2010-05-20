using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoTouch.Foundation;
using Flashback.Core;

namespace Flashback.UI
{
	/// <summary>
	/// Application settings.
	/// </summary>
	public class Settings
	{
		public static string Version { get; set; }

		/// <summary>
		/// Todo: save to settings
		/// </summary>
		public static bool IsFirstRun { get; set; }
		
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

		static Settings()
		{
			Version = "1.0";
			IsFirstRun = true;
		}

		/// <summary>
		/// Writes all NSUserDefaults to the info.plist file.
		/// </summary>
		public static void Write()
		{
			try
			{
				NSUserDefaults.StandardUserDefaults.SetBool(false, "firstrun");
				NSUserDefaults.StandardUserDefaults.SetString(Version, "version");

				NSUserDefaults.StandardUserDefaults.Synchronize();
			}
			catch (Exception e)
			{
				Logger.Warn("An exception occured while writing the settings: \n{0}", e);
			}
		}

		/// <summary>
		/// Reads the settings from the info.plist.
		/// </summary>
		public static void Read()
		{

			try
			{
				var defaults = NSUserDefaults.StandardUserDefaults;

				string version = defaults.StringForKey("version");
				if (string.IsNullOrEmpty(version))
					Version = version;

				bool firstrun = defaults.BoolForKey("firstrun");
			}
			catch (Exception e)
			{
				Logger.Warn("An exception occured while reading the settings: \n{0}", e);
			}
		}
	}
}
