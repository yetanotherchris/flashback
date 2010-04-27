using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flashback.Core.Domain
{
	public class QuestionManager
	{
		/// <summary>
		/// Answers the question with the score provided. Also saves the question.
		/// </summary>
		/// <param name="question"></param>
		/// <param name="score"></param>
		public static void AnswerQuestion(Question question, int score)
		{
			// Calculate the dates
			question.AskCount++;
			question.ResponseQuality = score;
			SetEasinessFactor(question);
			SetInterval(question);

			// If the quality response was lower than 3 then start repetitions for the item from the beginning 
			// without changing the E-Factor (i.e. use intervals I(1), I(2) etc. as if the item was memorized anew).
			if (question.ResponseQuality < 3)
			{
				question.Interval = 1;
			}

			// This is used for testing so we don't have to fake dates
			//question.LastAsked = question.NextAskOn;

			// If it's the first ask use Today. Otherwise use the LastAsked, which may not necessarily be today.
			if (question.LastAsked == DateTime.MinValue)
				question.NextAskOn = DateTime.Today.AddDays(question.Interval);
			else
				question.NextAskOn = question.LastAsked.AddDays(question.Interval);

			question.LastAsked = DateTime.Today;
			// todo:question.Save();

			string format = "[{0}][{1}]\tScore:{2}\tEF:{3}\tNext ask: {4}\tLast ask:{5}\tPrevious interval: {6}\tNew interval: {7}";
			Logger.Info(format, question.Category,
										DateTime.Now.ToString(),
										question.ResponseQuality,
										question.EasinessFactor,
										question.NextAskOn.ToShortDateString(),
										question.LastAsked.ToShortDateString(),
										question.PreviousInterval,
										question.Interval);
		}

		/// <summary>
		/// Calculates the Easiness factor of a question for use before save.
		/// </summary>
		private static void SetEasinessFactor(Question question)
		{
			/// EF':=f(EF,q)
			/// - EF' - new value of the E-Factor
			/// - EF - old value of the E-Factor
			/// - q - quality of the response
			/// - f - function used in calculating EF'.
			/// EF':=EF-0.8+0.28*q-0.02*q*q
			double q2 = question.ResponseQuality * question.ResponseQuality; // just for ease of reading

			if (question.EasinessFactor < 1.3)
				question.EasinessFactor = 1.3;

			double newEf = question.EasinessFactor - 0.8 + 0.28 * question.ResponseQuality - 0.02 * q2;

			question.EasinessFactor = newEf;
		}

		/// <summary>
		/// Calculates how long the interval should be until the next ask.
		/// </summary>
		private static void SetInterval(Question question)
		{
			/// [1] = 1 day
			/// [2] = 6 days later
			/// for n>2 I(n):=I(n-1)*EF
			/// 
			/// In C#:
			/// interval[n] = interval[n-1] * EF

			int newPreviousInterval = question.Interval;

			// Zero is where it hasn't been asked yet, just added
			if (question.PreviousInterval < 1)
			{
				question.Interval = 1;	// first ask
			}
			else if (question.PreviousInterval == 1)
			{
				question.Interval = 6;	// 2nd ask
			}
			else if (question.PreviousInterval >= 6)
			{
				question.Interval = (int)Math.Round(question.PreviousInterval * question.EasinessFactor, MidpointRounding.AwayFromZero); // school rounding
			}

			question.PreviousInterval = newPreviousInterval;
		}
	}
}
