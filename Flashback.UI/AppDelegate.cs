using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoTouch.UIKit;
using Flashback.Core;
using MonoTouch.Foundation;
using Vici.CoolStorage;

namespace Flashback.UI
{
	[Register("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		private UIWindow _window;

		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			Settings.ConfigureDatabase();

			_window = new UIWindow(UIScreen.MainScreen.Bounds);
			_window.MakeKeyAndVisible();
			return true;
		}

		#region Testdata
		void AddQuestions()
		{
			Question question = new Question();
			question.Id = Guid.NewGuid();
			question.Title = "Capital of Ireland";
			question.Answer = "Dublin";
			question.Save();
		}

		// This method is required in iPhoneOS 3.0
		public override void OnActivated(UIApplication application)
		{
		}

		void QuestionsFor()
		{
			CSList<Category> categories = Category.List();
			foreach (Category category in categories)
			{
				Console.WriteLine(category.Name);
			}
		}

		void AddQuestions()
		{

			for (int i = 0; i < 15; i++)
			{
				Category category = Category.New();
				category.Name = string.Format("Category {0}", i);

				for (int n = 0; n < 20; n++)
				{
					Question question = Question.New();
					question.Title = string.Format("Question {0} for {1}", n, i);
					question.Answer = "Some answer that is about a sentence or two in length but not much longer really: a b c d e";
					question.Category = category;
					question.Save();
				}
			}
		}
		#endregion
	}
}
