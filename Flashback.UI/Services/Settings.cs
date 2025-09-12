using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Maui.Storage;

namespace Flashback.UI.Services
{
    /// <summary>
    /// Application wide settings.
    /// </summary>
    public static class Settings
    {
        /// <summary>
        /// A version number for future updates to use. 
        /// </summary>
        public static string Version { get; set; } = "1.0";

        /// <summary>
        /// Whether this is the first time the app has run.
        /// </summary>
        public static bool IsFirstRun { get; set; } = true;
        
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

        /// <summary>
        /// Writes all preferences to storage.
        /// </summary>
        public static void Write()
        {
            try
            {
                Preferences.Set("firstrun", "false");
                Preferences.Set("version", Version);
            }
            catch (Exception e)
            {
                // Log error if needed
                Console.WriteLine($"An exception occurred while writing the settings: {e}");
            }
        }

        /// <summary>
        /// Reads the settings from storage.
        /// </summary>
        public static void Read()
        {
            try
            {
                Version = Preferences.Get("version", "1.0");
                string firstRun = Preferences.Get("firstrun", "true");
                IsFirstRun = firstRun == "true";
            }
            catch (Exception e)
            {
                // Log error if needed
                Console.WriteLine($"An exception occurred while reading the settings: {e}");
            }
        }
    }
}
