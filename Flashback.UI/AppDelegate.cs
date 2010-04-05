using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoTouch.UIKit;
using Flashback.Core;
using MonoTouch.Foundation;

namespace Flashback.UI
{
	[Register("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		private UIWindow _window;

		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			_window = new UIWindow(UIScreen.MainScreen.Bounds);
			_window.MakeKeyAndVisible();
			return true;
		}

		// This method is required in iPhoneOS 3.0
		public override void OnActivated(UIApplication application)
		{
		}
	}
}
