using Flashback.Core.Net.Repositories;

namespace Flashback.Maui
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);

            // Test repository initialization
            if (count == 1)
            {
                _ = Task.Run(async () =>
                {
                    try
                    {
                        var repo = SqliteRepository.Instance;
                        await repo.InitializeAsync();
                        
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            CounterBtn.Text = $"Database initialized! Clicked {count} times";
                        });
                    }
                    catch (Exception ex)
                    {
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            CounterBtn.Text = $"Error: {ex.Message}";
                        });
                    }
                });
            }
        }
    }
}
