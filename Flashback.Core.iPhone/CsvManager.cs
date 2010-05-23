using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LumenWorks.Framework.IO.Csv;
using System.IO;

namespace Flashback.Core.iPhone
{
	/// <summary>
	/// Imports/exports data using the CSV format.
	/// </summary>
	public class CsvManager
	{
		/// <summary>
		/// Exports the provided questions in CSV format: category,question,answer
		/// </summary>
		/// <param name="questions"></param>
		/// <returns></returns>
		public static string Export(IList<Question> questions)
		{
			StringBuilder builder = new StringBuilder();
			
			foreach (Question question in questions.OrderBy(q => q.Category.Name))
			{
#if LOGGING
				builder.AppendLine(string.Format("{0},{1},{2},{3}",
					question.Category.Name.Replace(",", "~"),
					question.Title.Replace(",", "~"),
					question.Answer.Replace(",", "~"),
					question.NextAskOn.ToShortDateString())
					);
#else

				builder.AppendLine(string.Format("{0},{1},{2}",
					question.Category.Name.Replace(",","~"),
					question.Title.Replace(",","~"),
					question.Answer.Replace(",","~"))
					);
#endif
			}

			return builder.ToString();
		}

		/// <summary>
		/// Imports CSV formatted data: category,question,answer into a list of Questions. This doesn't bulk insert using a transaction.
		/// </summary>
		public static List<Question> Import(string data)
		{
			List<Question> list = new List<Question>();

			try
			{
				// Load all categories
				List<Category> categories = Category.List().ToList();

				using (StringReader stringReader = new StringReader(data))
				using (CsvReader reader = new CsvReader(stringReader, false, ','))
				{
					reader.SkipEmptyLines = true;

					while (reader.ReadNextRecord())
					{
						string categoryText = reader[0];
						string questionText = reader[1];
						string answer = reader[2];

						if (!string.IsNullOrEmpty(categoryText) && !string.IsNullOrEmpty(questionText) && !string.IsNullOrEmpty(answer))
						{
							// Find the category in the list
							Category category = categories.FirstOrDefault(c => c.Name.ToLower() == categoryText.ToLower() && !c.InBuilt);

							// Create a new one and save it if it's not there
							if (category == null)
							{
								category = new Category();
								category.Name = categoryText.Replace("~",",");
								Category.Save(category);

								categories = Category.List().ToList();
							}

							// Save it
							Question question = new Question();
							question.Category = category;
							question.Title = questionText.Replace("~", ",");
							question.Answer = answer.Replace("~", ",");
							Question.Save(question);
						}
					}
				}
			}
			catch (Exception e)
			{
				Logger.Fatal("Unable to import data: {0}", e);
			}

			return list;
		}
	}
}
