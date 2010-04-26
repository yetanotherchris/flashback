using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoTouch.UIKit;
using Flashback.Core;
using MonoTouch.Foundation;
using Flashback.UI.Controllers;

namespace Flashback.UI
{
	[Register("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		private UIWindow _window;
		private RootController _rootController;

		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			//Repository.Current.DeleteDatabase();
			Repository.Default.CreateDatabase();
			//Question.Save(new Question() {Id=1});
			
			_rootController = new RootController();

			_window = new UIWindow(UIScreen.MainScreen.Bounds);
			_window.Add(_rootController.View);
			_window.MakeKeyAndVisible();

			return true;
		}

		// This method is required in iPhoneOS 3.0
		public override void OnActivated(UIApplication application)
		{
		}
	}
}
