using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Flashback.Core;

namespace Flashback.Tests.VS2010
{
	/// <summary>
	/// Summary description for UnitTest1
	/// </summary>
	[TestClass]
	public class CoreTests
	{
		private TestContext testContextInstance;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext
		{
			get
			{
				return testContextInstance;
			}
			set
			{
				testContextInstance = value;
			}
		}

		#region Additional test attributes
		//
		// You can use the following additional attributes as you write your tests:
		//
		// Use ClassInitialize to run code before running the first test in the class
		// [ClassInitialize()]
		// public static void MyClassInitialize(TestContext testContext) { }
		//
		// Use ClassCleanup to run code after all tests in a class have run
		// [ClassCleanup()]
		// public static void MyClassCleanup() { }
		//
		// Use TestInitialize to run code before running each test 
		// [TestInitialize()]
		// public void MyTestInitialize() { }
		//
		// Use TestCleanup to run code after each test has run
		// [TestCleanup()]
		// public void MyTestCleanup() { }
		//
		#endregion
		
		[TestMethod]
		public void AddCategoryTest()
		{
			Repository.Current.DeleteDatabase();
			Repository.Current.CreateDatabase();

			Category category = new Category();
			category.Name = "bob";
			category.Save();

			Assert.AreEqual(1, category.Id);

			category = Category.Read(1);
			Assert.AreEqual(1, category.Id);
			Assert.AreEqual("bob", category.Name);

			var s = Category.List(false, "@name", "bob","@id",2);
		}

		[TestMethod]
		public void AddQuestionTest()
		{
			Repository.Current.DeleteDatabase();
			Repository.Current.CreateDatabase();

			Category category = new Category();
			category.Name = "bob";
			category.Save();

			Question question = new Question();
			question.Title = "Is Fiona going to the baby?";
			question.Answer = "Yes, of course within seconds";
			question.AskCount = 60;
			question.Category = Category.Read(1);
			question.EasinessFactor = 70;
			question.Interval = 80;
			question.LastAsked = DateTime.Today.AddDays(-2);
			question.NextAskOn = DateTime.Today.AddDays(2);
			question.Order = 90;
			question.PreviousInterval = 100;
			question.ResponseQuality = 110;
			question.Save();

			Assert.AreEqual(1, question.Id);
		}

		[TestMethod]
		public void ReadQuestionTest()
		{
			Repository.Current.DeleteDatabase();
			Repository.Current.CreateDatabase();

			Category category = new Category();
			category.Name = "bob";
			category.Save();

			Question question = new Question();
			question.Title = "Is Fiona going to the baby?";
			question.Answer = "Yes, of course within seconds";
			question.AskCount = 60;
			question.Category = Category.Read(1);
			question.EasinessFactor = 70;
			question.Interval = 80;
			question.LastAsked = DateTime.Today.AddDays(-2);
			question.NextAskOn = DateTime.Today.AddDays(2);
			question.Order = 90;
			question.PreviousInterval = 100;
			question.ResponseQuality = 110;
			question.Save();

			question = Question.Read(1);
			Assert.AreEqual("Is Fiona going to the baby?", question.Title);
			Assert.AreEqual("Yes, of course within seconds", question.Answer);
			Assert.AreEqual(60, question.AskCount);
			Assert.AreEqual(1, question.Category.Id);
			Assert.AreEqual(70, question.EasinessFactor);
			Assert.AreEqual(DateTime.Today.AddDays(-2), question.LastAsked);
			Assert.AreEqual(DateTime.Today.AddDays(2), question.NextAskOn);
			Assert.AreEqual(90, question.Order);
			Assert.AreEqual(100, question.PreviousInterval);
			Assert.AreEqual(110, question.ResponseQuality);
		}

		/*
		void QuestionsFor()
		{
			CSList<Category> categories = Category.List();
			foreach (Category category in categories)
			{
				Console.WriteLine(category.Name);
			}
		}

		void AddQuestions()
		{

			for (int i = 0; i < 15; i++)
			{
				Category category = Category.New();
				category.Name = string.Format("Category {0}", i);

				for (int n = 0; n < 20; n++)
				{
					Question question = Question.New();
					question.Title = string.Format("Question {0} for {1}", n, i);
					question.Answer = "Some answer that is about a sentence or two in length but not much longer really: a b c d e";
					question.Category = category;
					question.Save();
				}
			}
		}*/
	}
}
