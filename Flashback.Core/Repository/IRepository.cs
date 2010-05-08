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
	}
}
