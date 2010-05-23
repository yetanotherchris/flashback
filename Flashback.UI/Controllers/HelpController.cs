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
	/// The HTML help controller, available for all editions.
	/// </summary>
	public class HelpController : UIViewController
	{
		private UIWebView _webView;
		private static string _helpHtml;
		private static string _upgradeHtml;
		private static string _foreignLanguageHtml;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			Title = "Help";
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
		///
		/// </summary>
		private string ReplaceTokens()
		{
			ReadHelpHtml();

			string html = _helpHtml;
			html = ReplaceUpgradeLink(html);
			html = ReplaceForeignLanguage(html);

			return html;
		}

		private string ReplaceUpgradeLink(string html)
		{
			// Replace upgrade link
			if (Settings.IsFullVersion)
			{
				html = html.Replace("#UPGRADE#", "");
			}
			else
			{
				ReadUpgradeHtml();
				html = html.Replace("#UPGRADE#", _upgradeHtml);
			}

			return html;
		}

		private string ReplaceForeignLanguage(string html)
		{
			string foreignLanguage = "";

			// Replace foreign language token
			if (Settings.IsGerman)
			{
				ReadForeignLanguageHtml();
				foreignLanguage = _foreignLanguageHtml.Replace("#LANGUAGE#", "German");
			}
			else if (Settings.IsFrench)
			{
				ReadForeignLanguageHtml();
				foreignLanguage = _foreignLanguageHtml.Replace("#LANGUAGE#","French");
			}
			else if (Settings.IsSpanish)
			{
				ReadForeignLanguageHtml();
				foreignLanguage = _foreignLanguageHtml.Replace("#LANGUAGE#", "Spanish");
			}

			return html.Replace("#LANGUAGE#", foreignLanguage);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		private void ReadHelpHtml()
		{
			if (!string.IsNullOrEmpty(_helpHtml))
				return;

			try
			{
				using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Flashback.Assets.HTML.help.html"))
				{
					using (StreamReader reader = new StreamReader(stream))
					{
						_helpHtml = reader.ReadToEnd();
					}
				}
			}
			catch (IOException e)
			{
				Logger.Warn("An error occured reading the help HTML: \n{0}", e);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		private void ReadUpgradeHtml()
		{
			if (!string.IsNullOrEmpty(_upgradeHtml))
				return;

			try
			{
				using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Flashback.Assets.HTML.upgrade.html"))
				{
					using (StreamReader reader = new StreamReader(stream))
					{
						_upgradeHtml = reader.ReadToEnd();
					}
				}
			}
			catch (IOException e)
			{
				Logger.Warn("An error occured reading the upgrade HTML: \n{0}", e);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		private void ReadForeignLanguageHtml()
		{
			if (!string.IsNullOrEmpty(_foreignLanguageHtml))
				return;

			try
			{
				using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Flashback.Assets.HTML.foreignlanguage.html"))
				{
					using (StreamReader reader = new StreamReader(stream))
					{
						_foreignLanguageHtml = reader.ReadToEnd();
					}
				}
			}
			catch (IOException e)
			{
				Logger.Warn("An error occured reading the foreignlanguage HTML: \n{0}", e);
			}
		}
	}
}
