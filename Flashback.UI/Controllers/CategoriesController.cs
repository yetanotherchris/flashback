using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoTouch.UIKit;
using Flashback.Core;
using MonoTouch.Foundation;
using System.Drawing;

namespace Flashback.UI.Controllers
{
	/// <summary>
	/// 
	/// </summary>
	public class CategoriesController : UITableViewController
	{
		private UIButton _addButton;
		private UIBarButtonItem _editButton;
		private UIBarButtonItem _doneButton;
		private bool _isEditing;

		private CategoriesData _data;
		private AddEditCategoryController _addEditCategoryController;
		private CategoriesTableSource _categoriesTableSource;

		public CategoriesController() : base(UITableViewStyle.Grouped) { }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			//	* Digg style table: 
			//    * Lower text displays number of questions
			//    * Inline text displays number due

			// + Add button
			_addButton = UIButton.FromType(UIButtonType.ContactAdd);
			_addButton.SetTitle("Add",UIControlState.Normal);
			_addButton.Frame = new System.Drawing.RectangleF(285,80,23,23);
			_addButton.TouchDown += delegate(object sender, EventArgs e)
			{
				_addEditCategoryController = new AddEditCategoryController(null);
				NavigationController.PushViewController(_addEditCategoryController, true);
			};
			View.AddSubview(_addButton);

			// Edit and done button
			_editButton = new UIBarButtonItem(UIBarButtonSystemItem.Edit);
			_editButton.Clicked += delegate(object sender, EventArgs e)
			{
				TableView.Editing = true;
				NavigationItem.SetRightBarButtonItem(_doneButton, false);
			};

			_doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done);
			_doneButton.Clicked += delegate(object sender, EventArgs e)
			{
				TableView.Editing = false;
				NavigationItem.SetRightBarButtonItem(_editButton, false);
			};

			NavigationItem.SetRightBarButtonItem(_editButton, false);

			// Setup the delegate and datasource
			_data = new CategoriesData();
			_categoriesTableSource = new CategoriesTableSource(_data, null);
			TableView.Source = _categoriesTableSource;
		}

		#region CategoriesTableSource
		private class CategoriesTableSource : UITableViewSource
		{
			private CategoriesData _data;
			private CategoriesController _navController;
			private CategoryHubController _questionsController;

			public CategoriesTableSource(CategoriesData data, CategoriesController navController)
			{
				_data = data;
				_navController = navController;
			}

			public override int RowsInSection(UITableView tableview, int section)
			{
				return _data.Categories.Count;
			}

			public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
			{
				UITableViewCell cell = tableView.DequeueReusableCell("cellid");

				if (cell == null)
				{
					cell = new UITableViewCell(UITableViewCellStyle.Subtitle, "cellid");
					cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
				}

				cell.DetailTextLabel.Text = "1 question due";
				cell.TextLabel.Text = _data.Categories[indexPath.Row].Name;

				return cell;
			}

			public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
			{
				if (editingStyle == UITableViewCellEditingStyle.Delete)
				{
					Category category = _data.Categories[indexPath.Row];
					Category.Delete(category.Id);

					tableView.DeleteRows(new[] { indexPath }, UITableViewRowAnimation.Fade);
				}
			}

			public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
			{
				_questionsController = new CategoryHubController(_data.Categories[indexPath.Row]);
				_navController.NavigationController.PushViewController(_questionsController, true);
			}
		}
		#endregion

		/// <summary>
		/// Keeps a single copy of the categories list, sorted into a list.
		/// </summary>
		public class CategoriesData
		{
			public List<Category> Categories { get; private set; }

			public CategoriesData()
			{
				Categories = Category.List().OrderBy(c => c.Name).ToList();
			}
		}
	}
}
