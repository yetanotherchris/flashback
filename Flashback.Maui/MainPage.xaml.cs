using Microsoft.Maui.Controls;
using Flashback.Core;
using System.Linq;

namespace Flashback.Maui
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            var questions = Question.List();
            QuestionsView.ItemsSource = questions;
        }
    }
}
