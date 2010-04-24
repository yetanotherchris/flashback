using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flashback.Core;
using MonoTouch.UIKit;
using System.Drawing;

namespace Flashback.UI.Controllers
{
	public class AddEditQuestionController : UIViewController
	{
		private Question _question;
		private Category _category;

		private UIBarButtonItem _saveButton;
		private UIBarButtonItem _cancelButton;
		private QuestionsController _questionsController;

		private UILabel _labelQuestion;
		private UILabel _labelAnswer;
		private UITextField _textFieldQuestion;
		private UITextField _textFieldAnswer;
		

		public AddEditQuestionController(Question question,Category category)
		{
			// Null means new category
			_question = question;
			_category = category;
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			
			// Remove this view from the stack

			if (_question == null)
			{
				_question = new Question();
				_question.Category = _category;
				Title = "Add Question";
			}
			else
			{
				Title = "Edit Question";
			}

			View.BackgroundColor = UIColor.GroupTableViewBackgroundColor;

			// Question label
			_labelQuestion = new UILabel();
			_labelQuestion.Text = "Question";
			_labelQuestion.Frame = new RectangleF(5, 30, 280, 23);
			_labelQuestion.BackgroundColor = UIColor.Clear;
			View.AddSubview(_labelQuestion);

			// Question textbox
			_textFieldQuestion = new UITextField();
			_textFieldQuestion.Text = _question.Title;
			_textFieldQuestion.Frame = new RectangleF(5, 60, 280, 23);
			_textFieldQuestion.BorderStyle = UITextBorderStyle.RoundedRect;
			_textFieldQuestion.ShouldReturn = delegate
		    {
		    		_textFieldQuestion.ResignFirstResponder();
		    		return true;
		    };
			View.AddSubview(_textFieldQuestion);

			// Answer label
			_labelAnswer = new UILabel();
			_labelAnswer.Text = "Answer";
			_labelAnswer.Frame = new RectangleF(5, 90, 280, 23);
			_labelAnswer.BackgroundColor = UIColor.Clear;
			View.AddSubview(_labelAnswer);

			// Answer textbox
			_textFieldAnswer = new UITextField();
			_textFieldAnswer.Text = _question.Answer;
			_textFieldAnswer.Frame = new RectangleF(5, 120, 280, 50);
			_textFieldAnswer.BorderStyle = UITextBorderStyle.RoundedRect;
			_textFieldAnswer.ShouldReturn = delegate
		    {
		    		_textFieldAnswer.ResignFirstResponder();
		    		return true;
		    };
			View.AddSubview(_textFieldAnswer);

			// Cancel
			_cancelButton = new UIBarButtonItem();
			_cancelButton.Title = "Cancel";
			_cancelButton.Clicked += delegate(object sender, EventArgs e)
			{
				NavigationController.PopViewControllerAnimated(true);
			};

			// Save button
			_saveButton = new UIBarButtonItem();
			_saveButton.Title = "Save";
			_saveButton.Clicked += delegate(object sender, EventArgs e)
			{
				if (string.IsNullOrEmpty(_textFieldQuestion.Text) || string.IsNullOrEmpty(_textFieldAnswer.Text))
				{
					UIAlertView alertView = new UIAlertView();
					alertView.AddButton("Close");
					alertView.Title = "Woops";
					alertView.Message = "Please enter a question and its answer";
					alertView.Show();

					return;
				}

				_question.Title = _textFieldQuestion.Text;
				_question.Answer = _textFieldAnswer.Text;
				_question.Save();

				// Make sure the data is refreshed on the question table controller
				((QuestionsController)NavigationController.ViewControllers[NavigationController.ViewControllers.Length-2]).ReloadData();
				NavigationController.PopViewControllerAnimated(false);
			};

			// Hide the navigation bar, back button and toolbar.
			NavigationController.SetToolbarHidden(true, false);
			NavigationItem.HidesBackButton = true;
			NavigationItem.SetLeftBarButtonItem(_cancelButton, false);
			NavigationItem.SetRightBarButtonItem(_saveButton, false);
		}
	}
}
