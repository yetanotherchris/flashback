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
		public void TestQuestion()
		{
			Category category = new Category();
			category.Name = "bob";
			category.Save();

			//Settings.ConfigureDatabase();
			//Question question = Question.New();
			//question.Title = "Capital of Ireland";
			//question.Answer = "Dublin";
			//question.Save();
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
