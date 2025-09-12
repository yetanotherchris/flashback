using Flashback.Core.Net.Repositories;
using Flashback.Maui.PlatformSpecific;

namespace Flashback.Maui
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            
            // Initialize the platform repository
            PlatformRepository.InitializeRepository();
            
            MainPage = new AppShell();
        }
    }
}
