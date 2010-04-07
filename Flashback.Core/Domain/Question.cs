using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flashback.Core
{
	public partial class Question : BaseDataObject<Question>
	{
		#region Properties	
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
		/// </list>
		/// </summary>
		public int ResponseQuality { get; set; }

		/// <summary>
		/// Calculated from the quality of the response. The default is 2.5
		/// </summary>
		public double EasinessFactor { get; set; }
		#endregion
	}
}
