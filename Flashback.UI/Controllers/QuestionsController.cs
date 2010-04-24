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
		private UIButton _addButton;
		private UIBarButtonItem _editButton;
		private UIBarButtonItem _doneButton;

		private QuestionsData _data;
		private AddEditQuestionController _addEditQuestionController;
		private QuestionsTableSource _categoriesTableSource;

		public QuestionsController(Category category) : base(UITableViewStyle.Grouped)
		{
			_data = new QuestionsData(category);
		}

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
				_addEditQuestionController = new AddEditQuestionController(null);
				NavigationController.PushViewController(_addEditQuestionController, true);
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
			_categoriesTableSource = new QuestionsTableSource(_data, null);
			TableView.Source = _categoriesTableSource;
		}

		#region CategoriesTableSource
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
					cell = new UITableViewCell(UITableViewCellStyle.Default, "cellid");
					cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
				}

				cell.TextLabel.Text = _data.Questions[indexPath.Row].Title;

				return cell;
			}

			public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
			{
				if (editingStyle == UITableViewCellEditingStyle.Delete)
				{
					Question question = _data.Questions[indexPath.Row];
					Question.Delete(question.Id);

					tableView.DeleteRows(new[] { indexPath }, UITableViewRowAnimation.Fade);
				}
			}

			public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
			{
				_addEditQuestionsController = new AddEditQuestionController(_data.Questions[indexPath.Row]);
				_questionsController.NavigationController.PushViewController(_addEditQuestionsController, true);
			}
		}
		#endregion

		/// <summary>
		/// Keeps a single copy of the questions list, sorted by order.
		/// </summary>
		public class QuestionsData
		{
			public List<Question> Questions { get; private set; }

			public QuestionsData(Category category)
			{
				Questions = Question.List(true,"@categoryid",category.Id).OrderBy(q => q.Order).ToList();
			}
		}
	}
}
