using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoTouch.UIKit;
using System.Drawing;
using Flashback.Core.iPhone;
using Flashback.Core;
using System.Threading;

namespace Flashback.UI.Controllers
{
	public class ExportController : UIViewController
	{
		private UILabel _labelHelp;
		private UITextView _textFieldExport;
		private UIBarButtonItem _exportButton;
		private BusyView _busyView;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			Title = "Export";
			View.BackgroundColor = UIColor.GroupTableViewBackgroundColor;
			
			// Export textbox
			_textFieldExport = new UITextView();
			_textFieldExport.Frame = new RectangleF(10, 15, 300, 200);
			View.AddSubview(_textFieldExport);

			// Help label
			_labelHelp = new UILabel();
			_labelHelp.Text = "The questions are exported in comma separated format (CSV), e.g.:" +
				"\n\n" +
				"category name,question,answer\n\n" +
				"A tilde is used (~) as a replacement for any comma. Use the standard iPhone/iTouch clipboard copy " +
				"feature to save the list.";
			_labelHelp.Font = UIFont.SystemFontOfSize(14f);
			_labelHelp.TextColor = UIColor.DarkGray;
			_labelHelp.Frame = new RectangleF(15, 235, 295, 150);
			_labelHelp.BackgroundColor = UIColor.Clear;
			_labelHelp.Lines = 10;
			View.AddSubview(_labelHelp);

			// Hide the toolbar.
			NavigationController.SetToolbarHidden(true, false);
			NavigationItem.HidesBackButton = false;
		}
		
		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);

			_busyView = new BusyView();
			_busyView.Show("Exporting...");
			
			Thread thread = new Thread(ThreadEntry);
			thread.Start();
		}
		
		private void ThreadEntry()
		{
			IList<Question> questions = Question.List().Where(q => !q.Category.InBuilt).ToList();
			string csv = CsvManager.Export(questions);
			
			InvokeOnMainThread(delegate()
			{
				_textFieldExport.Text = csv;
				_busyView.Hide();
			});
		}
	}
}
