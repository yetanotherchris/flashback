using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoTouch.UIKit;
using Flashback.Core;
using MonoTouch.Foundation;

namespace Flashback.UI.Controllers
{
	// Table controller with:
	// * Digg style table: 
	//    * Lower text displays number of questions
	//    * Inline text displays number due
	//
	// * Add button top right - pushs EditQuestionController.cs 
	// * Edit button top left - displays a EDIT | DELETE
	//   * - "EDIT" Pushs EditCategoryController.cs 

	public class CategoriesTableController : UITableViewController
	{
		private UIBarButtonItem _addButton;
		private UIBarButtonItem _editButton;

		private CategoriesTableDelegate _tableDelegate;
		private CategoriesTableDataSource _tableDataSource;
		private CategoriesData _data;

		private EditCategoryController _editCategoryController;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			// Add button
			_addButton = new UIBarButtonItem();
			_addButton.Title = "Add";
			_addButton.Clicked += delegate(object sender, EventArgs e)
			{
				_editCategoryController = new EditCategoryController();
				NavigationController.PushViewController(_editCategoryController, true);
			};

			// Edit button
			_editButton = new UIBarButtonItem();
			_editButton.Title = "Edit";
			_editButton.Clicked += delegate(object sender, EventArgs e)
			{
				TableView.Editing = true;
			};

			NavigationItem.SetLeftBarButtonItem(_addButton, false);
			NavigationItem.SetRightBarButtonItem(_editButton, false);

			// Setup the delegate and datasource
			_data = new CategoriesData();
			_tableDelegate = new CategoriesTableDelegate(_data,this);
			_tableDataSource = new CategoriesTableDataSource(_data);

			TableView.DataSource = _tableDataSource;
			TableView.Delegate = _tableDelegate;
		}
	}

	public class CategoriesTableDelegate : UITableViewDelegate
	{
		private CategoriesTableController _navController;
		private QuestionsHubController _questionsController;
		private CategoriesData _data;

		public CategoriesTableDelegate(CategoriesData data,CategoriesTableController navController)
		{
			_data = data;
			_navController = navController;
		}

		public override void RowSelected(UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			_questionsController = new QuestionsHubController(_data.Categories[indexPath.Row]);
			_navController.NavigationController.PushViewController(_questionsController, true);
		}

		public override void WillBeginEditing(UITableView tableView, NSIndexPath indexPath)
		{
			base.WillBeginEditing(tableView, indexPath);

			UILabel label = new UILabel();
			label.Text = "Editing";
			label.Frame = new System.Drawing.RectangleF(0,0,100,20);

			tableView.CellAt(indexPath).ContentView.AddSubview(label);
		}
	}

	public class CategoriesTableDataSource : UITableViewDataSource
	{
		private CategoriesData _data;

		public CategoriesTableDataSource(CategoriesData data)
		{
			_data = data;
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
				cell = new UITableViewCell(UITableViewCellStyle.Default, "cellid");
				cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
			}

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
	}

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
