using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoTouch.UIKit;

namespace Flashback.UI.Controllers
{
	public class RootController : UINavigationController
	{
		private CategoriesController _categoriesController;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			_categoriesController = new CategoriesController();
			PushViewController(_categoriesController, false);

			Toolbar.BarStyle = UIBarStyle.Black;
			NavigationBar.BarStyle = UIBarStyle.Black;
			
			// TODO: persist to last shown controller
		}
	}
}
