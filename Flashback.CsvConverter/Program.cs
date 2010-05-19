using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LumenWorks.Framework.IO.Csv;
using System.IO;

namespace Flashback.CsvConverter
{
	class Program
	{
		static void Main(string[] args)
		{
			args = new string[1];
			args[0] = @"C:\input.csv";

			StringBuilder builder = new StringBuilder();
			builder.AppendLine("BEGIN TRANSACTION;");

			string catFormat = "INSERT INTO categories (id,name,inbuilt,active) VALUES ({0},'{1}',1,0);";
			string questionformat = "INSERT INTO questions (categoryid,title,answer) VALUES ({0},'{1}','{2}');";
			Dictionary<string, int> categories = new Dictionary<string, int>();
			int idCounter = 1;

			using (StreamReader streamReader = new StreamReader(args[0]))
			{
				using (CsvReader reader = new CsvReader(streamReader, true, ','))
				{
					reader.SkipEmptyLines = true;

					while (reader.ReadNextRecord())
					{
						string categoryText = reader[0];
						string questionText = reader[1];
						string answer = reader[2];

						categoryText = categoryText.Replace("'", "''");
						questionText = questionText.Replace("'", "''");
						answer = answer.Replace("'", "''");

						if (!categories.ContainsKey(categoryText))
						{
							categories.Add(categoryText,++idCounter);
							
							builder.AppendLine();
							builder.AppendLine(string.Format("-- {0}", reader[0]));
							builder.AppendLine(string.Format(catFormat, idCounter,categoryText));
						}

						builder.AppendLine(string.Format(questionformat, idCounter, questionText, answer));
					}
				}
			}

			builder.AppendLine("COMMIT;");
			Console.WriteLine(builder.ToString());
			Console.Read();

			File.WriteAllText(@"C:\output.sql", builder.ToString());
		}
	}
}
