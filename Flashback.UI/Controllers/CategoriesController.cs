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
		private UIBarButtonItem _addButton;
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
			
			Title = "Categories";

			//	* Digg style table: 
			//    * Lower text displays number of questions
			//    * Inline text displays number due

			// + Add button
			_addButton = new UIBarButtonItem(UIBarButtonSystemItem.Add);
			_addButton.Clicked += delegate(object sender, EventArgs e)
			{
				_addEditCategoryController = new AddEditCategoryController(null);
				NavigationController.PushViewController(_addEditCategoryController, true);
			};

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

			NavigationItem.SetLeftBarButtonItem(_addButton, false);
			NavigationItem.SetRightBarButtonItem(_editButton, false);

			// Setup the delegate and datasource
			ReloadData();
			TableView.AllowsSelectionDuringEditing = true;
		}
		
		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear(animated);
			NavigationController.ToolbarHidden = true;
		}
		
		public void ReloadData()
		{
			_data = new CategoriesData();
			_categoriesTableSource = new CategoriesTableSource(_data, this);
			TableView.Source = _categoriesTableSource;
			TableView.ReloadData();
		}

		#region CategoriesTableSource
		private class CategoriesTableSource : UITableViewSource
		{
			private CategoriesData _data;
			private CategoriesController _parentController;
			private CategoryHubController _hubController;
			private AddEditCategoryController _addEditCategoryController;

			public CategoriesTableSource(CategoriesData data, CategoriesController parentController)
			{
				_data = data;
				_parentController = parentController;
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

				cell.DetailTextLabel.Text = "14 questions, 1 due today";
				cell.TextLabel.Text = _data.Categories[indexPath.Row].Name;

				return cell;
			}

			public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
			{
				if (editingStyle == UITableViewCellEditingStyle.Delete)
				{
					Category category = _data.Categories[indexPath.Row];
					Category.Delete(category.Id);
					_data.DeleteRow(category);
					
					tableView.DeleteRows(new[] { indexPath }, UITableViewRowAnimation.Fade);
				}
			}

			public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
			{
				if (tableView.Editing)
				{
					tableView.Editing = false;
					_addEditCategoryController = new AddEditCategoryController(_data.Categories[indexPath.Row]);
					_parentController.NavigationController.PushViewController(_addEditCategoryController, true);
				}
				else
				{
					_hubController = new CategoryHubController(_data.Categories[indexPath.Row]);
					_parentController.NavigationController.PushViewController(_hubController, true);
				}
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
			
			public void DeleteRow(Category category)
			{
				Categories.Remove(category);	
			}
		}
	}
}
