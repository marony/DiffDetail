using System;
using System.Linq;
using DiffDetail;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void SplitTest1()
		{
			var splitter = Splitter.CreateSplitter(SplitType.Character, new NothingFilter());
			string testData = "str";
			var r = splitter.Split(testData).ToList();
			Assert.AreEqual("s", r[0]);
			Assert.AreEqual("t", r[1]);
			Assert.AreEqual("r", r[2]);
		}

		[TestMethod]
		public void SplitTest2()
		{
			var splitter = Splitter.CreateSplitter(SplitType.Character, new NothingFilter());
			string testData = "";
			var r = splitter.Split(testData).ToList();
			Assert.AreEqual(0, r.Count);
		}

		[TestMethod]
		public void DiffTest1()
		{
			var diffLogic = DiffLogic.CreateDiff(DiffLogicType.Simple, SplitType.Character, "");
			var r = diffLogic.Diff("str", "str").ToList();
			Assert.AreEqual("s", r[0].Lhs);
			Assert.AreEqual("s", r[0].Rhs);
			Assert.AreEqual(Difference.Same, r[0].Diff);
			Assert.AreEqual("t", r[1].Lhs);
			Assert.AreEqual("t", r[1].Rhs);
			Assert.AreEqual(Difference.Same, r[1].Diff);
			Assert.AreEqual("r", r[2].Lhs);
			Assert.AreEqual("r", r[2].Rhs);
			Assert.AreEqual(Difference.Same, r[2].Diff);
		}

		[TestMethod]
		public void DiffTest2()
		{
			var diffLogic = DiffLogic.CreateDiff(DiffLogicType.Simple, SplitType.Character, "");
			var r = diffLogic.Diff("s", "st").ToList();
			Assert.AreEqual("s", r[0].Lhs);
			Assert.AreEqual("s", r[0].Rhs);
			Assert.AreEqual(Difference.Same, r[0].Diff);
			Assert.AreEqual(null, r[1].Lhs);
			Assert.AreEqual("t", r[1].Rhs);
			Assert.AreEqual(Difference.Add, r[1].Diff);
		}

		[TestMethod]
		public void DiffTest3()
		{
			var diffLogic = DiffLogic.CreateDiff(DiffLogicType.Simple, SplitType.Character, "");
			var r = diffLogic.Diff("st", "s").ToList();
			Assert.AreEqual("s", r[0].Lhs);
			Assert.AreEqual("s", r[0].Rhs);
			Assert.AreEqual(Difference.Same, r[0].Diff);
			Assert.AreEqual("t", r[1].Lhs);
			Assert.AreEqual(null, r[1].Rhs);
			Assert.AreEqual(Difference.Remove, r[1].Diff);
		}

		[TestMethod]
		public void DiffTest4()
		{
			var diffLogic = DiffLogic.CreateDiff(DiffLogicType.Simple, SplitType.Character, "");
			var r = diffLogic.Diff("s", "ts").ToList();
			Assert.AreEqual(null, r[0].Lhs);
			Assert.AreEqual("t", r[0].Rhs);
			Assert.AreEqual(Difference.Add, r[0].Diff);
			Assert.AreEqual("s", r[1].Lhs);
			Assert.AreEqual("s", r[1].Rhs);
			Assert.AreEqual(Difference.Same, r[1].Diff);
		}

		[TestMethod]
		public void DiffTest5()
		{
			var diffLogic = DiffLogic.CreateDiff(DiffLogicType.Simple, SplitType.Character, "");
			var r = diffLogic.Diff("ts", "s").ToList();
			Assert.AreEqual("t", r[0].Lhs);
			Assert.AreEqual(null, r[0].Rhs);
			Assert.AreEqual(Difference.Remove, r[0].Diff);
			Assert.AreEqual("s", r[1].Lhs);
			Assert.AreEqual("s", r[1].Rhs);
			Assert.AreEqual(Difference.Same, r[1].Diff);
		}

		[TestMethod]
		public void DiffTest6()
		{
			var diffLogic = DiffLogic.CreateDiff(DiffLogicType.Simple, SplitType.Character, "");
			var r = diffLogic.Diff("str", "smr").ToList();
			Assert.AreEqual("s", r[0].Lhs);
			Assert.AreEqual("s", r[0].Rhs);
			Assert.AreEqual(Difference.Same, r[0].Diff);
			Assert.AreEqual("t", r[1].Lhs);
			Assert.AreEqual(null, r[1].Rhs);
			Assert.AreEqual(Difference.Remove, r[1].Diff);
			Assert.AreEqual(null, r[2].Lhs);
			Assert.AreEqual("m", r[2].Rhs);
			Assert.AreEqual(Difference.Add, r[2].Diff);
			Assert.AreEqual("r", r[3].Lhs);
			Assert.AreEqual("r", r[3].Rhs);
			Assert.AreEqual(Difference.Same, r[3].Diff);
		}

		[TestMethod]
		public void DiffTest7()
		{
			var diffLogic = DiffLogic.CreateDiff(DiffLogicType.Simple, SplitType.Character, "");
			var r = diffLogic.Diff("ts", "").ToList();
			Assert.AreEqual("t", r[0].Lhs);
			Assert.AreEqual(null, r[0].Rhs);
			Assert.AreEqual(Difference.Remove, r[0].Diff);
			Assert.AreEqual("s", r[1].Lhs);
			Assert.AreEqual(null, r[1].Rhs);
			Assert.AreEqual(Difference.Remove, r[1].Diff);
		}

		[TestMethod]
		public void DiffTest8()
		{
			var diffLogic = DiffLogic.CreateDiff(DiffLogicType.Simple, SplitType.Character, "");
			var r = diffLogic.Diff("", "ts").ToList();
			Assert.AreEqual(null, r[0].Lhs);
			Assert.AreEqual("t", r[0].Rhs);
			Assert.AreEqual(Difference.Add, r[0].Diff);
			Assert.AreEqual(null, r[1].Lhs);
			Assert.AreEqual("s", r[1].Rhs);
			Assert.AreEqual(Difference.Add, r[1].Diff);
		}

		[TestMethod]
		public void FilterTest1()
		{
			var str = @" abc ";
			var f = new CSharpFilter();
			Assert.AreEqual(@"abc", f.FilterElement(str));
			str = @"  ";
			f = new CSharpFilter();
			Assert.AreEqual(null, f.FilterElement(str));
			str = @" a   b c ";
			f = new CSharpFilter();
			Assert.AreEqual(@"a b c", f.FilterElement(str));
		}

		[TestMethod]
		public void FilterTest2()
		{
			var str = @" abc // Test A ";
			var f = new CSharpFilter();
			Assert.AreEqual(@"abc", f.FilterElement(str));
			str = @"/* Test */";
			Assert.AreEqual(null, f.FilterElement(str));
			str = @"a /* Test */ b /* Test */ c";
			Assert.AreEqual(@"a b c", f.FilterElement(str));
			str = @"a/* Test */b/* Test */c";
			Assert.AreEqual(@"a b c", f.FilterElement(str));
		}

		[TestMethod]
		public void FilterTest3()
		{
			var str = @"/* Test ";
			var f = new CSharpFilter();
			Assert.AreEqual(null, f.FilterElement(str));
			str = @"/* Test ";
			Assert.AreEqual(null, f.FilterElement(str));
			str = @" Test */";
			Assert.AreEqual(null, f.FilterElement(str));
			str = @" Test */";
			Assert.AreEqual(@"Test */", f.FilterElement(str));
		}

		[TestMethod]
		public void FilterTest4()
		{
			var str = @" ab /* Test ";
			var f = new CSharpFilter();
			Assert.AreEqual("ab", f.FilterElement(str));
			str = @" Test */ cd ";
			Assert.AreEqual(@"cd", f.FilterElement(str));
		}

		[TestMethod]
		public void TrivialTest1()
		{
			var t = "\"test\"\"test\"\"\"test";
			t = t.Replace("\"", "\"\"");
			Assert.AreEqual("\"\"test\"\"\"\"test\"\"\"\"\"\"test", t);
		}

		[TestMethod]
		public void TrivialTest2()
		{
			var t = "\r\n";
			t = t.Trim();
			Assert.AreEqual(string.Empty, t);
			t = "\r";
			t = t.Trim();
			Assert.AreEqual(string.Empty, t);
			t = "\n";
			t = t.Trim();
			Assert.AreEqual(string.Empty, t);
		}
	}
}
