using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoTouch.UIKit;
using Flashback.Core;

namespace Flashback.UI.Controllers
{
	// UIWebView with:
	// * Title bar with "EDIT" button
	// * Smaller box with number of questions
	// * Big box for questions due today
	// * Calendar for future dates
	// * Big button for "ASK NOW"
	// * Smaller button for "Question Tips"

	public class QuestionsHubController : UIViewController
	{
		private Category _category;

		public QuestionsHubController(Category category)
		{
			_category = category;
		}
	}
}
