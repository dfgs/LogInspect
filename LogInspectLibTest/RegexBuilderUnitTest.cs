using LogInspect.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogInspect.ModelsTest
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
			regex = rb.BuildRegexPattern("NA","abc");
			Assert.AreEqual("abc", regex);
		}

		[TestMethod]
		public void ShouldMatchBasicPattern()
		{
			RegexBuilder rb;
			string regex;

			rb = new RegexBuilder();
			rb.Add("NS",new Pattern() { Name = "A", Value = "abc" });
			regex = rb.BuildRegexPattern("NS", "{A}");
			Assert.AreEqual("abc", regex);
			regex = rb.BuildRegexPattern("NS", "{NS.A}");
			Assert.AreEqual("abc", regex);
			regex = rb.BuildRegexPattern("NA", "{NS.A}");
			Assert.AreEqual("abc", regex);
			regex = rb.BuildRegexPattern("NB", "{A}");
			Assert.AreEqual("abc", regex);
		}

		[TestMethod]
		public void ShouldMatchBasicMixedPattern()
		{
			RegexBuilder rb;
			string regex;

			rb = new RegexBuilder();
			rb.Add("NS", new Pattern() { Name = "A", Value = "abc" });
			rb.Add("NS", new Pattern() { Name = "B", Value = "ghi" });
			regex = rb.BuildRegexPattern("NS", "{A}def{B}");
			Assert.AreEqual("abcdefghi", regex);
			regex = rb.BuildRegexPattern("NS", "{NS.A}def{NS.B}");
			Assert.AreEqual("abcdefghi", regex);
			regex = rb.BuildRegexPattern("NA", "{NS.A}def{NS.B}");
			Assert.AreEqual("abcdefghi", regex);
		}

		[TestMethod]
		public void ShouldMatchRecursivePattern()
		{
			RegexBuilder rb;
			string regex;

			rb = new RegexBuilder();
			rb.Add("NS", new Pattern() { Name = "A", Value = "a{B}c" });
			rb.Add("NS", new Pattern() { Name = "B", Value = "b" });
			regex = rb.BuildRegexPattern("NS", "{A}");
			Assert.AreEqual("abc", regex);
			regex = rb.BuildRegexPattern("NS", "{NS.A}");
			Assert.AreEqual("abc", regex);
			regex = rb.BuildRegexPattern("NA", "{NS.A}");
			Assert.AreEqual("abc", regex);
		}

		[TestMethod]
		public void ShouldMatchRegexWithSpecialChar()
		{
			RegexBuilder rb;
			string regex;

			rb = new RegexBuilder();
			regex = rb.BuildRegexPattern("NS","a{bc");
			Assert.AreEqual("a{bc", regex);
		}

		[TestMethod]
		public void ShouldMatchMixedPatternWithSpecialChar()
		{
			RegexBuilder rb;
			string regex;

			rb = new RegexBuilder();
			rb.Add("NS", new Pattern() { Name = "A", Value = "abc" });
			rb.Add("NS", new Pattern() { Name = "B", Value = "ghi" });
			regex = rb.BuildRegexPattern("NS","{A}d{ef{B}");
			Assert.AreEqual("abcd{efghi", regex);
		}

		[TestMethod]
		public void ShouldFailIfPatternNotFound()
		{
			RegexBuilder rb;

			rb = new RegexBuilder();
			rb.Add("NS", new Pattern() { Name = "A", Value = "a{B}c" });
			Assert.ThrowsException<KeyNotFoundException>(()=>rb.BuildRegexPattern("NS","{A}"));

		}

		[TestMethod]
		public void ShouldOverrideAnExistingPattern()
		{
			RegexBuilder rb;
			string regex;

			rb = new RegexBuilder();
			rb.Add("NS", new Pattern() { Name = "A", Value = "a{B}c" });
			rb.Add("NS", new Pattern() { Name = "B", Value = "b" });
			rb.Add("NS", new Pattern() { Name = "B", Value = "c" });
			regex = rb.BuildRegexPattern("NS","{A}");
			Assert.AreEqual("acc", regex);
		}

		[TestMethod]
		public void ShouldOverrideAnExistingPatternUsingDefaultNameSpace()
		{
			RegexBuilder rb;
			string regex;

			rb = new RegexBuilder();
			rb.Add("NS", new Pattern() { Name = "A", Value = "a{B}c" });
			rb.Add("NS", new Pattern() { Name = "B", Value = "b" });
			rb.Add("NA", new Pattern() { Name = "B", Value = "c" });
			regex = rb.BuildRegexPattern("NS", "{A}");
			Assert.AreEqual("abc", regex);
			regex = rb.BuildRegexPattern("NA", "{NS.A}");
			Assert.AreEqual("acc", regex);
		}

		[TestMethod]
		public void ShouldNotOverrideAnExistingPatternUsingExplicitNameSpace()
		{
			RegexBuilder rb;
			string regex;

			rb = new RegexBuilder();
			rb.Add("NS", new Pattern() { Name = "A", Value = "a{NS.B}c" });
			rb.Add("NS", new Pattern() { Name = "B", Value = "b" });
			rb.Add("NA", new Pattern() { Name = "B", Value = "c" });
			regex = rb.BuildRegexPattern("NS", "{A}");
			Assert.AreEqual("abc", regex);
			regex = rb.BuildRegexPattern("NA", "{NS.A}");
			Assert.AreEqual("abc", regex);
		}

	}

}
