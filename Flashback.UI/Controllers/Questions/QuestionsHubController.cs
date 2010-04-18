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
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			UIWebView webView = new UIWebView();
			webView.Frame = new System.Drawing.RectangleF(0,0,380,420);
			
			string template = "";
			Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Flashback.Assets.HTML.calendar.html");
			using (StreamReader reader = new StreamReader(stream))
			{
				template = reader.ReadToEnd();
			}
			webView.LoadHtmlString(template,new NSUrl("/"));
			View.AddSubview(webView);
		}
	}
}
