using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoTouch.UIKit;
using Flashback.Core;
using System.Drawing;
using System.Linq;

namespace Flashback.UI.Controllers
{
	/// <summary>
	/// Controller to add or update an existing category.
	/// </summary>
	public class AddEditCategoryController : UIViewController
	{
		private Category _category;

		private UIBarButtonItem _saveButton;
		private UIBarButtonItem _cancelButton;
		private UITextField _textFieldName;
		private UILabel _labelName;
		private UILabel _labelActive;
		private UISwitch _switchActive;
		private UILabel _labelInfo;
		
		private CategoryHubController _hubController;

		/// <summary>
		/// Creates a new instance of <see cref="AddEditCategoryController"/>
		/// </summary>
		/// <param name="category">If null, a new category is created.</param>
		public AddEditCategoryController(Category category)
		{
			_category = category;
		}

		/// <summary>
		/// Sets up all controls on the view.
		/// </summary>
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			View.BackgroundColor = UIColor.GroupTableViewBackgroundColor;

			if (_category == null)
			{
				_category = new Category();
				Title = "Add Category";
			}
			else
			{
				Title = "Edit Category";
			}

			// Name label
			_labelName = new UILabel();
			_labelName.Font = UIFont.BoldSystemFontOfSize(16f);
			_labelName.Text = "Category name";
			_labelName.Frame = new RectangleF(10, 10, 300, 25);
			_labelName.BackgroundColor = UIColor.Clear;
			View.AddSubview(_labelName);

			// Name textbox
			_textFieldName = new UITextField();
			_textFieldName.Text = _category.Name;
			_textFieldName.Frame = new RectangleF(10, 35, 300, 30);
			_textFieldName.BorderStyle = UITextBorderStyle.RoundedRect;
			_textFieldName.ShouldReturn = delegate
		    {
		    		_textFieldName.ResignFirstResponder();
		    		return true;
		    };
			View.AddSubview(_textFieldName);
			
			// Active label
			_labelActive = new UILabel();
			_labelActive.Font = UIFont.BoldSystemFontOfSize(16f);
			_labelActive.Text = "Active";
			_labelActive.Frame = new RectangleF(10, 75, 300, 25);
			_labelActive.BackgroundColor = UIColor.Clear;
			View.AddSubview(_labelActive);
			
			// Active switch
			_switchActive = new UISwitch();
			_switchActive.On = _category.Active;
			_switchActive.Frame = new RectangleF(10, 105, 300, 25);
			_switchActive.BackgroundColor = UIColor.Clear;
			View.AddSubview(_switchActive);
			
			// Info label
			_labelInfo = new UILabel();
			_labelInfo.Font = UIFont.SystemFontOfSize(14f);
			_labelInfo.Lines = 3;
			_labelInfo.Text = "Inactive categories aren't counted towards the number of questions due, so don't show "+
							  "on the application's badge on the homescreen.";
			_labelInfo.Frame = new RectangleF(10, 140, 300, 100);
			_labelInfo.TextColor = UIColor.Gray;
			_labelInfo.BackgroundColor = UIColor.Clear;
			View.AddSubview(_labelInfo);

			// Cancel
			_cancelButton = new UIBarButtonItem();
			_cancelButton.Title = "Cancel";
			_cancelButton.Clicked += delegate(object sender, EventArgs e)
			{
				NavigationController.PopViewControllerAnimated(true);
			};

			// Save button
			_saveButton = new UIBarButtonItem();
			_saveButton.Title = "Save";
			_saveButton.Clicked += SaveClick;

			// Hide the navigation bar, back button and toolbar.
			NavigationController.SetToolbarHidden(true, true);
			NavigationItem.HidesBackButton = true;
			NavigationItem.SetLeftBarButtonItem(_cancelButton, false);
			NavigationItem.SetRightBarButtonItem(_saveButton, false);
		}
		
		private void SaveClick(object sender, EventArgs e)
		{
			_textFieldName.ResignFirstResponder();
				
			// Check for empty textboxes
			if (string.IsNullOrEmpty(_textFieldName.Text))
			{
				UIAlertView alertView = new UIAlertView();
				alertView.AddButton("Close");
				alertView.Title = "Woops";
				alertView.Message = "Please enter your category name";
				alertView.Show();

				return;
			}

			// Save the category
			_category.Name = _textFieldName.Text;
			_category.Active = _switchActive.On;
			Category.Save(_category);
			
			NavigationController.PopViewControllerAnimated(true);
		}
	}
}
