using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoTouch.Foundation;
using Flashback.Core;

namespace Flashback.UI
{
	/// <summary>
	/// Application wide settings.
	/// </summary>
	public class Settings
	{
		/// <summary>
		/// A version number for future updates to use. 
		/// </summary>
		public static string Version { get; set; }

		/// <summary>
		/// Whether this is the first time the app has run.
		/// </summary>
		public static bool IsFirstRun { get; set; }
		
		#region Edition specific settings, for the help files
		/// <summary>
		/// Whether this is the lite or full edition.
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
		/// The German edition of the app.
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
		/// The Spanish edition of the app.
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
		/// The French edition of the app.
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
				NSUserDefaults.StandardUserDefaults.SetString("false", "firstrun");
				NSUserDefaults.StandardUserDefaults.SetString(Version, "version");

				NSUserDefaults.StandardUserDefaults.Synchronize();
			}
			catch (Exception e)
			{
				Logger.Warn("An exception occurred while writing the settings: \n{0}", e);
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
				if (!string.IsNullOrEmpty(version))
					Version = version;

				// A work around as bool fields will always be false if they don't exist
				string firstRun = defaults.StringForKey("firstrun");
				if (!string.IsNullOrEmpty(firstRun))
					IsFirstRun = false;
			}
			catch (Exception e)
			{
				Logger.Warn("An exception occurred while reading the settings: \n{0}", e);
			}
		}
	}
}
