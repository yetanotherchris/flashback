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
	/// <summary>
	/// Displays details about a category and lets the user start the test.
	/// </summary>
	public class CategoryHubController : UIViewController
	{
		private Category _category;
		private UIBarButtonItem _editCategoryButton;
		private UIBarButtonItem _editQuestionsButton;
        private UIBarButtonItem _calendarButton;

		private UILabel _labelQuestionsToday;
		private UILabel _labelNextDue;
		private UIButton _buttonStart;
		private UIButton _buttonReset;

		private AddEditCategoryController _addEditCategoryController;
		private QuestionsController _questionsController;
		private CalendarController _calendarController;
		
		private UIImage _bgImage;
		private UIImageView _bgImageView;

		/// <summary>
		/// Creates a new instance of <see cref="CategoryHubController"/>
		/// </summary>
		/// <param name="category"></param>
		public CategoryHubController(Category category)
		{
			_category = category;
		}
		
		/// <summary>
		/// Configures the toolbar buttons and the controls for the view.
		/// </summary>
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			Title = _category.Name;

			// Add the toolbar
			ToolbarItems = GetToolBar();
			NavigationController.ToolbarHidden = false;
			
			// Flashcard bg image
			_bgImage = UIImage.FromFile("Assets/Images/flashcardbg.jpg");
			_bgImageView = new UIImageView(_bgImage);
			_bgImageView.Frame = new RectangleF(0,0,320,480);
			View.AddSubview(_bgImageView);
			
			// Questions label
			_labelQuestionsToday = new UILabel();
			_labelQuestionsToday.Frame = new RectangleF(17, 15, 280, 50);
			_labelQuestionsToday.Font = UIFont.SystemFontOfSize(24f);
			_labelQuestionsToday.TextColor = UIColor.Black;
			_labelQuestionsToday.BackgroundColor = UIColor.Clear;
			_labelQuestionsToday.Text = "";	
			_labelQuestionsToday.Lines = 3;
			_labelQuestionsToday.TextAlignment = UITextAlignment.Center;
			View.AddSubview(_labelQuestionsToday);
			
			// Next due label
			_labelNextDue = new UILabel();
			_labelNextDue.Frame = new RectangleF(17, 45, 280, 100);
			_labelNextDue.Font = UIFont.SystemFontOfSize(16f);
			_labelNextDue.TextColor = UIColor.Gray;
			_labelNextDue.BackgroundColor = UIColor.Clear;
			_labelNextDue.Text = "";
			_labelNextDue.Lines = 3;
			_labelNextDue.TextAlignment = UITextAlignment.Center;
			View.AddSubview(_labelNextDue);

			// Start button
			UIImage startImage = UIImage.FromFile("Assets/Images/startbutton.png");			
			_buttonStart = new UIButton(new RectangleF(15, 225, 280, 48));
			_buttonStart.Font = UIFont.BoldSystemFontOfSize(20);
			_buttonStart.SetTitleColor(UIColor.White, UIControlState.Normal);
			_buttonStart.SetBackgroundImage(startImage,UIControlState.Normal);
			_buttonStart.BackgroundColor = UIColor.Clear;
			_buttonStart.TouchDown += delegate
			{
				// Start sets the category as active
				if (!_category.Active)
				{
					_category.Active = true;
					Category.Save(_category);
				}
				
				AnswerQuestionsController controller = new AnswerQuestionsController(_category);
				NavigationController.PushViewController(controller, true);
			};
			View.AddSubview(_buttonStart);
			
			// Reset button
			UIImage resetImage = UIImage.FromFile("Assets/Images/resetbutton.png");
			_buttonReset = new UIButton(new RectangleF(15, 275, 280, 48));
			_buttonReset.Font = UIFont.BoldSystemFontOfSize(20);
			_buttonReset.SetTitleColor(UIColor.White, UIControlState.Normal);
			_buttonReset.SetBackgroundImage(resetImage,UIControlState.Normal);
			_buttonReset.BackgroundColor = UIColor.Clear;
			
			_buttonReset.TouchDown += delegate
			{
				foreach (Question question in Question.ForCategory(_category))
				{
					question.Reset();
					Question.Save(question);
				}
				
				UIAlertView alert = new UIAlertView();
				alert.Title = "Reset";
				alert.Message = "All questions for the category have been reset";
				alert.AddButton("Close");
				alert.Show();
				alert.Dismissed += delegate {
					SetLabelTitles();
				};
			};
			View.AddSubview(_buttonReset);
			
			SetNonVisible();
		}
		
		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
			NavigationController.ToolbarHidden = false;

			SetLabelTitles();
			FadeIn();
		}
		
		private void FadeIn()
		{
			UIView.SetAnimationDelay(0.5);
			UIView.BeginAnimations(null);
			
			_labelQuestionsToday.Alpha = 1;
			_labelNextDue.Alpha = 1;
			_buttonStart.Alpha = 1;
			_buttonReset.Alpha = 1;

			UIView.CommitAnimations();
		}

		private void SetNonVisible()
		{
			_labelQuestionsToday.Alpha = 0;
			_labelNextDue.Alpha = 0;
			_buttonStart.Alpha = 0;
			_buttonReset.Alpha = 0;
		}
		
		private void SetLabelTitles()
		{
			// Update the questions label
			IList<Question> questions = Question.ForCategory(_category);
			int questionCount = questions.Count;
			int dueTodayCount = Question.DueToday(questions).ToList().Count;
			
			_labelNextDue.Text = "";
			_labelQuestionsToday.Text = "";			
			
			// If there are questions for the category then figure out the label.
			if (questionCount > 0)
			{
				_buttonStart.Enabled = true;
				_buttonReset.Enabled = true;

				string due = (dueTodayCount > 0) ? dueTodayCount.ToString() : "No";
				string s = (dueTodayCount > 1 || dueTodayCount == 0) ? "s" : "";
				_labelQuestionsToday.Text = string.Format("{0} question{1} due today.",due,s);
				
				if (dueTodayCount == 0)
				{
					_buttonStart.Enabled = false; // Is this the most intuitive behaviour?

					DateTime datetime = Question.NextDueDate(questions);
					string dateSuffix = DateSuffix(datetime.Day);
					_labelNextDue.Text = string.Format("Question{0} are next due on {1}{2} {3}.",s,datetime.ToString("dddd d"),dateSuffix,datetime.ToString("MMMM"));
				}
			}
			else
			{
				_buttonStart.Enabled = false;
				_buttonReset.Enabled = false;

				_labelQuestionsToday.Frame = new RectangleF(20, 0, 280, 80);
				_labelQuestionsToday.Text = "There are no questions in this category yet.";	
			}
			
			//_buttonStart.Enabled = true; // For testing
		}
		
		private UIBarButtonItem[] GetToolBar()
		{
			int buttonWidth = 80;

			// Edit category
			_editCategoryButton = new UIBarButtonItem();
			_editCategoryButton.Image = UIImage.FromFile("Assets/Images/Toolbar/toolbar_edit.png");
			_editCategoryButton.Title = "Edit category";
			_editCategoryButton.Width = buttonWidth;
			_editCategoryButton.Clicked += delegate
			{
				AddEditCategoryController controller = new AddEditCategoryController(_category);
				NavigationController.PushViewController(controller, true);
			};

			// Manage questions
			_editQuestionsButton = new UIBarButtonItem();
			_editQuestionsButton.Image = UIImage.FromFile("Assets/Images/Toolbar/toolbar_questions.png");
			_editQuestionsButton.Title = "Manage questions";
			_editCategoryButton.Width = buttonWidth;
			_editQuestionsButton.Clicked += delegate
			{
				QuestionsController controller = new QuestionsController(_category);
				NavigationController.PushViewController(controller, true);
			};

			// Calendar
			_calendarButton = new UIBarButtonItem();
			_calendarButton.Image = UIImage.FromFile("Assets/Images/Toolbar/toolbar_calendar.png");
			_calendarButton.Title = "Calendar";
			_calendarButton.Width = buttonWidth;
			_calendarButton.Clicked += delegate
			{
				CalendarController controller = new CalendarController(_category);
				NavigationController.PushViewController(controller, true);
			};

			return new UIBarButtonItem[] { _editCategoryButton, _editQuestionsButton, _calendarButton };
		}
		
		private string DateSuffix(int day)
		{
			string suffix = "";
			
			int ones = day % 10;
			int tens = (int)Math.Floor(day / 10M) % 10;
			
			if (tens == 1)
			{
				suffix = "th";
			}
			else
			{
				switch (ones)
				{
					case 1:
						suffix = "st";
						break;
			
					case 2:
						suffix = "nd";
						break;
			
					case 3:
						suffix = "rd";
						break;
			
					default:
						suffix = "th";
						break;
				}
			}
			
			return suffix;
		}
	}
}
