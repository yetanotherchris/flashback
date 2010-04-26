using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;

namespace Flashback.Core
{
	public class Question
	{
		private Category _category;
		
		#region Properties	
		[PrimaryKey,AutoIncrement]
		public int Id { get; set; }
		
		/// <summary>
		/// The category for the question
		/// </summary>
		[Ignore]
		internal Category Category
		{
			get
			{
				if (_category == null)
					_category = Category.Read(CategoryId);
				
				return _category;
			}
			set
			{
				CategoryId = value.Id;	
			}
		}
		
		public Category GetCategory()
		{
			return Category.Read(CategoryId);
		}
		
		public void SetCategory(Category category)
		{
			CategoryId = category.Id;	
		}
		
		public int CategoryId { get; set;}

		/// <summary>
		/// The question text.
		/// </summary>
		[MaxLength(100)]
		public string Title { get; set; }

		/// <summary>
		/// The question's answer.
		/// </summary>
		[MaxLength(1000)]
		public string Answer { get; set; }

		/// <summary>
		/// The order this question appears in the category.
		/// </summary>
		public double Order { get; set; }

		/// <summary>
		/// The Date the question was last asked/studied.
		/// </summary>
		public DateTime LastAsked { get; set; }

		/// <summary>
		/// The Date the question is next due to be asked on.
		/// </summary>
		public DateTime NextAskOn { get; set; }

		/// <summary>
		/// The previous interval, which is used to calculate the interval. Storing this means we don't
		/// have to recursively calculate backwards.
		/// </summary>
		public int PreviousInterval { get; set; }

		/// <summary>
		/// The number of days until the question is next asked.
		/// </summary>
		public int Interval { get; set; }
		
		/// <summary>
		/// Number of times the question has been asked
		/// </summary>
		public int AskCount { get; set; }

		/// <summary>
		/// The ease that the question was answered. The following values should be used:
		/// <list type="bullet">
		/// <item>5 - perfect response</item>
		/// <item>4 - correct response after a hesitation</item>
		/// <item>3 - correct response recalled with serious difficulty</item>
		/// <item>2 - incorrect response; where the correct one seemed easy to recall</item>
		/// <item>1 - incorrect response; the correct one remembered</item>
		/// <item>0 - complete blackout.</item>
		/// http://www.supermemo.com/english/ol/sm2.htm
		/// </list>
		/// </summary>
		public int ResponseQuality { get; set; }

		/// <summary>
		/// Calculated from the quality of the response. The default is 2.5
		/// </summary>
		public double EasinessFactor { get; set; }
		#endregion
		
		public Question()
		{
			LastAsked = DateTime.Today;
			NextAskOn = DateTime.Today;
		}
		
		public static int Save(Question question)
		{
			using (SQLiteConnection connection = new SQLiteConnection(Settings.DatabaseFile))
			{
				connection.Trace = true;
				
				if (question.Id < 1)
					return connection.Insert(question);
				else
				{
					connection.Update(question);	
					return question.Id;
				}
			}
		}
		
		public static Question Read(int id)
		{
			using (SQLiteConnection connection = new SQLiteConnection(Settings.DatabaseFile))
			{
				return connection.Table<Question>().FirstOrDefault(q => q.Id == id);
			}
		}
		
		public static IList<Question> List()
		{
			using (SQLiteConnection connection = new SQLiteConnection(Settings.DatabaseFile))
			{
				return connection.Table<Question>().ToList();
			}
		}
		
		public static IList<Question> ForCategory(Category category)
		{
			using (SQLiteConnection connection = new SQLiteConnection(Settings.DatabaseFile))
			{
				connection.Trace = true;
				
				int id = category.Id; // This is a quirk with SQLite not getting the property value
				return connection.Table<Question>().Where(q => q.CategoryId == id).ToList();
			}
		}
		
		public static void Delete(int id)
		{
			using (SQLiteConnection connection = new SQLiteConnection(Settings.DatabaseFile))
			{
				connection.Delete<Question>(new Question { Id=id });
			}
		}
	}
}
