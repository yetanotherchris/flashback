using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoTouch.UIKit;
using System.Drawing;
using MonoTouch.Foundation;
using System.IO;
using System.Reflection;
using Flashback.Core;

namespace Flashback.UI.Controllers
{
	/// <summary>
	/// The HTML tips controller for the full edition.
	/// </summary>
	public class TipsController : UIViewController
	{
		private UIWebView _webView;
		private static string _tipsHtml;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			Title = "Tips";
			View.BackgroundColor = UIColor.GroupTableViewBackgroundColor;

			ReadHelpHtml();

			_webView = new UIWebView();
			_webView.Frame = new RectangleF(0, 0, 320, 480);
			_webView.LoadHtmlString(_tipsHtml, new NSUrl("/"));
			_webView.BackgroundColor = UIColor.Clear;
			_webView.Opaque = false;
			View.AddSubview(_webView);
		}

		/// <summary>
		/// Retrieves the Calendar HTML from the embedded resource into a static field.
		/// </summary>
		/// <returns></returns>
		private void ReadHelpHtml()
		{
			if (!string.IsNullOrEmpty(_tipsHtml))
				return;

			try
			{
				using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Flashback.Assets.HTML.tips.html"))
				{
					using (StreamReader reader = new StreamReader(stream))
					{
						_tipsHtml = reader.ReadToEnd();
					}
				}
			}
			catch (IOException e)
			{
				Logger.Warn("An error occurred reading the tips HTML: \n{0}", e);
			}
		}
	}
}
