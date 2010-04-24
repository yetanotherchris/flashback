using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoTouch.UIKit;
using Flashback.Core;
using System.IO;
using System.Reflection;
using MonoTouch.Foundation;

namespace Flashback.UI.Controllers
{
	public class CategoryHubController : UIViewController
	{
		private Category _category;
		private UIBarButtonItem _editCategoryButton;
		private UIBarButtonItem _editQuestionsButton;
        private UIBarButtonItem _calendarButton;

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
			
			Title = _category.Name;

			// Add the toolbar
			ToolbarItems = GetToolBar();
			NavigationController.ToolbarHidden = false;

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
		
		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear(animated);
			NavigationController.ToolbarHidden = false;
		}

		public UIBarButtonItem[] GetToolBar()
		{
			// Edit categories
			_editCategoryButton = new UIBarButtonItem();
			//_editCategoryButton.Image = UIImage.FromFile("Assets/Images/Toolbar/editcategories.png");
			_editCategoryButton.Title = "Edit categories";
			_editCategoryButton.Clicked += delegate
			{
				
			};

			// Edit questions
			_editQuestionsButton = new UIBarButtonItem();
			//_editQuestionsButton.Image = UIImage.FromFile("Assets/Images/Toolbar/editquestions.png");
			_editQuestionsButton.Title = "Edit questions";
			_editQuestionsButton.Clicked += delegate
			{
				QuestionsController controller = new QuestionsController(_category);
				NavigationController.PushViewController(controller,true);
			};

			// Calendar
			_calendarButton = new UIBarButtonItem();
			//_calenderButton.Image = UIImage.FromFile("Assets/Images/Toolbar/calendar.png");
			_calendarButton.Title = "Edit questions";
			_calendarButton.Clicked += delegate
			{
				
			};

			return new UIBarButtonItem[] { _editCategoryButton,_editQuestionsButton, _calendarButton};
		}
	}
}
