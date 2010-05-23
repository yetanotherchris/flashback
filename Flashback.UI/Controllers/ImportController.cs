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
	/// <summary>
	/// Imports questions as CSV from a textbox.
	/// </summary>
	public class ImportController : UIViewController
	{
		private UILabel _labelHelp;
		private UITextView _textFieldImport;
		private UIBarButtonItem _importButton;
		private BusyView _busyView;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			Title = "Import";
			View.BackgroundColor = UIColor.GroupTableViewBackgroundColor;
			
			// Import textbox
			_textFieldImport = new UITextView();
			_textFieldImport.Frame = new RectangleF(10, 15, 300, 170);
			_textFieldImport.Layer.CornerRadius = 5;
			_textFieldImport.ClipsToBounds = true;
			_textFieldImport.BecomeFirstResponder();
			_textFieldImport.Text = "To import questions, use a comma separated format like so:" +
				"\n\n" +
				"category name,question,answer\n\n" +
				"The category name is not case sensitive. Use a tilde (~) if you need to use a comma.";
			View.AddSubview(_textFieldImport);

			// Help label
			_labelHelp = new UILabel();
			
			_labelHelp.Font = UIFont.SystemFontOfSize(14f);
			_labelHelp.TextColor = UIColor.DarkGray;
			_labelHelp.Frame = new RectangleF(15, 235, 295, 150);
			_labelHelp.BackgroundColor = UIColor.Clear;
			_labelHelp.Lines = 10;
			View.AddSubview(_labelHelp);

			// Import button
			_importButton = new UIBarButtonItem();
			_importButton.Title = "Import";
			_importButton.Clicked += ImportClick;

			// Hide the toolbar.
			NavigationController.SetToolbarHidden(true, true);
			NavigationItem.HidesBackButton = false;
			NavigationItem.SetRightBarButtonItem(_importButton, false);
		}
		
		private void ImportClick(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(_textFieldImport.Text))
			{
				// AlertView here
				UIAlertView alertView = new UIAlertView();
				alertView.Title = "Please enter some questions to import";
				alertView.Show();
				return;
			}
			
			_busyView = null;
			
			// Show a loading view if we there's a lot of text
			if (_textFieldImport.Text.Length > 500)
			{
				_busyView = new BusyView();
				_busyView.Show("Importing...");
			}
			
			Thread thread = new Thread(Import);
			thread.Start();
		}
		
		/// <summary>
		/// This is in a new thread as the BusyView won't show until the method exits (if it's synchronous).
		/// </summary>
		private void Import()
		{
			try
			{
				CsvManager.Import(_textFieldImport.Text);
			}
			catch (Exception ex)
			{
				InvokeOnMainThread(delegate
				{
					if (_busyView != null)
						_busyView.Hide();
				});
				
				// Pokemon exception handling, as we don't really know what CsvManager will throw out.
				Logger.Warn("Unable to import: \n{0}",ex);
				
				InvokeOnMainThread(delegate
				{
					UIAlertView alertView = new UIAlertView();
					alertView.Title = "There was a problem importing.";
					alertView.Message = "Some of the imported text is not in the CSV format.";
					alertView.Show();
				});
			}
			finally
			{
				InvokeOnMainThread(delegate
				{
					if (_busyView != null)
						_busyView.Hide();
					
					NavigationController.PopViewControllerAnimated(false);
				});
			}
		}
	}
}
