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
	public class CategoryTests
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
			Repository.Default.DeleteDatabase();
			Repository.Default.CreateDatabase();

			Category category = new Category();
			category.Name = "bob";
			Category.Save(category);

			Assert.AreEqual(1, category.Id);
		}

		[TestMethod]
		public void UpdateCategoryTest()
		{
			Repository.Default.DeleteDatabase();
			Repository.Default.CreateDatabase();

			// Create
			Category category = new Category();
			category.Name = "bob";
			Category.Save(category);

			// Update
			category = Category.Read(1);
			category.Name = "bob two";
			Category.Save(category);

			// Read back
			category = Category.Read(1);
			Assert.AreEqual(1, category.Id);
			Assert.AreEqual("bob two", category.Name);
		}

		[TestMethod]
		public void ReadCategoryTest()
		{
			Repository.Default.DeleteDatabase();
			Repository.Default.CreateDatabase();

			// Create
			Category category = new Category();
			category.Name = "bob";
			Category.Save(category);

			// Read back
			category = Category.Read(1);
			Assert.AreEqual(1, category.Id);
			Assert.AreEqual("bob", category.Name);

			//var s = Category.List(false, "@name", "bob", "@id", 2);
		}
	}
}
