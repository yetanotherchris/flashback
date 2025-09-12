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
            // For now, just set the cross-platform repository instance
            SqliteRepository.Instance = new SqliteRepository();
        }
    }
}
