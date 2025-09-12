using Flashback.Core.Net.Repositories;
using Flashback.Core;

namespace Flashback.Maui
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        private readonly IRepository _repository;

        public MainPage(IRepository repository)
        {
            InitializeComponent();
            _repository = repository;
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
                _ = Task.Run(() =>
                {
                    try
                    {
                        // Test that we can get categories (which will initialize DB if needed)
                        var categories = _repository.ListCategories();
                        
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            CounterBtn.Text = $"Database initialized! Found {categories.Count} categories. Clicked {count} times";
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
