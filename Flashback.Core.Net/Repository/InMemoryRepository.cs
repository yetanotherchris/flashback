using System;
using System.Collections.Generic;
using System.Linq;

namespace Flashback.Core
{
    public class InMemoryRepository : IRepository
    {
        private readonly List<Category> _categories = new List<Category>();
        private readonly List<Question> _questions = new List<Question>();
        private int _nextCategoryId = 1;
        private int _nextQuestionId = 1;

        public void CreateDatabase()
        {
            // seed default data
            if (_categories.Count == 0)
            {
                var c = new Category { Id = _nextCategoryId++, Name = "General", Active = true, InBuilt = true };
                _categories.Add(c);

                _questions.Add(new Question { Id = _nextQuestionId++, Title = "What is Flashback?", Answer = "A spaced repetition app.", Category = c, NextAskOn = DateTime.Today });
                _questions.Add(new Question { Id = _nextQuestionId++, Title = "How to study?", Answer = "Repeat and review.", Category = c, NextAskOn = DateTime.Today.AddDays(1) });
            }
        }

        public void DeleteDatabase()
        {
            _categories.Clear();
            _questions.Clear();
        }

        public void DeleteCategory(int id)
        {
            _categories.RemoveAll(c => c.Id == id);
            _questions.RemoveAll(q => q.Category?.Id == id);
        }

        public void DeleteQuestion(int id)
        {
            _questions.RemoveAll(q => q.Id == id);
        }

        public IList<Category> ListCategories()
        {
            return _categories.ToList();
        }

        public IList<Question> ListQuestions()
        {
            return _questions.OrderBy(q => q.Order).ToList();
        }

        public IList<Question> QuestionsForCategory(Category category)
        {
            return _questions.Where(q => q.Category != null && q.Category.Id == category.Id).ToList();
        }

        public Category ReadCategory(int id)
        {
            return _categories.FirstOrDefault(c => c.Id == id);
        }

        public Question ReadQuestion(int id)
        {
            return _questions.FirstOrDefault(q => q.Id == id);
        }

        public int SaveCategory(Category category)
        {
            if (category.Id == 0)
            {
                category.Id = _nextCategoryId++;
                _categories.Add(category);
            }
            else
            {
                var idx = _categories.FindIndex(c => c.Id == category.Id);
                if (idx >= 0) _categories[idx] = category;
            }

            return category.Id;
        }

        public int SaveQuestion(Question question)
        {
            if (question.Id == 0)
            {
                question.Id = _nextQuestionId++;
                question.Order = _questions.Count;
                _questions.Add(question);
            }
            else
            {
                var idx = _questions.FindIndex(q => q.Id == question.Id);
                if (idx >= 0) _questions[idx] = question;
            }

            return question.Id;
        }

        public void MoveQuestion(Question question, int newIndex)
        {
            var q = _questions.FirstOrDefault(x => x.Id == question.Id);
            if (q == null) return;
            _questions.Remove(q);
            _questions.Insert(Math.Max(0, Math.Min(newIndex, _questions.Count)), q);
            for (int i = 0; i < _questions.Count; i++) _questions[i].Order = i;
        }
    }
}
