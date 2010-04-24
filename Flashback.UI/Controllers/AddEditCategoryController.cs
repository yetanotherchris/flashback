using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoTouch.UIKit;
using Flashback.Core;
using MonoTouch.Dialog;
using System.Drawing;

namespace Flashback.UI.Controllers
{
	public class AddEditCategoryController : UIViewController
	{
		private Category _category;

		private UIBarButtonItem _saveButton;
		private UIBarButtonItem _cancelButton;
		private CategoryHubController _hubController;

		private UITextField _textFieldName;
		private UILabel _labelName;

		public AddEditCategoryController(Category category)
		{
			// Null means new category
			_category = category;
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			if (_category == null)
			{
				_category = new Category();
				Title = "Add Category";
			}
			else
			{
				Title = "Edit Category";
			}

			View.BackgroundColor = UIColor.GroupTableViewBackgroundColor;

			// Name label
			_labelName = new UILabel();
			_labelName.Text = "Name";
			_labelName.Frame = new RectangleF(5, 30, 280, 23);
			View.AddSubview(_labelName);

			// Textbox
			_textFieldName = new UITextField();
			_textFieldName.Text = _category.Name;
			_textFieldName.Frame = new RectangleF(5, 60, 280, 23);
			View.AddSubview(_textFieldName);

			// Cancel
			_cancelButton = new UIBarButtonItem();
			_cancelButton.Title = "Cancel";
			_cancelButton.Clicked += delegate(object sender, EventArgs e)
			{
				_hubController = new CategoryHubController(_category);
				NavigationController.PushViewController(_hubController, false);
			};

			// Save button
			_saveButton = new UIBarButtonItem();
			_saveButton.Title = "Save";
			_saveButton.Clicked += delegate(object sender, EventArgs e)
			{
				if (string.IsNullOrEmpty(_textFieldName.Text))
				{
					UIAlertView alertView = new UIAlertView();
					alertView.AddButton("Close");
					alertView.Title = "Woops";
					alertView.Message = "Please enter your category name";
					alertView.Show();

					return;
				}

				_category.Name = _textFieldName.Text;
				_category.Save();

				_hubController = new CategoryHubController(_category);
				NavigationController.PushViewController(_hubController, false);
			};

			// Hide the navigation bar, back button and toolbar.
			NavigationController.SetToolbarHidden(true, false);
			NavigationItem.HidesBackButton = true;
			NavigationItem.SetLeftBarButtonItem(_cancelButton, false);
			NavigationItem.SetRightBarButtonItem(_saveButton, false);
		}
	}
}
