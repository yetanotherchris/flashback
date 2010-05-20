using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoTouch.UIKit;
using System.Drawing;
using Flashback.Core;
using System.IO;
using System.Reflection;
using MonoTouch.Foundation;

namespace Flashback.UI.Controllers
{
	public class CalendarController : UIViewController
	{
		private UIWebView _webView;
		private static string _calendarHtml;
		private Category _category;

		public CalendarController(Category category)
		{
			_category = category;
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			Title = _category.Name;
			View.BackgroundColor = UIColor.GroupTableViewBackgroundColor;

			string html = ReplaceTokens();
			_webView = new UIWebView();
			_webView.Frame = new RectangleF(0, 0, 320, 480);
			_webView.LoadHtmlString(html, new NSUrl("/"));
			_webView.BackgroundColor = UIColor.Clear;
			_webView.Opaque = false;
			View.AddSubview(_webView);
		}

		/// <summary>
		/// Replaces the #DATE# token in the calendar template with dates due for questions.
		/// </summary>
		private string ReplaceTokens()
		{
			ReadCalendarHtml();
			
			string template = _calendarHtml;
			string dateFormat = "_eventDates['{0}'] = \"{1}\";";

			StringBuilder builder = new StringBuilder();
			Dictionary<string,int> dates = new Dictionary<string,int>();

			// Turn today green if there are some due
			// Work out how many are due for each day. This could be done with LINQ but this way is reusable in the next bit.
			foreach (Question question in Question.ForCategory(_category))
			{
				string date = question.NextAskOn.ToString("d:M:yyyy");
				
				// Set past dates as today
				if (question.NextAskOn < DateTime.Today)
					date = DateTime.Today.ToString("d:M:yyyy");

				if (dates.ContainsKey(date))
					dates[date] += 1;
				else
					dates.Add(date,1);
			}

			// The Javascript
			foreach (string dueDate in dates.Keys)
			{
				builder.AppendLine(string.Format(dateFormat, dueDate, dates[dueDate]));
			}

			string s = builder.ToString();
			return template.Replace("#DATES#",builder.ToString());
		}

		/// <summary>
		/// Retrieves the Calendar HTML from the embedded resource into a static field.
		/// </summary>
		/// <returns></returns>
		private void ReadCalendarHtml()
		{
			if (!string.IsNullOrEmpty(_calendarHtml))
				return;

			try
			{
				using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Flashback.Assets.HTML.calendar.html"))
				{
					using (StreamReader reader = new StreamReader(stream))
					{
						_calendarHtml = reader.ReadToEnd();
					}
				}
			}
			catch (IOException e)
			{
				Logger.Warn("An error occured reading the calendar HTML: \n{0}", e);
			}
		}
	}
}
