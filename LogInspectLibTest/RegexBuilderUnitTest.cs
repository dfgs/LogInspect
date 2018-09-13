using LogInspectLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogInspectLibTest
{
	[TestClass]
	public class RegexBuilderUnitTest
	{


		[TestMethod]
		public void ShouldMatchBasicRegex()
		{
			RegexBuilder rb;
			string regex;

			rb = new RegexBuilder();
			regex = rb.BuildRegexPattern("abc");
			Assert.AreEqual("abc", regex);
		}

		[TestMethod]
		public void ShouldMatchBasicPattern()
		{
			RegexBuilder rb;
			string regex;

			rb = new RegexBuilder();
			rb.Add(new Pattern() { Name = "A", Value = "abc" });
			regex = rb.BuildRegexPattern("{A}");
			Assert.AreEqual("abc", regex);
		}

		[TestMethod]
		public void ShouldMatchBasicMixedPattern()
		{
			RegexBuilder rb;
			string regex;

			rb = new RegexBuilder();
			rb.Add(new Pattern() { Name = "A", Value = "abc" });
			rb.Add(new Pattern() { Name = "B", Value = "ghi" });
			regex = rb.BuildRegexPattern("{A}def{B}");
			Assert.AreEqual("abcdefghi", regex);
		}

		[TestMethod]
		public void ShouldMatchRecursivePattern()
		{
			RegexBuilder rb;
			string regex;

			rb = new RegexBuilder();
			rb.Add(new Pattern() { Name = "A", Value = "a{B}c" });
			rb.Add(new Pattern() { Name = "B", Value = "b" });
			regex = rb.BuildRegexPattern("{A}");
			Assert.AreEqual("abc", regex);
		}

		[TestMethod]
		public void ShouldMatchRegexWithSpecialChar()
		{
			RegexBuilder rb;
			string regex;

			rb = new RegexBuilder();
			regex = rb.BuildRegexPattern("a{bc");
			Assert.AreEqual("a{bc", regex);
		}

		[TestMethod]
		public void ShouldMatchMixedPatternWithSpecialChar()
		{
			RegexBuilder rb;
			string regex;

			rb = new RegexBuilder();
			rb.Add(new Pattern() { Name = "A", Value = "abc" });
			rb.Add(new Pattern() { Name = "B", Value = "ghi" });
			regex = rb.BuildRegexPattern("{A}d{ef{B}");
			Assert.AreEqual("abcd{efghi", regex);
		}

		[TestMethod]
		public void ShouldFailIfPatternNotFound()
		{
			RegexBuilder rb;

			rb = new RegexBuilder();
			rb.Add(new Pattern() { Name = "A", Value = "a{B}c" });
			Assert.ThrowsException<KeyNotFoundException>(()=>rb.BuildRegexPattern("{A}"));
		}

		[TestMethod]
		public void ShouldOverrideAnExistingPattern()
		{
			RegexBuilder rb;
			string regex;

			rb = new RegexBuilder();
			rb.Add(new Pattern() { Name = "A", Value = "a{B}c" });
			rb.Add(new Pattern() { Name = "B", Value = "b" });
			rb.Add(new Pattern() { Name = "B", Value = "c" });
			regex = rb.BuildRegexPattern("{A}");
			Assert.AreEqual("acc", regex);
		}


	}

}
