using System;
using System.Collections.Generic;
using System.Linq;

namespace Flashback.Core
{
    public class Question
    {
        public int Id { get; set; }
        public Category? Category { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;
        public double Order { get; set; }
        public DateTime LastAsked { get; set; }
        public DateTime NextAskOn { get; set; }
        public int PreviousInterval { get; set; }
        public int Interval { get; set; }
        public int AskCount { get; set; }
        public int ResponseQuality { get; set; }
        public double EasinessFactor { get; set; }

        public Question()
        {
            LastAsked = DateTime.MinValue;
            NextAskOn = DateTime.MinValue;
            AskCount = 0;
            Interval = 0;
            PreviousInterval = 0;
            ResponseQuality = 0;
            EasinessFactor = 0;
        }

        public static int Save(Question question)
        {
            var id = Repository.Default.SaveQuestion(question);
            question.Id = id;
            return id;
        }

        public static Question? Read(int id)
        {
            return Repository.Default.ReadQuestion(id);
        }

        public static void Move(Question question, int newIndex)
        {
            Repository.Default.MoveQuestion(question, newIndex);
        }

        public static void Delete(int id)
        {
            Repository.Default.DeleteQuestion(id);
        }

        public static IList<Question> List()
        {
            return Repository.Default.ListQuestions();
        }

        public static IEnumerable<Question> DueToday(IList<Question> list)
        {
            return list.Where(q => q.NextAskOn < DateTime.Today.AddDays(1));
        }

        public static IEnumerable<Question> ActiveDueToday(IList<Question> list)
        {
            return list.Where(q => q.NextAskOn < DateTime.Today.AddDays(1) && q.Category.Active);
        }

        public static DateTime NextDueDate(IList<Question> questions)
        {
            var question = questions.OrderByDescending(q => q.NextAskOn).FirstOrDefault();
            return question == null ? DateTime.Now : question.NextAskOn;
        }

        public static IList<Question> ForCategory(Category category)
        {
            return Repository.Default.QuestionsForCategory(category);
        }
    }
}
