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
	/// The table controller (and first shown controller) for the list of categories.
	/// </summary>
	public class CategoriesController : UITableViewController
	{
		private UIBarButtonItem _addButton;
		private UIBarButtonItem _importButton;
		private UIBarButtonItem _exportButton;
		private UIBarButtonItem _helpButton;
		private UIBarButtonItem _tipsButton;
		private UIBarButtonItem _spacer;
		
		private UIBarButtonItem _editButton;
		private UIBarButtonItem _doneButton;

		private CategoriesData _data;
		
		private AddEditCategoryController _addEditCategoryController;
		private ImportController _importController;
		private ExportController _exportController;
		private HelpController _helpController;
		private TipsController _tipsController;
		
		private CategoriesTableSource _categoriesTableSource;
		private FirstRunDelegate _firstRunDelegate;

		/// <summary>
		/// The table is UITableViewStyle.Grouped by default.
		/// </summary>
		public CategoriesController() : base(UITableViewStyle.Grouped) { }

		/// <summary>
		/// When the view loads, the toolbar is configured and the edit button shown for the full edition.
		/// </summary>
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			
			Title = "Categories";
			ToolbarItems = GetToolBar();

			if (Settings.IsFullVersion)
			{
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
			}
		}
		
		/// <summary>
		/// Reloads the table's data just before the view appears.
		/// </summary>
		/// <param name="animated"></param>
		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear (animated);

			// Setup the datasource
			ReloadData();
		}
		
		/// <summary>
		/// Displays the first run modal dialog when the view appears, if it's the first run.
		/// </summary>
		/// <param name="animated"></param>
		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
			NavigationController.ToolbarHidden = false;

			// Ask to see the help screen on first run.
			if (Settings.IsFirstRun)
			{
				Settings.IsFirstRun = false;
				
				UIAlertView alertview = new UIAlertView();
				alertview.Title = "Welcome to Flashback";
				alertview.Message = "It's recommended you read the help before starting.\n Would you like to do this now?";
				alertview.AddButton("Yes");
				alertview.AddButton("Later");
				
				_firstRunDelegate = new FirstRunDelegate();
				_firstRunDelegate.ParentController = this;
				alertview.Delegate = _firstRunDelegate;
				
				alertview.Show();
			}
		}
		
		/// <summary>
		/// Configures the UITableView's datasource, and forces a reload of the data from the database.
		/// </summary>
		public void ReloadData()
		{
			_data = new CategoriesData();
			_categoriesTableSource = new CategoriesTableSource(_data, this);
			TableView.Source = _categoriesTableSource;
			TableView.ReloadData();
		}

		/// <summary>
		/// Configures the bottom toolbar.
		/// </summary>
		/// <returns></returns>
		private UIBarButtonItem[] GetToolBar()
		{
			int buttonWidth = 45;
			
			// Add button
			_addButton = new UIBarButtonItem();
			_addButton.Image = UIImage.FromFile("Assets/Images/Toolbar/toolbar_add.png");
			_addButton.Title = "Add category";
			_addButton.Width = buttonWidth;
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
			_helpButton.Image = UIImage.FromFile("Assets/Images/Toolbar/toolbar_help.png");
			_helpButton.Title = "Help";
			_helpButton.Width = buttonWidth;
			_helpButton.Clicked += delegate
			{
				_helpController = new HelpController();
				NavigationController.PushViewController(_helpController, true);
			};

			if (Settings.IsFullVersion)
			{
				_spacer = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);
				//_spacer.Width = buttonWidth;
				
				// Import button
				_importButton = new UIBarButtonItem();
				_importButton.Image = UIImage.FromFile("Assets/Images/Toolbar/toolbar_import.png");
				_importButton.Title = "Import";
				_importButton.Width = buttonWidth;
				_importButton.Clicked += delegate
				{
					_importController = new ImportController();
					NavigationController.PushViewController(_importController, true);
				};

				// Export button
				_exportButton = new UIBarButtonItem();
				_exportButton.Image = UIImage.FromFile("Assets/Images/Toolbar/toolbar_export.png");
				_exportButton.Title = "Export";
				_importButton.Width = buttonWidth;
				_exportButton.Clicked += delegate
				{
					_exportController = new ExportController();
					NavigationController.PushViewController(_exportController, true);
				};

				// Tips button
				_tipsButton = new UIBarButtonItem();
				_tipsButton.Image = UIImage.FromFile("Assets/Images/Toolbar/toolbar_tips.png");
				_tipsButton.Title = "Tips";
				_tipsButton.Width = buttonWidth;
				_tipsButton.Clicked += delegate
				{
					_tipsController = new TipsController();
					NavigationController.PushViewController(_tipsController, true);
				};

				return new UIBarButtonItem[] { _addButton, _spacer,
					_importButton, _spacer,
					_exportButton, _spacer,
					_helpButton, _spacer,
					_tipsButton 
				};
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
				
				string s = (questionCount > 1) ? "s" : "";

				if (category.Active)
					cell.DetailTextLabel.Text = string.Format("{0} question{1}, {2} due today", questionCount,s, dueTodayCount);
				else
					cell.DetailTextLabel.Text = string.Format("{0} question{1}. (Inactive)", questionCount,s);

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

		/// <summary>
		/// The responding delegate for the first run modal dialog
		/// </summary>
		private class FirstRunDelegate : UIAlertViewDelegate
		{
			public CategoriesController ParentController { get;set; }
			
			// These are here to correct an object rooting problem monotouch seems to have with the delegate.
			public FirstRunDelegate() : base() {}
			public FirstRunDelegate(IntPtr handle) : base(handle) {}
			public FirstRunDelegate(NSObjectFlag t) : base(t) {}
			public FirstRunDelegate(NSCoder coder) : base(coder) {}

			public override void Clicked(UIAlertView alertview, int buttonIndex)
			{
				if (buttonIndex == 0)
				{
					HelpController helpController = new HelpController();
					ParentController.NavigationController.PushViewController(helpController,true);
				}
			}
		}
	}
}
