using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Flashback.Core;

namespace Flashback.UI.Pages
{
    public partial class AddEditQuestionPage : ContentPage
    {
        private Question _question;
        private Category _category;

        public AddEditQuestionPage(Question question, Category category)
        {
            InitializeComponent();
            BindingContext = this;

            _question = question ?? new Question { Category = category };
            _category = category;
            QuestionTitle = _question.Title;
            QuestionAnswer = _question.Answer;
            Title = question == null ? "Add Question" : "Edit Question";
        }

        public string QuestionTitle { get; set; }
        public string QuestionAnswer { get; set; }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            // Any additional setup if needed
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(QuestionTitle) || string.IsNullOrEmpty(QuestionAnswer))
            {
                await DisplayAlert("Woops", "Please enter a question and its answer", "Close");
                return;
            }

            _question.Title = QuestionTitle;
            _question.Answer = QuestionAnswer;
            Question.Save(_question);

            // Reload parent data if needed (e.g., via MessagingCenter or event)
            await Navigation.PopAsync();
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
