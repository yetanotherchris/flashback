using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vici.CoolStorage;

namespace Flashback.Core
{
	[MapTo("questions")]
	public class Question : CSObject<Question,Guid>
	{
		#region Properties
		/// <summary>
		/// Unique id of the question.
		/// </summary>
		public Guid Id 
		{ 
			get { return (Guid) GetField("Id"); } 
			set { SetField("Id", value); } 
		}

		/// <summary>
		/// The question text.
		/// </summary>
		public string Title
		{
			get { return (string)GetField("Title"); }
			set { SetField("Title", value); }
		}

		/// <summary>
		/// The question's answer.
		/// </summary>
		public string Answer
		{
			get { return (string)GetField("Answer"); }
			set { SetField("Answer", value); }
		}

		/// <summary>
		/// The order this question appears in the category.
		/// </summary>
		public double Order
		{
			get { return (double)GetField("Order"); }
			set { SetField("Order", value); }
		}

		/// <summary>
		/// The category for the question
		/// </summary>
		public Category Category
		{
			get { return (Category)GetField("Category"); }
			set { SetField("Category", value); }
		}

		/// <summary>
		/// The Date the question was last asked/studied.
		/// </summary>
		public DateTime LastAsked
		{
			get { return (DateTime)GetField("LastAsked"); }
			set { SetField("LastAsked", value); }
		}

		/// <summary>
		/// The Date the question is next due to be asked on.
		/// </summary>
		public DateTime NextAskOn
		{
			get { return (DateTime)GetField("NextAskOn"); }
			set { SetField("NextAskOn", value); }
		}

		/// <summary>
		/// The number of days until the question is next asked.
		/// </summary>
		public int Interval
		{
			get { return (int)GetField("Interval"); }
			set { SetField("Interval", value); }
		}

		/// <summary>
		/// Number of times the question has been asked
		/// </summary>
		public int AskCount
		{
			get { return (int)GetField("AskCount"); }
			set { SetField("AskCount", value); }
		}

		/// <summary>
		/// The ease that the question was answered. The following values should be used:
		/// <list type="bullet">
		/// <item>5 - perfect response</item>
		/// <item>4 - correct response after a hesitation</item>
		/// <item>3 - correct response recalled with serious difficulty</item>
		/// <item>2 - incorrect response; where the correct one seemed easy to recall</item>
		/// <item>1 - incorrect response; the correct one remembered</item>
		/// <item>0 - complete blackout.</item>
		/// </list>
		/// </summary>
		public int ResponseQuality
		{
			get { return (int)GetField("ResponseQuality"); }
			set { SetField("ResponseQuality", value); }
		}

		/// <summary>
		/// Calculated from the quality of the response. The default is 2.5
		/// </summary>
		public double EasinessFactor
		{
			get { return (double)GetField("EasinessFactor"); }
			set { SetField("EasinessFactor", value); }
		}
		#endregion

		#region Ctor
		/// <summary>
		/// Initializes a new instance of the <see cref="Question"/> class.
		/// </summary>
		public Question()
		{
			LastAsked = DateTime.Today;
			NextAskOn = DateTime.Today;
			AskCount = 0;
			Interval = 0;
			Order = 1;
		}
		#endregion
	}
}
