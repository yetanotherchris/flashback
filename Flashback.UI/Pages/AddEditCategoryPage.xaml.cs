using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Flashback.Core;

namespace Flashback.UI.Pages
{
    public partial class AddEditCategoryPage : ContentPage
    {
        private Category _category;

        public AddEditCategoryPage(Category category = null)
        {
            InitializeComponent();
            BindingContext = this;

            _category = category ?? new Category();
            CategoryName = _category.Name;
            IsActive = _category.Active;
            Title = category == null ? "Add Category" : "Edit Category";
        }

        public string CategoryName { get; set; }
        public bool IsActive { get; set; }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            // Any additional setup if needed
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(CategoryName))
            {
                await DisplayAlert("Woops", "Please enter your category name", "Close");
                return;
            }

            _category.Name = CategoryName;
            _category.Active = IsActive;
            Category.Save(_category);

            await Navigation.PopAsync();
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
