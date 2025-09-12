using Flashback.Core.Net.Repositories;
using Flashback.Maui.PlatformSpecific;
using Microsoft.Extensions.Logging;

namespace Flashback.Maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            // Register services
            builder.Services.AddSingleton<ISqliteRepository>(provider =>
            {
                PlatformRepository.InitializeRepository();
                return SqliteRepository.Instance;
            });

            return builder.Build();
        }
    }
}
