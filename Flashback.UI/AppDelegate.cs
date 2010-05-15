using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoTouch.UIKit;
using Flashback.Core;
using MonoTouch.Foundation;
using Flashback.UI.Controllers;
using Flashback.Core.iPhone;

namespace Flashback.UI
{
	[Register("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		private UIWindow _window;
		private RootController _rootController;
		private SqliteRepository _sqliteRepository;

		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			// Set the repository type
			_sqliteRepository = new SqliteRepository();
			Repository.SetInstance(_sqliteRepository);
			Repository.Default.CreateDatabase();
			
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
		
		public override void WillTerminate (UIApplication application)
		{
			UpdateApplicationBadge();
		}
		
		private void UpdateApplicationBadge()
		{
			UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;	
			
			IList<Question> questions = Question.List();
			int dueTodayCount = Question.ActiveDueToday(questions).ToList().Count;	
			UIApplication.SharedApplication.ApplicationIconBadgeNumber = dueTodayCount;
		}
	}
}
