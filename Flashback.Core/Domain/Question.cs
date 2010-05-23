using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flashback.Core
{
	/// <summary>
	/// Represents a single question and its answer, and all Supermemo related metadata.
	/// </summary>
	public class Question
	{
		#region Properties
		/// <summary>
		/// The unique id of the question.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// The category for the question
		/// </summary>
		public Category Category { get; set; }

		/// <summary>
		/// The question text.
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// The question's answer.
		/// </summary>
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
			Reset();
		}
		
		/// <summary>
		/// Resets the question to its defaults.
		/// </summary>
		public void Reset()
		{
			LastAsked = DateTime.MinValue;
			NextAskOn = DateTime.MinValue;
			AskCount = 0;
			Interval = 0;
			PreviousInterval = 0;
			ResponseQuality = 0;
			EasinessFactor = 0;
		}
		
		/// <summary>
		/// Displays the <see cref="Question.Title"/>
		/// </summary>
		/// <returns></returns>
		public override string ToString ()
		{
			return Title;
		}

		/// <summary>
		/// Saves the question to the default repository, returning its id.
		/// </summary>
		/// <param name="question"></param>
		/// <returns></returns>
		public static int Save(Question question)
		{
			return Repository.Default.SaveQuestion(question);
		}

		/// <summary>
		/// Retrieves a question from the database repository based on the id provided.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static Question Read(int id)
		{
			return Repository.Default.ReadQuestion(id);
		}

		/// <summary>
		/// Moves the provided question to the new index in the list, and updates all other questions.
		/// </summary>
		/// <param name="question"></param>
		/// <param name="newIndex"></param>
		public static void Move(Question question, int newIndex)
		{
			Repository.Default.MoveQuestion(question, newIndex);
		}

		/// <summary>
		/// Deletes the question using the provided id.
		/// </summary>
		/// <param name="id"></param>
		public static void Delete(int id)
		{
			Repository.Default.DeleteQuestion(id);
		}

		/// <summary>
		/// Retrieves all questions from the database repository.
		/// </summary>
		/// <returns></returns>
		public static IList<Question> List()
		{
			return Repository.Default.ListQuestions();
		}

		/// <summary>
		/// Retrieves all questions from the database repository that are over due or due today.
		/// </summary>
		/// <param name="list"></param>
		/// <returns></returns>
		public static IEnumerable<Question> DueToday(IList<Question> list)
		{
			return list.Where(q => q.NextAskOn < DateTime.Today.AddDays(1));
		}
		
		/// <summary>
		/// Filters the list of questions provided to ones due today, with an active category.
		/// </summary>
		/// <param name="list"></param>
		/// <returns></returns>
		public static IEnumerable<Question> ActiveDueToday(IList<Question> list)
		{
			return list.Where(q => q.NextAskOn < DateTime.Today.AddDays(1) && q.Category.Active);
		}
		
		/// <summary>
		/// Gets the first question from the list's <see cref="Question.NextAskOn"/> property. This is
		/// displayed in the category hub.
		/// </summary>
		/// <param name="questions"></param>
		/// <returns></returns>
		public static DateTime NextDueDate(IList<Question> questions)
		{
			Question question = questions.OrderByDescending(q => q.NextAskOn).FirstOrDefault();
			
			if (question == null)
				return DateTime.Now;
			else
				return question.NextAskOn;
		}

		/// <summary>
		/// Gets a list of all questions for a category from the database repository.
		/// </summary>
		/// <param name="category"></param>
		/// <returns></returns>
		public static IList<Question> ForCategory(Category category)
		{
			return Repository.Default.QuestionsForCategory(category);
		}
	}
}
