using Flashback.Core.Net.Repositories;
using Flashback.Maui.PlatformSpecific;
using Flashback.Core;
using Microsoft.Maui.Storage;
using System.IO;
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
            // Create a cross-platform database path and register a SqliteRepository that implements the legacy IRepository
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "flashback.db");

            // Ensure the platform-specific initializer (if present) runs
            try
            {
                PlatformRepository.InitializeRepository();
            }
            catch
            {
                // ignore if platform initializer isn't present on some targets
            }

            // Register the concrete SqliteRepository for DI
            builder.Services.AddSingleton<IRepository>(provider =>
            {
                var repo = new SqliteRepository(dbPath);
                // Create DB/schema if needed
                repo.CreateDatabase();
                // Bridge to legacy static Repository
                Repository.SetInstance(repo);
                return repo;
            });

            return builder.Build();
        }
    }
}
