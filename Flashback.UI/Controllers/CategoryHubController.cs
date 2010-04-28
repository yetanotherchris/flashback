using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoTouch.UIKit;
using Flashback.Core;
using System.IO;
using System.Reflection;
using MonoTouch.Foundation;
using System.Drawing;

namespace Flashback.UI.Controllers
{
	public class CategoryHubController : UIViewController
	{
		private Category _category;
		private UIBarButtonItem _editCategoryButton;
		private UIBarButtonItem _editQuestionsButton;
        private UIBarButtonItem _calendarButton;

		private UILabel _labelQuestionsToday;
		private UIButton _buttonStart;

		private AddEditCategoryController _addEditCategoryController;
		private QuestionsController _questionsController;
		private CalendarController _calendarController;

		public CategoryHubController(Category category)
		{
			_category = category;
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			// Add the toolbar
			ToolbarItems = GetToolBar();
			NavigationController.ToolbarHidden = false;

			_labelQuestionsToday = new UILabel();
			_labelQuestionsToday.Text = string.Format("{0} questions due today",Question.DueToday().ToList().Count);
			_labelQuestionsToday.Font = UIFont.SystemFontOfSize(16f);
			_labelQuestionsToday.TextColor = UIColor.Gray;
			_labelQuestionsToday.Frame = new RectangleF(15, 30, 290, 50);
			_labelQuestionsToday.BackgroundColor = UIColor.Clear;
			_labelQuestionsToday.Lines = 2;
			View.AddSubview(_labelQuestionsToday);

			// Start button
			_buttonStart = UIButton.FromType(UIButtonType.RoundedRect);
			_buttonStart.SetTitle("Start", UIControlState.Normal);
			_buttonStart.BackgroundColor = UIColor.Red;
			_buttonStart.SetTitleColor(UIColor.White,UIControlState.Normal);
			_buttonStart.Frame = new RectangleF(15, 90, 100, 25);
			_buttonStart.TouchDown += delegate
			{
				AnswerQuestionsController controller = new AnswerQuestionsController(_category);
				NavigationController.PushViewController(controller, true);
			};
			View.AddSubview(_buttonStart);

			// The webview		
			/*
			UIWebView webView = new UIWebView();
			webView.Frame = new System.Drawing.RectangleF(0,0,380,420);
			
			string template = "";
			Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Flashback.Assets.HTML.calendar.html");
			using (StreamReader reader = new StreamReader(stream))
			{
				template = reader.ReadToEnd();
			}
			webView.LoadHtmlString(template,new NSUrl("/"));
			View.AddSubview(webView);*/
		}
		
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			Title = _category.Name;
		}
		
		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear(animated);
			NavigationController.ToolbarHidden = false;
		}

		private UIBarButtonItem[] GetToolBar()
		{
			// Edit category
			_editCategoryButton = new UIBarButtonItem();
			_editCategoryButton.Image = UIImage.FromFile("Assets/Images/Toolbar/toolbar_edit.png");
			_editCategoryButton.Title = "Edit category";
			_editCategoryButton.Clicked += delegate
			{
				AddEditCategoryController controller = new AddEditCategoryController(_category);
				NavigationController.PushViewController(controller, false);
			};

			// Manage questions
			_editQuestionsButton = new UIBarButtonItem();
			_editQuestionsButton.Image = UIImage.FromFile("Assets/Images/Toolbar/toolbar_questions.png");
			_editQuestionsButton.Title = "Manage questions";
			_editQuestionsButton.Clicked += delegate
			{
				QuestionsController controller = new QuestionsController(_category);
				NavigationController.PushViewController(controller, true);
			};

			// Calendar
			_calendarButton = new UIBarButtonItem();
			_calendarButton.Image = UIImage.FromFile("Assets/Images/Toolbar/toolbar_calendar.png");
			_calendarButton.Title = "Calendar";
			_calendarButton.Clicked += delegate
			{
				CalendarController controller = new CalendarController();
				NavigationController.PushViewController(controller, true);
			};

			return new UIBarButtonItem[] { _editCategoryButton, _editQuestionsButton, _calendarButton };
		}
	}
}
