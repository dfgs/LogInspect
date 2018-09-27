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
	public class NameSpaceDictionaryUnitTest
	{


		[TestMethod]
		public void ShouldGetNamedItem()
		{
			NameSpaceDictionary<string> rb;

			rb = new NameSpaceDictionary<string>();
			rb.Add("nsA", "A", "a");
			Assert.AreEqual("a", rb.GetItem("A"));
			Assert.AreEqual("a", rb.GetItem("nsA","A"));
		}
		[TestMethod]
		public void ShouldGetFullNamedItem()
		{
			NameSpaceDictionary<string> rb;

			rb = new NameSpaceDictionary<string>();
			rb.Add("nsA", "A", "a");
			Assert.AreEqual("a", rb.GetItem("nsA.A"));
			Assert.AreEqual("a", rb.GetItem("nsA", "nsA.A"));
			Assert.AreEqual("a", rb.GetItem("nsB", "nsA.A"));
		}

		[TestMethod]
		public void ShouldOverrideItem()
		{
			NameSpaceDictionary<string> rb;

			rb = new NameSpaceDictionary<string>();
			rb.Add("nsA", "A", "a");
			rb.Add("nsB", "A", "b");
			Assert.AreEqual("b", rb.GetItem("A"));
		}

		[TestMethod]
		public void ShouldGetOverrideItem()
		{
			NameSpaceDictionary<string> rb;

			rb = new NameSpaceDictionary<string>();
			rb.Add("nsA", "A", "a");
			rb.Add("nsB", "A", "b");
			Assert.AreEqual("a", rb.GetItem("nsA.A"));
			Assert.AreEqual("a", rb.GetItem("nsA", "nsA.A"));
			Assert.AreEqual("a", rb.GetItem("nsB", "nsA.A"));
		}


	}
}