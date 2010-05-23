using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoTouch.UIKit;

namespace Flashback.UI.Controllers
{
	/// <summary>
	/// The controller that is first shown when the app launches.
	/// </summary>
	public class RootController : UINavigationController
	{
		private CategoriesController _categoriesController;

		/// <summary>
		/// Pushes the <see cref="CategoriesController"/> when loaded.
		/// </summary>
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			_categoriesController = new CategoriesController();
			PushViewController(_categoriesController, false);

			Toolbar.BarStyle = UIBarStyle.Black;
			NavigationBar.BarStyle = UIBarStyle.Black;			
		}
	}
}
