using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoTouch.UIKit;
using System.Drawing;
using Flashback.Core.iPhone;
using Flashback.Core;

namespace Flashback.UI.Controllers
{
	public class ExportController : UIViewController
	{
		private UILabel _labelHelp;
		private UITextView _textFieldExport;
		private UIBarButtonItem _exportButton;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			Title = "Export";

			// Help label
			_labelHelp = new UILabel();
			_labelHelp.Text = "All questions are exported here using a comma separated format like so:" +
				"\n\n" +
				"category name,question,answer\n\n" +
				"A tilde is used (~) as a replacement for any comma. Use the standard iPhone/iTouch copy " +
				"feature to save the list.";
			_labelHelp.Font = UIFont.SystemFontOfSize(13f);
			_labelHelp.TextColor = UIColor.Gray;
			_labelHelp.Frame = new RectangleF(15, 245, 295, 150);
			_labelHelp.BackgroundColor = UIColor.Clear;
			_labelHelp.Lines = 10;
			View.AddSubview(_labelHelp);

			// Import textbox
			_textFieldExport = new UITextView();
			_textFieldExport.Frame = new RectangleF(10, 35, 300, 200);
			View.AddSubview(_textFieldExport);

			// Hide the toolbar.
			NavigationController.SetToolbarHidden(true, false);
			NavigationItem.HidesBackButton = false;
		}
		
		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);

			IList<Question> questions = Question.List().Where(q => !q.Category.InBuilt).ToList();
			_textFieldExport.Text =  CsvManager.Export(questions);
		}
	}
}
