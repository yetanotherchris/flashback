using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoTouch.UIKit;
using Flashback.Core;
using MonoTouch.Foundation;

namespace Flashback.UI.Controllers
{
	/// <summary>
	/// Digg style table
	/// </summary>
	public class QuestionsController : UITableViewController
	{
		private UIBarButtonItem _addButton;
		private UIBarButtonItem _editButton;
		private UIBarButtonItem _doneButton;

		private QuestionsData _data;
		private Category _category;
		private AddEditQuestionController _addEditQuestionController;
		private QuestionsTableSource _questionsTableSource;

		public QuestionsController(Category category) : base(UITableViewStyle.Grouped)
		{
			_data = new QuestionsData(category);
			_category = category;
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			
			Title = _category.Name;
			ToolbarItems = GetToolBar();
			NavigationController.ToolbarHidden = false;

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

			// Setup the datasource
			ReloadData();
		}
		
		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear(animated);
			NavigationController.ToolbarHidden = false;
		}
		
		public void ReloadData()
		{
			_data = new QuestionsData(_category);
			_questionsTableSource = new QuestionsTableSource(_data, this);
			TableView.Source = _questionsTableSource;
			TableView.ReloadData();
		}

		private UIBarButtonItem[] GetToolBar()
		{
			// Add button
			_addButton = new UIBarButtonItem();
			_addButton.Image = UIImage.FromFile("Assets/Images/Toolbar/toolbar_add.png");
			_addButton.Title = "Add question";
			_addButton.Clicked += delegate
			{
				_addEditQuestionController = new AddEditQuestionController(null, _data.Category);
				NavigationController.PushViewController(_addEditQuestionController, true);
			};

			return new UIBarButtonItem[] { _addButton };
		}

		#region QuestionsTableViewSource
		private class QuestionsTableSource : UITableViewSource
		{
			private QuestionsData _data;
			private QuestionsController _questionsController;
			private AddEditQuestionController _addEditQuestionsController;

			public QuestionsTableSource(QuestionsData data, QuestionsController questionsController)
			{
				_data = data;
				_questionsController = questionsController;
			}

			public override int RowsInSection(UITableView tableview, int section)
			{
				return _data.Questions.Count;
			}

			public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
			{
				UITableViewCell cell = tableView.DequeueReusableCell("cellid");

				if (cell == null)
				{
					cell = new UITableViewCell(UITableViewCellStyle.Subtitle, "cellid");
					cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
				}

				cell.TextLabel.Text = _data.Questions[indexPath.Row].Title;
				cell.DetailTextLabel.Text = _data.Questions[indexPath.Row].Answer;

				return cell;
			}

			public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
			{
				if (editingStyle == UITableViewCellEditingStyle.Delete)
				{
					Question question = _data.Questions[indexPath.Row];
					Question.Delete(question.Id);
					_data.DeleteRow(question);

					tableView.DeleteRows(new[] { indexPath }, UITableViewRowAnimation.Fade);
				}
			}

			public override void MoveRow(UITableView tableView, NSIndexPath sourceIndexPath, NSIndexPath destinationIndexPath)
			{
				// Re-order using question.Order
				Question.Move(_data.Questions[sourceIndexPath.Row], destinationIndexPath.Row);
			}

			public override bool CanMoveRow(UITableView tableView, NSIndexPath indexPath)
			{
				return true;
			}

			public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
			{
				Question question = _data.Questions[indexPath.Row];
				_addEditQuestionsController = new AddEditQuestionController(question,question.Category);
				_questionsController.NavigationController.PushViewController(_addEditQuestionsController, false);
			}
		}
		#endregion

		/// <summary>
		/// Keeps a single copy of the questions list, sorted by order.
		/// </summary>
		public class QuestionsData
		{
			public List<Question> Questions { get; private set; }
			public Category Category { get;private set;}

			public QuestionsData(Category category)
			{
				Category = category;
				Questions = Question.ForCategory(category).OrderBy(q => q.Order).ToList();
			}
			
			public void DeleteRow(Question question)
			{
				Questions.Remove(question);	
			}
		}
	}
}
