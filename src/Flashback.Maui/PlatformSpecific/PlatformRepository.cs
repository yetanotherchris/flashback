using Flashback.Core.Net.Repositories;

#if ANDROID
using Android.Content;
#elif IOS
using Foundation;
#elif WINDOWS
using Microsoft.Maui.Essentials;
#elif MACCATALYST
using Foundation;
#endif

namespace Flashback.Maui.PlatformSpecific
{
    public static class PlatformRepository
    {
        public static void InitializeRepository()
        {
            // Platform-specific initialization if needed
            // This method is now primarily for platform-specific setup
            // The actual repository instance is managed through DI in MauiProgram
        }
    }
}
