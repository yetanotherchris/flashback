using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoTouch.UIKit;
using Flashback.Core;
using System.Drawing;
using Flashback.Core.Domain;
using MonoTouch.Foundation;
using MonoTouch.CoreGraphics;

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

		private UnderlineButton _buttonShow;
		private UIButton _buttonScore1;
		private UIButton _buttonScore2;
		private UIButton _buttonScore3;
		private UIButton _buttonScore4;
		private UIButton _buttonScore5;
		private UIButton _buttonScore6;
		
		private UIImage _bgImage;
		private UIImageView _bgImageView;

		public AnswerQuestionsController(Category category)
		{
			_questionIndex = 0;
			_category = category;

			_questions = Question.DueToday(Question.ForCategory(category)).OrderBy(q => q.Order).ToList();

			// Assume it's the first time of asking if there's none due today (todo:test)
			if (_questions.Count < 1)
				_questions = Question.ForCategory(_category).ToList();
		}

		public override void ViewDidLoad()
		{
			// Hide the toolbar
			base.ViewDidLoad();
			NavigationController.SetToolbarHidden(true,false);
			
			// Error state
			if (_questions.Count < 1)
			{
				Logger.Warn("AnswerQuestionsController was pushed with zero questions for {0}", _category.Name);
				NavigationController.PopViewControllerAnimated(true);
			}
			
			// Flashcard bg image
			_bgImage = UIImage.FromFile("Assets/Images/flashcardbg.jpg");
			_bgImageView = new UIImageView(_bgImage);
			_bgImageView.Frame = new RectangleF(0,0,320,480);
			View.AddSubview(_bgImageView);

			// The question label
			_labelQuestion = new UILabel();
			_labelQuestion.Text = "";
			_labelQuestion.Font = UIFont.SystemFontOfSize(24f);
			_labelQuestion.TextColor = UIColor.Blue;
			_labelQuestion.Frame = new RectangleF(15, 15, 290, 50);
			_labelQuestion.BackgroundColor = UIColor.Clear;
			_labelQuestion.Lines = 2;
			View.AddSubview(_labelQuestion);

			// Show button, triggers the answer being visible
			_buttonShow = new UnderlineButton();
			_buttonShow.Font = UIFont.SystemFontOfSize(16f);
			_buttonShow.SetTitle("Reveal answer...",UIControlState.Normal);
			_buttonShow.SetTitleColor(UIColor.Black,UIControlState.Normal);
			_buttonShow.HorizontalAlignment = UIControlContentHorizontalAlignment.Left;
			_buttonShow.Frame = new RectangleF(15,80,120,25);
			_buttonShow.TouchDown += new EventHandler(ShowAnswer);
			View.AddSubview(_buttonShow);	
			
			// The answer label
			_labelAnswer = new UILabel();
			_labelAnswer.Text = "";
			_labelAnswer.Font = UIFont.SystemFontOfSize(24f);
			_labelAnswer.TextColor = UIColor.Gray;
			_labelAnswer.Frame = new RectangleF(15, 140, 290, 180);
			_labelAnswer.BackgroundColor = UIColor.Clear;
			_labelAnswer.Lines = 7;
			View.AddSubview(_labelAnswer);

			// The 6 score buttons
			int width = 45;
			int height = 40;
			int top = 370;
			int left = 12;
			int padding = 5;
			
			_buttonScore1 = UIButton.FromType(UIButtonType.RoundedRect);
			_buttonScore1.SetTitle("1", UIControlState.Normal);
			_buttonScore1.Frame = new RectangleF(left, top, width, height);
			_buttonScore1.TouchDown += new EventHandler(ScoreClick);
			View.AddSubview(_buttonScore1);
			left += width + padding;

			_buttonScore2 = UIButton.FromType(UIButtonType.RoundedRect);
			_buttonScore2.SetTitle("2", UIControlState.Normal);
			_buttonScore2.Frame = new RectangleF(left, top, width, height);
			_buttonScore2.TouchDown += new EventHandler(ScoreClick);
			View.AddSubview(_buttonScore2);
			left += width + padding;

			_buttonScore3 = UIButton.FromType(UIButtonType.RoundedRect);
			_buttonScore3.SetTitle("3", UIControlState.Normal);
			_buttonScore3.Frame = new RectangleF(left, top, width, height);
			_buttonScore3.TouchDown += new EventHandler(ScoreClick);
			View.AddSubview(_buttonScore3);
			left += width + padding;

			_buttonScore4 = UIButton.FromType(UIButtonType.RoundedRect);
			_buttonScore4.SetTitle("4", UIControlState.Normal);
			_buttonScore4.Frame = new RectangleF(left, top, width, height);
			_buttonScore4.TouchDown += new EventHandler(ScoreClick);
			View.AddSubview(_buttonScore4);
			left += width + padding;

			_buttonScore5 = UIButton.FromType(UIButtonType.RoundedRect);
			_buttonScore5.SetTitle("5", UIControlState.Normal);
			_buttonScore5.Frame = new RectangleF(left, top, width, height);
			_buttonScore5.TouchDown += new EventHandler(ScoreClick);
			View.AddSubview(_buttonScore5);
			left += width + padding;

			_buttonScore6 = UIButton.FromType(UIButtonType.RoundedRect);
			_buttonScore6.SetTitle("6", UIControlState.Normal);
			_buttonScore6.Frame = new RectangleF(left, top, width, height);
			_buttonScore6.TouchDown += new EventHandler(ScoreClick);
			View.AddSubview(_buttonScore6);
			
			// The first question, display it in the labels
			_currentQuestion = _questions[_questionIndex];
			BindQuestion(_currentQuestion);
		}
		
		public class UnderlineButton : UIButton
		{
			public override void Draw (RectangleF rect)
			{
				base.Draw (rect);
				
				// Red underline
				CGContext context = UIGraphics.GetCurrentContext();
				context.SetRGBFillColor(0xFF,0xFF,0,1);
				context.SetLineWidth(1);
				context.MoveTo(0,rect.Height -5);
				context.AddLineToPoint(rect.Width,rect.Height -5);
				context.StrokePath();	
			}
		}
		
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			Title = _category.Name;	
		}

		private void BindQuestion(Question question)
		{
			_labelQuestion.Text = question.Title;
			
			_labelAnswer.Alpha = 0;
			_labelAnswer.Text = question.Answer;
			
			// As UILabel sucks, this verticially aligns it
			NSString nsString = new NSString(_labelAnswer.Text);
			SizeF size = nsString.StringSize(_labelAnswer.Font);
			SizeF requiredSize = new SizeF(size.Width,size.Height * _labelAnswer.Lines);
			_labelAnswer.Frame = new RectangleF(_labelAnswer.Frame.Left, _labelAnswer.Frame.Top, _labelAnswer.Frame.Width, requiredSize.Height);
			
			for (int i=0;i < requiredSize.Height / size.Height ;i++)
			{
				_labelAnswer.Text += "\n";	
			}
		}

		private void ShowAnswer(object sender, EventArgs e)
		{
			_labelAnswer.Alpha = 1;
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
