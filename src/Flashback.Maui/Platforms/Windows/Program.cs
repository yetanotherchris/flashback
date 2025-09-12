using Microsoft.UI.Xaml;
using Microsoft.Maui;

namespace Flashback.Maui.WinUI
{
    public class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Microsoft.UI.Xaml.Application.Start((p) =>
            {
                var context = new Microsoft.UI.Dispatching.DispatcherQueueSynchronizationContext(Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread());
                System.Threading.SynchronizationContext.SetSynchronizationContext(context);
                new App();
            });
        }
    }

    public class App : MauiWinUIApplication
    {
        protected override MauiApp CreateMauiApp() => Flashback.Maui.MauiProgram.CreateMauiApp();
    }
}