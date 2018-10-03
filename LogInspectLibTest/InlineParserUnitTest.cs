using System;
using System.Collections;
using System.Linq;
using LogInspectLib;
using LogInspectLib.Parsers;
using LogInspectLibTest.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogInspectLibTest
{
	[TestClass]
	public class InlineParserUnitTest
	{

		[TestMethod]
		public void ShouldHaveCorrectConstructorParameters()
		{
			Assert.ThrowsException<ArgumentNullException>(() => { new InlineParser(null); });
		}

		
		[TestMethod]
		public void ShouldReadOrderedContiguous()
		{
			InlineParser parser;
			Column column;
			Inline[] inlines;


			column = new Column() { Name = "C1" };
			column.InlineColoringRules.Add("Red");
			column.InlineColoringRules.Add("Green");
			column.InlineColoringRules.Add("Blue");
			column.InlineColoringRules.Add("White");
			column.InlineColoringRules.Add("Black");

			parser = new InlineParser(Utils.EmptyRegexBuilder);
			parser.Add("NS", new InlineColoringRule() { Name="Red", Pattern = "Red" });
			parser.Add("NS", new InlineColoringRule() { Name = "Green", Pattern = "Green" });
			parser.Add("NS", new InlineColoringRule() { Name = "Blue", Pattern = "Blue" });
			parser.Add("NS", new InlineColoringRule() { Name = "White", Pattern = "White" });
			parser.Add("NS", new InlineColoringRule() { Name = "Black", Pattern = "Black" });

			inlines = parser.Parse("RedGreenBlueBlackWhite").ToArray();
			Assert.AreEqual(5, inlines.Length);
			Assert.AreEqual("Red", inlines[0].Value);
			Assert.AreEqual("Green", inlines[1].Value);
			Assert.AreEqual("Blue", inlines[2].Value);
			Assert.AreEqual("Black", inlines[3].Value);
			Assert.AreEqual("White", inlines[4].Value);
		}
		[TestMethod]
		public void ShouldReadUnorderedContiguous()
		{
			InlineParser parser;
			Column column;
			Inline[] inlines;

			column = new Column() { Name = "C1" };
			column.InlineColoringRules.Add("Blue");
			column.InlineColoringRules.Add("Red");
			column.InlineColoringRules.Add("Green");
			column.InlineColoringRules.Add("White");
			column.InlineColoringRules.Add("Black");

			parser = new InlineParser(Utils.EmptyRegexBuilder);
			parser.Add("NS", new InlineColoringRule() { Name = "Red", Pattern = "Red" });
			parser.Add("NS", new InlineColoringRule() { Name = "Green", Pattern = "Green" });
			parser.Add("NS", new InlineColoringRule() { Name = "Blue", Pattern = "Blue" });
			parser.Add("NS", new InlineColoringRule() { Name = "White", Pattern = "White" });
			parser.Add("NS", new InlineColoringRule() { Name = "Black", Pattern = "Black" });

			inlines = parser.Parse("RedGreenBlueBlackWhite").ToArray();
			Assert.AreEqual(5, inlines.Length);
			Assert.AreEqual("Red", inlines[0].Value);
			Assert.AreEqual("Green", inlines[1].Value);
			Assert.AreEqual("Blue", inlines[2].Value);
			Assert.AreEqual("Black", inlines[3].Value);
			Assert.AreEqual("White", inlines[4].Value);
		}

		[TestMethod]
		public void ShouldReadOrderedNonContiguous()
		{
			InlineParser parser;
			Column column;
			Inline[] inlines;


			column = new Column() { Name = "C1" };
			column.InlineColoringRules.Add("Red");
			column.InlineColoringRules.Add("Green");
			column.InlineColoringRules.Add("Blue");
			column.InlineColoringRules.Add("White");
			column.InlineColoringRules.Add("Black");

			parser = new InlineParser(Utils.EmptyRegexBuilder);
			parser.Add("NS", new InlineColoringRule() { Name = "Red", Pattern = "Red" });
			parser.Add("NS", new InlineColoringRule() { Name = "Green", Pattern = "Green" });
			parser.Add("NS", new InlineColoringRule() { Name = "Blue", Pattern = "Blue" });
			parser.Add("NS", new InlineColoringRule() { Name = "White", Pattern = "White" });
			parser.Add("NS", new InlineColoringRule() { Name = "Black", Pattern = "Black" });

			 inlines = parser.Parse("Red_Green_Blue_Black_White").ToArray();
			Assert.AreEqual(9, inlines.Length);
			Assert.AreEqual("Red", inlines[0].Value);
			Assert.AreEqual("_", inlines[1].Value);
			Assert.AreEqual("Green", inlines[2].Value);
			Assert.AreEqual("_", inlines[3].Value);
			Assert.AreEqual("Blue", inlines[4].Value);
			Assert.AreEqual("_", inlines[5].Value);
			Assert.AreEqual("Black", inlines[6].Value);
			Assert.AreEqual("_", inlines[7].Value);
			Assert.AreEqual("White", inlines[8].Value);
		}

		[TestMethod]
		public void ShouldReadUnorderedNonContiguous()
		{
			InlineParser parser;
			Column column;
			Inline[] inlines;

			column = new Column() { Name = "C1" };
			column.InlineColoringRules.Add("Blue");
			column.InlineColoringRules.Add("Red");
			column.InlineColoringRules.Add("Green");
			column.InlineColoringRules.Add("White");
			column.InlineColoringRules.Add("Black");

			parser = new InlineParser(Utils.EmptyRegexBuilder);
			parser.Add("NS", new InlineColoringRule() { Name = "Red", Pattern = "Red" });
			parser.Add("NS", new InlineColoringRule() { Name = "Green", Pattern = "Green" });
			parser.Add("NS", new InlineColoringRule() { Name = "Blue", Pattern = "Blue" });
			parser.Add("NS", new InlineColoringRule() { Name = "White", Pattern = "White" });
			parser.Add("NS", new InlineColoringRule() { Name = "Black", Pattern = "Black" });

			inlines = parser.Parse("Red_Green_Blue_Black_White").ToArray();
			Assert.AreEqual(9, inlines.Length);
			Assert.AreEqual("Red", inlines[0].Value);
			Assert.AreEqual("_", inlines[1].Value);
			Assert.AreEqual("Green", inlines[2].Value);
			Assert.AreEqual("_", inlines[3].Value);
			Assert.AreEqual("Blue", inlines[4].Value);
			Assert.AreEqual("_", inlines[5].Value);
			Assert.AreEqual("Black", inlines[6].Value);
			Assert.AreEqual("_", inlines[7].Value);
			Assert.AreEqual("White", inlines[8].Value);
		}

		[TestMethod]
		public void ShouldReadHoleAtStart()
		{
			InlineParser parser;
			Column column;
			Inline[] inlines;


			column = new Column() { Name = "C1" };
			column.InlineColoringRules.Add("Red");
			column.InlineColoringRules.Add("Green");
			column.InlineColoringRules.Add("Blue");
			column.InlineColoringRules.Add("White");
			column.InlineColoringRules.Add("Black");

			parser = new InlineParser(Utils.EmptyRegexBuilder);
			parser.Add("NS", new InlineColoringRule() { Name = "Red", Pattern = "Red" });
			parser.Add("NS", new InlineColoringRule() { Name = "Green", Pattern = "Green" });
			parser.Add("NS", new InlineColoringRule() { Name = "Blue", Pattern = "Blue" });
			parser.Add("NS", new InlineColoringRule() { Name = "White", Pattern = "White" });
			parser.Add("NS", new InlineColoringRule() { Name = "Black", Pattern = "Black" });

			inlines = parser.Parse("__Red_Green_Blue_Black_White").ToArray();
			Assert.AreEqual(10, inlines.Length);
			Assert.AreEqual("__", inlines[0].Value);
			Assert.AreEqual("Red", inlines[1].Value);
			Assert.AreEqual("_", inlines[2].Value);
			Assert.AreEqual("Green", inlines[3].Value);
			Assert.AreEqual("_", inlines[4].Value);
			Assert.AreEqual("Blue", inlines[5].Value);
			Assert.AreEqual("_", inlines[6].Value);
			Assert.AreEqual("Black", inlines[7].Value);
			Assert.AreEqual("_", inlines[8].Value);
			Assert.AreEqual("White", inlines[9].Value);
		}

		[TestMethod]
		public void ShouldReadHoleAtEnd()
		{
			InlineParser parser;
			Column column;
			Inline[] inlines;


			column = new Column() { Name = "C1" };
			column.InlineColoringRules.Add("Red");
			column.InlineColoringRules.Add("Green");
			column.InlineColoringRules.Add("Blue");
			column.InlineColoringRules.Add("White");
			column.InlineColoringRules.Add("Black");

			parser = new InlineParser(Utils.EmptyRegexBuilder);
			parser.Add("NS", new InlineColoringRule() { Name = "Red", Pattern = "Red" });
			parser.Add("NS", new InlineColoringRule() { Name = "Green", Pattern = "Green" });
			parser.Add("NS", new InlineColoringRule() { Name = "Blue", Pattern = "Blue" });
			parser.Add("NS", new InlineColoringRule() { Name = "White", Pattern = "White" });
			parser.Add("NS", new InlineColoringRule() { Name = "Black", Pattern = "Black" });

			inlines = parser.Parse("Red_Green_Blue_Black_White__").ToArray();
			Assert.AreEqual(10, inlines.Length);
			Assert.AreEqual("Red", inlines[0].Value);
			Assert.AreEqual("_", inlines[1].Value);
			Assert.AreEqual("Green", inlines[2].Value);
			Assert.AreEqual("_", inlines[3].Value);
			Assert.AreEqual("Blue", inlines[4].Value);
			Assert.AreEqual("_", inlines[5].Value);
			Assert.AreEqual("Black", inlines[6].Value);
			Assert.AreEqual("_", inlines[7].Value);
			Assert.AreEqual("White", inlines[8].Value);
			Assert.AreEqual("__", inlines[9].Value);
		}


		[TestMethod]
		public void ShouldIgnoreCase()
		{
			InlineParser parser;
			Column column;
			Inline[] inlines;


			column = new Column() { Name = "C1" };
			column.InlineColoringRules.Add("Red");
			column.InlineColoringRules.Add("Green");
			column.InlineColoringRules.Add("Blue");
			column.InlineColoringRules.Add("White");
			column.InlineColoringRules.Add("Black");

			parser = new InlineParser(Utils.EmptyRegexBuilder);
			parser.Add("NS", new InlineColoringRule() { Name = "Red", Pattern = "red",IgnoreCase=true });
			parser.Add("NS", new InlineColoringRule() { Name = "Green", Pattern = "grEen", IgnoreCase = true });
			parser.Add("NS", new InlineColoringRule() { Name = "Blue", Pattern = "blUE", IgnoreCase = true });
			parser.Add("NS", new InlineColoringRule() { Name = "White", Pattern = "wHitE", IgnoreCase = true });
			parser.Add("NS", new InlineColoringRule() { Name = "Black", Pattern = "BLACK", IgnoreCase = true });

			inlines = parser.Parse("RedGreenBlueBlackWhite").ToArray();
			Assert.AreEqual(5, inlines.Length);
			Assert.AreEqual("Red", inlines[0].Value);
			Assert.AreEqual("Green", inlines[1].Value);
			Assert.AreEqual("Blue", inlines[2].Value);
			Assert.AreEqual("Black", inlines[3].Value);
			Assert.AreEqual("White", inlines[4].Value);
		}

		[TestMethod]
		public void ShouldReadWhenAIsPrioritaryAndIntersectType1()
		{
			InlineParser parser;
			Column column;
			Inline[] inlines;


			column = new Column() { Name = "C1" };
			column.InlineColoringRules.Add("A");
			column.InlineColoringRules.Add("B");

			parser = new InlineParser(Utils.EmptyRegexBuilder);
			parser.Add("NS", new InlineColoringRule() { Name = "A", Pattern = "2345678" });
			parser.Add("NS", new InlineColoringRule() { Name = "B", Pattern = "4567" });

			inlines = parser.Parse("01234567890").ToArray();
			Assert.AreEqual(3, inlines.Length);
			Assert.AreEqual("01", inlines[0].Value);
			Assert.AreEqual("2345678", inlines[1].Value);
			Assert.AreEqual("90", inlines[2].Value);
		}
		[TestMethod]
		public void ShouldReadWhenAIsPrioritaryAndIntersectType2()
		{
			InlineParser parser;
			Column column;
			Inline[] inlines;


			column = new Column() { Name = "C1" };
			column.InlineColoringRules.Add("A");
			column.InlineColoringRules.Add("B");

			parser = new InlineParser(Utils.EmptyRegexBuilder);
			parser.Add("NS", new InlineColoringRule() { Name = "A", Pattern = "2345678" });
			parser.Add("NS", new InlineColoringRule() { Name = "B", Pattern = "1234" });

			inlines = parser.Parse("01234567890").ToArray();
			Assert.AreEqual(3, inlines.Length);
			Assert.AreEqual("01", inlines[0].Value);
			Assert.AreEqual("2345678", inlines[1].Value);
			Assert.AreEqual("90", inlines[2].Value);
		}
		[TestMethod]
		public void ShouldReadWhenAIsPrioritaryAndIntersectType3()
		{
			InlineParser parser;
			Column column;
			Inline[] inlines;


			column = new Column() { Name = "C1" };
			column.InlineColoringRules.Add("A");
			column.InlineColoringRules.Add("B");

			parser = new InlineParser(Utils.EmptyRegexBuilder);
			parser.Add("NS", new InlineColoringRule() { Name = "A", Pattern = "2345678" });
			parser.Add("NS", new InlineColoringRule() { Name = "B", Pattern = "6789" });

			inlines = parser.Parse("01234567890").ToArray();
			Assert.AreEqual(3, inlines.Length);
			Assert.AreEqual("01", inlines[0].Value);
			Assert.AreEqual("2345678", inlines[1].Value);
			Assert.AreEqual("90", inlines[2].Value);
		}


		[TestMethod]
		public void ShouldReadWhenBIsPrioritaryAndIntersectType1()
		{
			InlineParser parser;
			Column column;
			Inline[] inlines;


			column = new Column() { Name = "C1" };
			column.InlineColoringRules.Add("A");
			column.InlineColoringRules.Add("B");

			parser = new InlineParser(Utils.EmptyRegexBuilder);
			parser.Add("NS", new InlineColoringRule() { Name = "B", Pattern = "4567" });
			parser.Add("NS", new InlineColoringRule() { Name = "A", Pattern = "2345678" });

			inlines = parser.Parse("01234567890").ToArray();
			Assert.AreEqual(3, inlines.Length);
			Assert.AreEqual("0123", inlines[0].Value);
			Assert.AreEqual("4567", inlines[1].Value);
			Assert.AreEqual("890", inlines[2].Value);
		}
		[TestMethod]
		public void ShouldReadWhenBIsPrioritaryAndIntersectType2()
		{
			InlineParser parser;
			Column column;
			Inline[] inlines;


			column = new Column() { Name = "C1" };
			column.InlineColoringRules.Add("B");
			column.InlineColoringRules.Add("A");

			parser = new InlineParser(Utils.EmptyRegexBuilder);
			parser.Add("NS", new InlineColoringRule() { Name = "B", Pattern = "1234" });
			parser.Add("NS", new InlineColoringRule() { Name = "A", Pattern = "2345678" });

			inlines = parser.Parse("01234567890").ToArray();
			Assert.AreEqual(3, inlines.Length);
			Assert.AreEqual("0", inlines[0].Value);
			Assert.AreEqual("1234", inlines[1].Value);
			Assert.AreEqual("567890", inlines[2].Value);
		}
		[TestMethod]
		public void ShouldReadWhenBIsPrioritaryAndIntersectType3()
		{
			InlineParser parser;
			Column column;
			Inline[] inlines;


			column = new Column() { Name = "C1" };
			column.InlineColoringRules.Add("B");
			column.InlineColoringRules.Add("A");

			parser = new InlineParser(Utils.EmptyRegexBuilder);
			parser.Add("NS", new InlineColoringRule() { Name = "B", Pattern = "6789" });
			parser.Add("NS", new InlineColoringRule() { Name = "A", Pattern = "2345678" });

			inlines = parser.Parse("01234567890").ToArray();
			Assert.AreEqual(3, inlines.Length);
			Assert.AreEqual("012345", inlines[0].Value);
			Assert.AreEqual("6789", inlines[1].Value);
			Assert.AreEqual("0", inlines[2].Value);
		}


	}
}
