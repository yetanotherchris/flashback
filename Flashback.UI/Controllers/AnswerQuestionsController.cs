using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoTouch.UIKit;
using Flashback.Core;
using System.Drawing;
using Flashback.Core.Domain;

namespace Flashback.UI.Controllers
{
	// * ReallySimple style with back/forward
	// * Final View displays how you did: number correct, next ask date

	public class AnswerQuestionsController : UIViewController
	{
		private Category _category;
		private List<Question> _questions;
		private int _questionIndex;
		private Question _currentQuestion;

		private UILabel _labelQuestion;
		private UILabel _labelAnswer;

		private UIButton _buttonShow;
		private UIButton _buttonScore1;
		private UIButton _buttonScore2;
		private UIButton _buttonScore3;
		private UIButton _buttonScore4;
		private UIButton _buttonScore5;
		private UIButton _buttonScore6;

		public AnswerQuestionsController(Category category)
		{
			_questionIndex = 0;
			_category = category;

			_questions = Question.DueToday().ToList();

			// Assume it's the first time of asking if there's none due today (todo:test)
			if (_questions.Count < 1)
				_questions = Question.ForCategory(_category).ToList();
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			
			// Error state
			if (_questions.Count < 1)
			{
				Logger.Warn("AnswerQuestionsController was pushed with zero questions for {0}", _category.Name);
				NavigationController.PopViewControllerAnimated(true);
			}

			// The question label
			_labelQuestion = new UILabel();
			_labelQuestion.Text = "";
			_labelQuestion.Font = UIFont.SystemFontOfSize(16f);
			_labelQuestion.TextColor = UIColor.Gray;
			_labelQuestion.Frame = new RectangleF(15, 30, 290, 50);
			_labelQuestion.BackgroundColor = UIColor.Clear;
			_labelQuestion.Lines = 2;
			View.AddSubview(_labelQuestion);

			// Show button, triggers the answer being visible
			_buttonShow = UIButton.FromType(UIButtonType.RoundedRect);
			_buttonShow.SetTitle("Show answer",UIControlState.Normal);
			_buttonShow.Frame = new RectangleF(15,90,40,25);
			_buttonShow.TouchDown += new EventHandler(ShowAnswer);
			View.AddSubview(_buttonShow);

			// The answer label
			_labelAnswer = new UILabel();
			_labelAnswer.Text = "";
			_labelAnswer.Font = UIFont.SystemFontOfSize(16f);
			_labelAnswer.TextColor = UIColor.Clear;
			_labelAnswer.Frame = new RectangleF(15, 120, 290, 120);
			_labelAnswer.BackgroundColor = UIColor.Clear;
			_labelAnswer.Lines = 5;
			View.AddSubview(_labelQuestion);

			// The 6 score buttons
			_buttonScore1 = UIButton.FromType(UIButtonType.RoundedRect);
			_buttonScore1.SetTitle("1", UIControlState.Normal);
			_buttonScore1.Frame = new RectangleF(15, 250, 30, 30);
			_buttonScore1.TouchDown += new EventHandler(ScoreClick);
			View.AddSubview(_buttonScore1);

			_buttonScore2 = UIButton.FromType(UIButtonType.RoundedRect);
			_buttonScore2.SetTitle("2", UIControlState.Normal);
			_buttonScore2.Frame = new RectangleF(50, 250, 30, 30);
			_buttonScore2.TouchDown += new EventHandler(ScoreClick);
			View.AddSubview(_buttonScore2);

			_buttonScore3 = UIButton.FromType(UIButtonType.RoundedRect);
			_buttonScore3.SetTitle("3", UIControlState.Normal);
			_buttonScore3.Frame = new RectangleF(85, 250, 30, 30);
			_buttonScore3.TouchDown += new EventHandler(ScoreClick);
			View.AddSubview(_buttonScore3);

			_buttonScore4 = UIButton.FromType(UIButtonType.RoundedRect);
			_buttonScore4.SetTitle("4", UIControlState.Normal);
			_buttonScore4.Frame = new RectangleF(120, 250, 30, 30);
			_buttonScore4.TouchDown += new EventHandler(ScoreClick);
			View.AddSubview(_buttonScore4);

			_buttonScore5 = UIButton.FromType(UIButtonType.RoundedRect);
			_buttonScore5.SetTitle("5", UIControlState.Normal);
			_buttonScore5.Frame = new RectangleF(155, 250, 30, 30);
			_buttonScore5.TouchDown += new EventHandler(ScoreClick);
			View.AddSubview(_buttonScore5);

			_buttonScore6 = UIButton.FromType(UIButtonType.RoundedRect);
			_buttonScore6.SetTitle("6", UIControlState.Normal);
			_buttonScore6.Frame = new RectangleF(190, 250, 30, 30);
			_buttonScore6.TouchDown += new EventHandler(ScoreClick);
			View.AddSubview(_buttonScore6);
		}

		private void BindQuestion(Question question)
		{
			_labelQuestion.Text = question.Title;

			_labelAnswer.TextColor = UIColor.Clear;	
			_labelAnswer.Text = question.Answer;

			_currentQuestion = question;
		}

		private void ShowAnswer(object sender, EventArgs e)
		{
			_labelAnswer.TextColor = UIColor.White;
		}

		private void ScoreClick(object sender, EventArgs e)
		{
			UIButton button = (UIButton)sender;
			int score = Convert.ToInt32(button.Title(UIControlState.Normal)) - 1;

			// Update the Question's properties and save it
			QuestionManager.AnswerQuestion(_currentQuestion, score);
			Question.Save(_currentQuestion);

			// Pop the next question, or if there's none left Push the controller
			++_questionIndex;

			if (_questionIndex < _questions.Count)
			{
				_currentQuestion = _questions[_questionIndex];
				BindQuestion(_currentQuestion);
			}
			else
			{
				NavigationController.PopViewControllerAnimated(true);
			}
		}
	}
}
