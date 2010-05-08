using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoTouch.UIKit;
using System.Drawing;
using Flashback.Core.iPhone;

namespace Flashback.UI.Controllers
{
	public class ExportController : UIViewController
	{
		private UILabel _labelHelp;
		private UITextField _textFieldExport;
		private UIBarButtonItem _exportButton;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			Title = "Export";

			// Help label
			_labelHelp = new UILabel();
			_labelHelp.Text = "This will export all your questions, using a comma separated format like so:" +
				"\n\n" +
				"category name,question,answer\n\n" +
				"A tilde is used (~) as a replacement for any comma";
			_labelHelp.Font = UIFont.SystemFontOfSize(16f);
			_labelHelp.TextColor = UIColor.Gray;
			_labelHelp.Frame = new RectangleF(15, 245, 295, 150);
			_labelHelp.BackgroundColor = UIColor.Clear;
			_labelHelp.Lines = 10;
			View.AddSubview(_labelHelp);

			// Import textbox
			_textFieldExport = new UITextField();
			_textFieldExport.Frame = new RectangleF(10, 35, 300, 200);
			_textFieldExport.BorderStyle = UITextBorderStyle.RoundedRect;
			_textFieldExport.UserInteractionEnabled = false;
			View.AddSubview(_textFieldExport);

			// Export button
			_exportButton = new UIBarButtonItem();
			_exportButton.Title = "Export";
			_exportButton.Clicked += delegate(object sender, EventArgs e)
			{
				_textFieldExport.ResignFirstResponder();
				CsvManager.Import(_textFieldExport.Text);
			};

			// Hide the toolbar.
			NavigationController.SetToolbarHidden(true, false);
			NavigationItem.SetLeftBarButtonItem(_exportButton, false);
		}
	}
}
