using Microsoft.Maui.Controls;

namespace Flashback.UI.Controllers
{
	/// <summary>
	/// The page that is first shown when the app launches.
	/// </summary>
	public class RootPage : ContentPage
	{
		private CategoriesPage _categoriesPage;

		/// <summary>
		/// Initialises the page and pushes the categories page.
		/// </summary>
		public RootPage()
		{
			_categoriesPage = new CategoriesPage();
			Navigation.PushAsync(_categoriesPage, false);
		}
	}
}
