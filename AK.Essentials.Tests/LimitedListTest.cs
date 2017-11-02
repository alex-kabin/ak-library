using AK.Essentials.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AK.Essentials.Tests
{
    
    
    /// <summary>
    ///This is a test class for LimitedListTest and is intended
    ///to contain all LimitedListTest Unit Tests
    ///</summary>
	[TestClass()]
	public class LimitedListTest
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
		//You can use the following additional attributes as you write your tests:
		//
		//Use ClassInitialize to run code before running the first test in the class
		//[ClassInitialize()]
		//public static void MyClassInitialize(TestContext testContext)
		//{
		//}
		//
		//Use ClassCleanup to run code after all tests in a class have run
		//[ClassCleanup()]
		//public static void MyClassCleanup()
		//{
		//}
		//
		//Use TestInitialize to run code before running each test
		//[TestInitialize()]
		//public void MyTestInitialize()
		//{
		//}
		//
		//Use TestCleanup to run code after each test has run
		//[TestCleanup()]
		//public void MyTestCleanup()
		//{
		//}
		//
		#endregion


		/// <summary>
		///A test for Add
		///</summary>
		public void TestAddHelper<T>()
		{
			int size = 0; // TODO: Initialize to an appropriate value
			CircularCollection<T> target = new CircularCollection<T>(size); // TODO: Initialize to an appropriate value
			T item = default(T); // TODO: Initialize to an appropriate value
			target.Add(item);
			Assert.Inconclusive("A method that does not return a value cannot be verified.");
		}

		[TestMethod()]
		public void TestAdd()
		{
			TestAddHelper<GenericParameterHelper>();
		}

		/// <summary>
		///A test for Clear
		///</summary>
		public void TestClearHelper<T>()
		{
			int size = 0; // TODO: Initialize to an appropriate value
			CircularCollection<T> target = new CircularCollection<T>(size); // TODO: Initialize to an appropriate value
			target.Clear();
			Assert.Inconclusive("A method that does not return a value cannot be verified.");
		}

		[TestMethod()]
		public void TestClear()
		{
			TestClearHelper<GenericParameterHelper>();
		}

		/// <summary>
		///A test for Contains
		///</summary>
		public void TestContainsHelper<T>()
		{
			int size = 0; // TODO: Initialize to an appropriate value
			CircularCollection<T> target = new CircularCollection<T>(size); // TODO: Initialize to an appropriate value
			T item = default(T); // TODO: Initialize to an appropriate value
			bool expected = false; // TODO: Initialize to an appropriate value
			bool actual;
			actual = target.Contains(item);
			Assert.AreEqual(expected, actual);
			Assert.Inconclusive("Verify the correctness of this test method.");
		}

		[TestMethod()]
		public void TestContains()
		{
			TestContainsHelper<GenericParameterHelper>();
		}

		/// <summary>
		///A test for Remove
		///</summary>
		public void TestRemoveHelper<T>()
		{
			int size = 0; // TODO: Initialize to an appropriate value
			CircularCollection<T> target = new CircularCollection<T>(size); // TODO: Initialize to an appropriate value
			T item = default(T); // TODO: Initialize to an appropriate value
			bool expected = false; // TODO: Initialize to an appropriate value
			bool actual;
			actual = target.Remove(item);
			Assert.AreEqual(expected, actual);
			Assert.Inconclusive("Verify the correctness of this test method.");
		}

		[TestMethod()]
		public void TestRemove()
		{
			TestRemoveHelper<GenericParameterHelper>();
		}

		/// <summary>
		///A test for Count
		///</summary>
		public void TestCountHelper<T>()
		{
			int size = 0; // TODO: Initialize to an appropriate value
			CircularCollection<T> target = new CircularCollection<T>(size); // TODO: Initialize to an appropriate value
			int actual;
			actual = target.Count;
			Assert.Inconclusive("Verify the correctness of this test method.");
		}

		[TestMethod()]
		public void TestCount()
		{
			TestCountHelper<GenericParameterHelper>();
		}
	}
}
