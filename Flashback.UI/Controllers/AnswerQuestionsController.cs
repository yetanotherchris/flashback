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
	/// <summary>
	/// Displays all questions for a category, with a score and reveal question link.
	/// </summary>
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

		/// <summary>
		/// Creates a new instance of <see cref="AnswerQuestionsController"/>
		/// </summary>
		/// <param name="category"></param>
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
			NavigationController.SetToolbarHidden(true,true);
			
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
			_labelQuestion.TextColor = UIColor.Black;
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
			AddScoreButtons();
			
			// The first question, display it in the labels
			_currentQuestion = _questions[_questionIndex];
			BindQuestion(_currentQuestion);
		}

		/// <summary>
		/// Fades the buttons in when the view has appeared.
		/// </summary>
		/// <param name="animated"></param>
		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
			FadeIn();
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			Title = _category.Name;
		}

		private void AddScoreButtons()
		{
			int width = 45;
			int height = 40;
			int top = 370;
			int left = 12;
			int padding = 5;

			UIImage image1 = UIImage.FromFile("Assets/Images/button_1.png");
			_buttonScore1 = new UIButton();
			_buttonScore1.SetBackgroundImage(image1, UIControlState.Normal);
			_buttonScore1.Frame = new RectangleF(left, top, width, height);
			_buttonScore1.TouchDown += new EventHandler(ScoreClick);
			View.AddSubview(_buttonScore1);
			left += width + padding;

			UIImage image2 = UIImage.FromFile("Assets/Images/button_2.png");
			_buttonScore2 = new UIButton();
			_buttonScore2.SetBackgroundImage(image2, UIControlState.Normal);
			_buttonScore2.Frame = new RectangleF(left, top, width, height);
			_buttonScore2.TouchDown += new EventHandler(ScoreClick);
			View.AddSubview(_buttonScore2);
			left += width + padding;

			UIImage image3 = UIImage.FromFile("Assets/Images/button_3.png");
			_buttonScore3 = new UIButton();
			_buttonScore3.SetBackgroundImage(image3, UIControlState.Normal);
			_buttonScore3.Frame = new RectangleF(left, top, width, height);
			_buttonScore3.TouchDown += new EventHandler(ScoreClick);
			View.AddSubview(_buttonScore3);
			left += width + padding;

			UIImage image4 = UIImage.FromFile("Assets/Images/button_4.png");
			_buttonScore4 = new UIButton();
			_buttonScore4.SetBackgroundImage(image4, UIControlState.Normal);
			_buttonScore4.Frame = new RectangleF(left, top, width, height);
			_buttonScore4.TouchDown += new EventHandler(ScoreClick);
			View.AddSubview(_buttonScore4);
			left += width + padding;

			UIImage image5 = UIImage.FromFile("Assets/Images/button_5.png");
			_buttonScore5 = new UIButton();
			_buttonScore5.SetBackgroundImage(image5, UIControlState.Normal);
			_buttonScore5.Frame = new RectangleF(left, top, width, height);
			_buttonScore5.TouchDown += new EventHandler(ScoreClick);
			View.AddSubview(_buttonScore5);
			left += width + padding;

			UIImage image6 = UIImage.FromFile("Assets/Images/button_6.png");
			_buttonScore6 = new UIButton();
			_buttonScore6.SetBackgroundImage(image6, UIControlState.Normal);
			_buttonScore6.Frame = new RectangleF(left, top, width, height);
			_buttonScore6.TouchDown += new EventHandler(ScoreClick);
			View.AddSubview(_buttonScore6);

			// Set the buttons to invisible to start with
			SetButtonAlphas();
		}
		
		private void SetButtonAlphas()
		{
			_buttonScore1.Alpha = 0;
			_buttonScore2.Alpha = 0;
			_buttonScore3.Alpha = 0;
			_buttonScore4.Alpha = 0;
			_buttonScore5.Alpha = 0;
			_buttonScore6.Alpha = 0;	
		}
		
		private void FadeIn()
		{
			UIView.SetAnimationDelay(0.5);
			UIView.BeginAnimations(null);
			_buttonScore1.Alpha = 1;
			_buttonScore2.Alpha = 1;
			_buttonScore3.Alpha = 1;
			_buttonScore4.Alpha = 1;
			_buttonScore5.Alpha = 1;
			_buttonScore6.Alpha = 1;
			UIView.CommitAnimations();
		}
		
		private void DisableScoreButtons()
		{
			_buttonScore1.Enabled = false;
			_buttonScore2.Enabled = false;
			_buttonScore3.Enabled = false;
			_buttonScore4.Enabled = false;
			_buttonScore5.Enabled = false;
			_buttonScore6.Enabled = false;
		}
		
		private void EnableScoreButtons()
		{
			_buttonScore1.Enabled = true;
			_buttonScore2.Enabled = true;
			_buttonScore3.Enabled = true;
			_buttonScore4.Enabled = true;
			_buttonScore5.Enabled = true;
			_buttonScore6.Enabled = true;
		}

		/// <summary>
		/// Sets up the question/answer labels for the provided question.
		/// </summary>
		/// <param name="question"></param>
		private void BindQuestion(Question question)
		{
			DisableScoreButtons();
			
			// Question and answer text
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
			EnableScoreButtons();
		}

		private void ScoreClick(object sender, EventArgs e)
		{
			UIButton button = (UIButton)sender;
			int score = Convert.ToInt32(button.Title(UIControlState.Normal)) - 1;

			// Update the Question's properties and save it
			QuestionManager.AnswerQuestion(_currentQuestion, score);
			Question.Save(_currentQuestion);

			// Pop the next question, or if there's none left pop the controller
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
