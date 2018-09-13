using System;
using System.Collections;
using System.Linq;
using LogInspectLib;
using LogInspectLib.Parsers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogInspectLibTest
{
	[TestClass]
	public class InlineParserUnitTest
	{
		private static IRegexBuilder regexBuilder = new RegexBuilder();

		[TestMethod]
		public void ShouldReadOrderedContiguous()
		{
			InlineParser parser;
			Column column;
			Inline[] inlines;

			column = new Column() { Name = "C1" };
			column.InlineColoringRules.Add(new InlineColoringRule() { Foreground = "Red", Pattern = "Red" });
			column.InlineColoringRules.Add(new InlineColoringRule() { Foreground = "Green", Pattern = "Green" });
			column.InlineColoringRules.Add(new InlineColoringRule() { Foreground = "Blue", Pattern = "Blue" });
			column.InlineColoringRules.Add(new InlineColoringRule() { Foreground = "White", Pattern = "White" });
			column.InlineColoringRules.Add(new InlineColoringRule() { Foreground = "Black", Pattern = "Black" });

			parser = new InlineParser(column,regexBuilder);
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
			column.InlineColoringRules.Add(new InlineColoringRule() { Foreground = "Blue", Pattern = "Blue" });
			column.InlineColoringRules.Add(new InlineColoringRule() { Foreground = "Red", Pattern = "Red" });
			column.InlineColoringRules.Add(new InlineColoringRule() { Foreground = "Green", Pattern = "Green" });
			column.InlineColoringRules.Add(new InlineColoringRule() { Foreground = "Black", Pattern = "Black" });
			column.InlineColoringRules.Add(new InlineColoringRule() { Foreground = "White", Pattern = "White" });

			parser = new InlineParser(column, regexBuilder);
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
			column.InlineColoringRules.Add(new InlineColoringRule() { Foreground = "Red", Pattern = "Red" });
			column.InlineColoringRules.Add(new InlineColoringRule() { Foreground = "Green", Pattern = "Green" });
			column.InlineColoringRules.Add(new InlineColoringRule() { Foreground = "Blue", Pattern = "Blue" });
			column.InlineColoringRules.Add(new InlineColoringRule() { Foreground = "White", Pattern = "White" });
			column.InlineColoringRules.Add(new InlineColoringRule() { Foreground = "Black", Pattern = "Black" });

			parser = new InlineParser(column, regexBuilder);
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
			column.InlineColoringRules.Add(new InlineColoringRule() { Foreground = "Blue", Pattern = "Blue" });
			column.InlineColoringRules.Add(new InlineColoringRule() { Foreground = "Red", Pattern = "Red" });
			column.InlineColoringRules.Add(new InlineColoringRule() { Foreground = "Green", Pattern = "Green" });
			column.InlineColoringRules.Add(new InlineColoringRule() { Foreground = "Black", Pattern = "Black" });
			column.InlineColoringRules.Add(new InlineColoringRule() { Foreground = "White", Pattern = "White" });

			parser = new InlineParser(column, regexBuilder);
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
			column.InlineColoringRules.Add(new InlineColoringRule() { Foreground = "Red", Pattern = "Red" });
			column.InlineColoringRules.Add(new InlineColoringRule() { Foreground = "Green", Pattern = "Green" });
			column.InlineColoringRules.Add(new InlineColoringRule() { Foreground = "Blue", Pattern = "Blue" });
			column.InlineColoringRules.Add(new InlineColoringRule() { Foreground = "White", Pattern = "White" });
			column.InlineColoringRules.Add(new InlineColoringRule() { Foreground = "Black", Pattern = "Black" });

			parser = new InlineParser(column, regexBuilder);
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
			column.InlineColoringRules.Add(new InlineColoringRule() { Foreground = "Red", Pattern = "Red" });
			column.InlineColoringRules.Add(new InlineColoringRule() { Foreground = "Green", Pattern = "Green" });
			column.InlineColoringRules.Add(new InlineColoringRule() { Foreground = "Blue", Pattern = "Blue" });
			column.InlineColoringRules.Add(new InlineColoringRule() { Foreground = "White", Pattern = "White" });
			column.InlineColoringRules.Add(new InlineColoringRule() { Foreground = "Black", Pattern = "Black" });

			parser = new InlineParser(column, regexBuilder);
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


	}
}
