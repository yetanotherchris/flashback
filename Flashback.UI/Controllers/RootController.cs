using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoTouch.UIKit;

namespace Flashback.UI.Controllers
{
	// * UINavigationController with no toolbar
	// * First controller is categories
	// * Title bar is the app name.
	public class RootController : UINavigationController
	{
		private CategoriesTableController _categoriesController;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			_categoriesController = new CategoriesTableController();
			PushViewController(_categoriesController, false);

			// TODO: persist to last shown controller
		}
	}
}
