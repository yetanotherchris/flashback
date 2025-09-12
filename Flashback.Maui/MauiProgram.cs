using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Flashback.Core;

namespace Flashback.Maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts => { });

            // register the in-memory repository as the repository implementation
            var repo = new InMemoryRepository();
            repo.CreateDatabase();
            Repository.SetInstance(repo);

            return builder.Build();
        }
    }
}
