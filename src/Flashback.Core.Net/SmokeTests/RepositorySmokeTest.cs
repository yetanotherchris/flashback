using System;
using Flashback.Core.Net.Repositories;
using Flashback.Core;

namespace Flashback.Core.Net.SmokeTests
{
    public static class RepositorySmokeTest
    {
        public static int Run(string dbPath)
        {
            try
            {
                Console.WriteLine($"Using DB path: {dbPath}");
                var repo = new SqliteRepository(dbPath);
                repo.DeleteDatabase();
                repo.CreateDatabase();

                // Create a category
                var cat = new Category { Name = "SmokeTest", InBuilt = false, Active = true };
                var catId = repo.SaveCategory(cat);
                Console.WriteLine($"Created category id: {catId}");

                // Create a question
                var q = new Question { Category = new Category { Id = catId }, Title = "Q1", Answer = "A1", AskCount = 0, LastAsked = DateTime.Now, NextAskOn = DateTime.Now, Order = 0, PreviousInterval = 0, Interval = 0, EasinessFactor = 0.0, ResponseQuality = 0 };
                var qId = repo.SaveQuestion(q);
                Console.WriteLine($"Created question id: {qId}");

                var read = repo.ReadQuestion(qId);
                Console.WriteLine($"Read question title: {read.Title}, answer: {read.Answer}");

                if (read.Title != "Q1" || read.Answer != "A1")
                {
                    Console.Error.WriteLine("Smoke test failed: data mismatch");
                    return 2;
                }

                Console.WriteLine("Smoke test passed");
                return 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Smoke test error: {ex}");
                return 3;
            }
        }
    }
}
