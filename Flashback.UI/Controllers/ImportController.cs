using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoTouch.UIKit;
using System.Drawing;
using Flashback.Core.iPhone;

namespace Flashback.UI.Controllers
{
	public class ImportController : UIViewController
	{
		private UILabel _labelHelp;
		private UITextView _textFieldImport;
		private UIBarButtonItem _importButton;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			Title = "Import";

			// Help label
			_labelHelp = new UILabel();
			_labelHelp.Text = "To import questions, use a comma separated format like so:" +
				"\n\n" +
				"category name,question,answer\n\n" +
				"The category name is not case sensitive. Use a tilde (~) if you need to use a comma.";
			_labelHelp.Font = UIFont.SystemFontOfSize(14f);
			_labelHelp.TextColor = UIColor.Gray;
			_labelHelp.Frame = new RectangleF(15, 245, 295, 150);
			_labelHelp.BackgroundColor = UIColor.Clear;
			_labelHelp.Lines = 10;
			View.AddSubview(_labelHelp);

			// Import textbox
			_textFieldImport = new UITextView();
			_textFieldImport.Frame = new RectangleF(10, 35, 300, 200);
			View.AddSubview(_textFieldImport);

			// Import button
			_importButton = new UIBarButtonItem();
			_importButton.Title = "Import";
			_importButton.Clicked += delegate(object sender, EventArgs e)
			{
				_textFieldImport.ResignFirstResponder();

				if (!string.IsNullOrEmpty(_textFieldImport.Text))
				{
					CsvManager.Import(_textFieldImport.Text);
					
					// Reload the table controller's data as it doesn't reload it.
					NavigationController.PopViewControllerAnimated(false);
				}
				else 
				{
					// AlertView here	
				}
			};

			// Hide the toolbar.
			NavigationController.SetToolbarHidden(true, false);
			NavigationItem.HidesBackButton = false;
			NavigationItem.SetRightBarButtonItem(_importButton, false);
		}
	}
}
