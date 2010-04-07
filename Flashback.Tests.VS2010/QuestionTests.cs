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
	public class QuestionTests
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
			question.EasinessFactor = 70.50;
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
		public void UpdateQuestionTest()
		{
			Repository.Current.DeleteDatabase();
			Repository.Current.CreateDatabase();

			// Create
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

			// Update
			question.Title = "Is this updated?";
			question.Answer = "Yes, of course so";
			question.AskCount = 1;
			question.Category = Category.Read(1);
			question.EasinessFactor = 10.50;
			question.Interval = 20;
			question.LastAsked = DateTime.Today.AddDays(-4);
			question.NextAskOn = DateTime.Today.AddDays(4);
			question.Order = 30;
			question.PreviousInterval = 40;
			question.ResponseQuality = 50;
			question.Save();

			// Read back
			question = Question.Read(1);
			Assert.AreEqual("Is this updated?", question.Title);
			Assert.AreEqual("Yes, of course so", question.Answer);
			Assert.AreEqual(1, question.AskCount);
			Assert.AreEqual(1, question.Category.Id);
			Assert.AreEqual(10.50, question.EasinessFactor);
			Assert.AreEqual(DateTime.Today.AddDays(-4), question.LastAsked);
			Assert.AreEqual(DateTime.Today.AddDays(4), question.NextAskOn);
			Assert.AreEqual(30, question.Order);
			Assert.AreEqual(40, question.PreviousInterval);
			Assert.AreEqual(50, question.ResponseQuality);
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
			question.EasinessFactor = 70.50;
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
			Assert.AreEqual(70.50, question.EasinessFactor);
			Assert.AreEqual(DateTime.Today.AddDays(-2), question.LastAsked);
			Assert.AreEqual(DateTime.Today.AddDays(2), question.NextAskOn);
			Assert.AreEqual(90, question.Order);
			Assert.AreEqual(100, question.PreviousInterval);
			Assert.AreEqual(110, question.ResponseQuality);
		}
	}
}
