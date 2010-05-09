using System;
using System.Collections.Generic;

namespace Flashback.Core
{
	public interface IRepository
	{
		void CreateDatabase();
		void DeleteDatabase();

		void DeleteCategory(int id);
		void DeleteQuestion(int id);
		IList<Category> ListCategories();
		IList<Question> ListQuestions();
		IList<Question> QuestionsForCategory(Category category);
		Category ReadCategory(int id);
		Question ReadQuestion(int id);
		int SaveCategory(Category category);
		int SaveQuestion(Question question);

		/// <summary>
		/// Moves the question to the new position, and re-orders all existing questions.
		/// </summary>
		/// <param name="question"></param>
		/// <param name="newIndex"></param>
		void MoveQuestion(Question question, int newIndex);
	}
}
