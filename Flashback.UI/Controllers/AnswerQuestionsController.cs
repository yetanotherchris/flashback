using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoTouch.UIKit;
using Flashback.Core;
using System.Drawing;

namespace Flashback.UI.Controllers
{
	// * ReallySimple style with back/forward
	// * Final View displays how you did: number correct, next ask date

	public class AnswerQuestionsController : UIViewController
	{
		private Category _category;

		private UILabel _labelQuestion;

		public AnswerQuestionsController(Category category)
		{
			_category = category;
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			List<Question> questions = Question.ForCategory(_category).ToList();

			_labelQuestion = new UILabel();
			_labelQuestion.Text = questions[0].Title;
			_labelQuestion.Font = UIFont.SystemFontOfSize(16f);
			_labelQuestion.TextColor = UIColor.Gray;
			_labelQuestion.Frame = new RectangleF(15, 245, 295, 150);
			_labelQuestion.BackgroundColor = UIColor.Clear;
			_labelQuestion.Lines = 10;
			View.AddSubview(_labelQuestion);
		}
	}
}
