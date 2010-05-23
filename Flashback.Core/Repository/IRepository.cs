using System;
using System.Collections.Generic;

namespace Flashback.Core
{
	/// <summary>
	/// Represents a database repository for CRUD operations.
	/// </summary>
	public interface IRepository
	{
		/// <summary>
		/// Creates the database on disk.
		/// </summary>
		void CreateDatabase();

		/// <summary>
		/// Deletes the database from disk.
		/// </summary>
		void DeleteDatabase();

		/// <summary>
		/// Deletes the category for the provided id, and all questions that belong to it.
		/// </summary>
		/// <param name="id"></param>
		void DeleteCategory(int id);

		/// <summary>
		/// Deletes the question for the provided id.
		/// </summary>
		/// <param name="id"></param>
		void DeleteQuestion(int id);

		/// <summary>
		/// Retrieves all categories stored in the database.
		/// </summary>
		/// <returns></returns>
		IList<Category> ListCategories();

		/// <summary>
		/// Retrieves all questions in the database.
		/// </summary>
		/// <returns></returns>
		IList<Question> ListQuestions();

		/// <summary>
		/// Retrieves all questions for the provided category.
		/// </summary>
		/// <param name="category"></param>
		/// <returns></returns>
		IList<Question> QuestionsForCategory(Category category);

		/// <summary>
		/// Retrieves a single category using the provided id.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Category ReadCategory(int id);

		/// <summary>
		/// Retrieves a single question using the provided id.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Question ReadQuestion(int id);

		/// <summary>
		/// Saves an existing or new category. The category's id is returned.
		/// </summary>
		/// <param name="category"></param>
		/// <returns></returns>
		int SaveCategory(Category category);


		/// <summary>
		/// Saves an existing or new question. The question's id is returned.
		/// </summary>
		/// <param name="question"></param>
		/// <returns></returns>
		int SaveQuestion(Question question);

		/// <summary>
		/// Moves the question to the new position, and re-orders all existing questions,
		/// so the <see cref="Question.Order"/> is correct.
		/// </summary>
		/// <param name="question"></param>
		/// <param name="newIndex"></param>
		void MoveQuestion(Question question, int newIndex);
	}
}
