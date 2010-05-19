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
		private UIBarButtonItem _importButton;
		private UIBarButtonItem _exportButton;
		private UIBarButtonItem _helpButton;
		private UIBarButtonItem _informationButton;
		
		private UIBarButtonItem _editButton;
		private UIBarButtonItem _doneButton;
		private bool _isEditing;

		private CategoriesData _data;
		
		private AddEditCategoryController _addEditCategoryController;
		private ImportController _importController;
		private ExportController _exportController;
		private TipsController _tipsController;
		
		private CategoriesTableSource _categoriesTableSource;

		public CategoriesController() : base(UITableViewStyle.Grouped) { }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			
			Title = "Categories";
			ToolbarItems = GetToolBar();

#if FULLVERSION
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
#endif
		}
		
		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear (animated);

			// Setup the datasource
			ReloadData();
		}
		
		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
			NavigationController.ToolbarHidden = false;
		}
		
		public void ReloadData()
		{
			_data = new CategoriesData();
			_categoriesTableSource = new CategoriesTableSource(_data, this);
			TableView.Source = _categoriesTableSource;
			TableView.ReloadData();
		}

		private UIBarButtonItem[] GetToolBar()
		{
			// Add button
			_addButton = new UIBarButtonItem();
			_addButton.Image = UIImage.FromFile("Assets/Images/Toolbar/toolbar_add.png");
			_addButton.Title = "Add category";
			_addButton.Clicked += delegate
			{
				if (Settings.IsFullVersion)
				{
					_addEditCategoryController = new AddEditCategoryController(null);
					NavigationController.PushViewController(_addEditCategoryController, true);
				}
				else
				{
					UpgradeView view = new UpgradeView();
					view.Show("Only one category is available in the free edition.");
				}
			};

			// Help button
			_helpButton = new UIBarButtonItem();
			_helpButton.Image = UIImage.FromFile("Assets/Images/Toolbar/toolbar_information.png");
			_helpButton.Title = "Help";
			_helpButton.Clicked += delegate
			{
				_tipsController = new TipsController();
				NavigationController.PushViewController(_tipsController, true);
			};

			if (Settings.IsFullVersion)
			{
				// Import button
				_importButton = new UIBarButtonItem();
				_importButton.Image = UIImage.FromFile("Assets/Images/Toolbar/toolbar_import.png");
				_importButton.Title = "Import";
				_importButton.Clicked += delegate
				{
					_importController = new ImportController();
					NavigationController.PushViewController(_importController, true);
				};

				// Export button
				_exportButton = new UIBarButtonItem();
				_exportButton.Image = UIImage.FromFile("Assets/Images/Toolbar/toolbar_export.png");
				_exportButton.Title = "Export";
				_exportButton.Clicked += delegate
				{
					_exportController = new ExportController();
					NavigationController.PushViewController(_exportController, true);
				};

				// Tips button
				_informationButton = new UIBarButtonItem();
				_informationButton.Image = UIImage.FromFile("Assets/Images/Toolbar/toolbar_information.png");
				_informationButton.Title = "Tips";
				_informationButton.Clicked += delegate
				{
					_tipsController = new TipsController();
					NavigationController.PushViewController(_tipsController, true);
				};

				return new UIBarButtonItem[] { _addButton, _importButton, _exportButton, _helpButton, _informationButton };
			}
			else
			{
				return new UIBarButtonItem[] { _addButton, _helpButton };
			}
		}

		#region CategoriesTableSource
		private class CategoriesTableSource : UITableViewSource
		{
			private CategoriesData _data;
			private CategoriesController _parentController;
			private CategoryHubController _hubController;
			private AddEditCategoryController _addEditCategoryController;
			private int _questionsDueCount;

			public CategoriesTableSource(CategoriesData data, CategoriesController parentController)
			{
				_questionsDueCount = 0;
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
				
				// Calculate how many questions there are, and how many are due
				Category category = _data.Categories[indexPath.Row];
				IList<Question> questions = Question.ForCategory(category);
				int questionCount = questions.Count;
				int dueTodayCount = Question.DueToday(questions).ToList().Count;

				if (category.Active)
					cell.DetailTextLabel.Text = string.Format("{0} questions, {1} due today", questionCount, dueTodayCount);
				else
					cell.DetailTextLabel.Text = string.Format("{0} questions. (Inactive)", questionCount);

				cell.TextLabel.Text = category.Name;

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
				_hubController = new CategoryHubController(_data.Categories[indexPath.Row]);
				_parentController.NavigationController.PushViewController(_hubController, true);
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
				Categories = Category.List().ToList().OrderBy(c => c.Name).ToList();
			}
			
			public void DeleteRow(Category category)
			{
				Categories.Remove(category);	
			}
		}
	}
}
