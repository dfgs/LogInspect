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
		public void ShouldHaveCorrectConstructorParameters()
		{
			Assert.ThrowsException<ArgumentNullException>(() => { new InlineParser(null); });
		}

		
		[TestMethod]
		public void ShouldReadOrderedContiguous()
		{
			IRegexBuilder regexBuilder;
			InlineParser parser;
			Column column;
			Inline[] inlines;

			regexBuilder = new RegexBuilder();
			regexBuilder.Add("NS", new Pattern() { Name = "Red", Foreground = "Red" });
			regexBuilder.Add("NS", new Pattern() { Name = "Green", Foreground = "Green" });
			regexBuilder.Add("NS", new Pattern() { Name = "Blue", Foreground = "Blue" });
			regexBuilder.Add("NS", new Pattern() { Name = "White", Foreground = "White" });
			regexBuilder.Add("NS", new Pattern() { Name = "Black", Foreground = "Black" });

			column = new Column() { Name = "C1" };
			column.InlinePatternNames.Add("Red");
			column.InlinePatternNames.Add("Green");
			column.InlinePatternNames.Add("Blue");
			column.InlinePatternNames.Add("White");
			column.InlinePatternNames.Add("Black");

			parser = new InlineParser(regexBuilder);
			parser.Add("NS", "Red");
			parser.Add("NS", "Green");
			parser.Add("NS", "Blue");
			parser.Add("NS", "White");
			parser.Add("NS", "Black");

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
			IRegexBuilder regexBuilder;
			InlineParser parser;
			Column column;
			Inline[] inlines;

			regexBuilder = new RegexBuilder();
			regexBuilder.Add("NS", new Pattern() { Name = "Red", Foreground = "Red" });
			regexBuilder.Add("NS", new Pattern() { Name = "Green", Foreground = "Green" });
			regexBuilder.Add("NS", new Pattern() { Name = "Blue", Foreground = "Blue" });
			regexBuilder.Add("NS", new Pattern() { Name = "White", Foreground = "White" });
			regexBuilder.Add("NS", new Pattern() { Name = "Black", Foreground = "Black" });

			column = new Column() { Name = "C1" };
			column.InlinePatternNames.Add("Blue");
			column.InlinePatternNames.Add("Red");
			column.InlinePatternNames.Add("Green");
			column.InlinePatternNames.Add("White");
			column.InlinePatternNames.Add("Black");

			parser = new InlineParser(regexBuilder);
			parser.Add("NS", "Red");
			parser.Add("NS", "Green");
			parser.Add("NS", "Blue");
			parser.Add("NS", "White");
			parser.Add("NS", "Black");

			inlines = parser.Parse("RedGreenBlueBlackWhite").ToArray();
			Assert.AreEqual(5, inlines.Length);
			Assert.AreEqual("Blue", inlines[2].Value);
			Assert.AreEqual("Red", inlines[0].Value);
			Assert.AreEqual("Green", inlines[1].Value);
			Assert.AreEqual("Black", inlines[3].Value);
			Assert.AreEqual("White", inlines[4].Value);
		}

		[TestMethod]
		public void ShouldReadOrderedNonContiguous()
		{
			IRegexBuilder regexBuilder;
			InlineParser parser;
			Column column;
			Inline[] inlines;

			regexBuilder = new RegexBuilder();
			regexBuilder.Add("NS", new Pattern() { Name = "Red", Foreground = "Red" });
			regexBuilder.Add("NS", new Pattern() { Name = "Green", Foreground = "Green" });
			regexBuilder.Add("NS", new Pattern() { Name = "Blue", Foreground = "Blue" });
			regexBuilder.Add("NS", new Pattern() { Name = "White", Foreground = "White" });
			regexBuilder.Add("NS", new Pattern() { Name = "Black", Foreground = "Black" });

			column = new Column() { Name = "C1" };
			column.InlinePatternNames.Add("Red");
			column.InlinePatternNames.Add("Green");
			column.InlinePatternNames.Add("Blue");
			column.InlinePatternNames.Add("White");
			column.InlinePatternNames.Add("Black");

			parser = new InlineParser(regexBuilder);
			parser.Add("NS", "Red");
			parser.Add("NS", "Green");
			parser.Add("NS", "Blue");
			parser.Add("NS", "White");
			parser.Add("NS", "Black");

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
			IRegexBuilder regexBuilder;
			InlineParser parser;
			Column column;
			Inline[] inlines;

			regexBuilder = new RegexBuilder();
			regexBuilder.Add("NS", new Pattern() { Name = "Red", Foreground = "Red" });
			regexBuilder.Add("NS", new Pattern() { Name = "Green", Foreground = "Green" });
			regexBuilder.Add("NS", new Pattern() { Name = "Blue", Foreground = "Blue" });
			regexBuilder.Add("NS", new Pattern() { Name = "White", Foreground = "White" });
			regexBuilder.Add("NS", new Pattern() { Name = "Black", Foreground = "Black" });

			column = new Column() { Name = "C1" };
			column.InlinePatternNames.Add("Blue");
			column.InlinePatternNames.Add("Red");
			column.InlinePatternNames.Add("Green");
			column.InlinePatternNames.Add("White");
			column.InlinePatternNames.Add("Black");

			parser = new InlineParser(regexBuilder);
			parser.Add("NS", "Red");
			parser.Add("NS", "Green");
			parser.Add("NS", "Blue");
			parser.Add("NS", "White");
			parser.Add("NS", "Black");

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
			IRegexBuilder regexBuilder;
			InlineParser parser;
			Column column;
			Inline[] inlines;

			regexBuilder = new RegexBuilder();
			regexBuilder.Add("NS", new Pattern() { Name = "Red", Foreground = "Red" });
			regexBuilder.Add("NS", new Pattern() { Name = "Green", Foreground = "Green" });
			regexBuilder.Add("NS", new Pattern() { Name = "Blue", Foreground = "Blue" });
			regexBuilder.Add("NS", new Pattern() { Name = "White", Foreground = "White" });
			regexBuilder.Add("NS", new Pattern() { Name = "Black", Foreground = "Black" });

			column = new Column() { Name = "C1" };
			column.InlinePatternNames.Add("Red");
			column.InlinePatternNames.Add("Green");
			column.InlinePatternNames.Add("Blue");
			column.InlinePatternNames.Add("White");
			column.InlinePatternNames.Add("Black");

			parser = new InlineParser(regexBuilder);
			parser.Add("NS", "Red");
			parser.Add("NS", "Green");
			parser.Add("NS", "Blue");
			parser.Add("NS", "White");
			parser.Add("NS", "Black");

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
			IRegexBuilder regexBuilder;
			InlineParser parser;
			Column column;
			Inline[] inlines;

			regexBuilder = new RegexBuilder();
			regexBuilder.Add("NS", new Pattern() { Name = "Red", Foreground = "Red" });
			regexBuilder.Add("NS", new Pattern() { Name = "Green", Foreground = "Green" });
			regexBuilder.Add("NS", new Pattern() { Name = "Blue", Foreground = "Blue" });
			regexBuilder.Add("NS", new Pattern() { Name = "White", Foreground = "White" });
			regexBuilder.Add("NS", new Pattern() { Name = "Black", Foreground = "Black" });

			column = new Column() { Name = "C1" };
			column.InlinePatternNames.Add("Red");
			column.InlinePatternNames.Add("Green");
			column.InlinePatternNames.Add("Blue");
			column.InlinePatternNames.Add("White");
			column.InlinePatternNames.Add("Black");

			parser = new InlineParser(regexBuilder);
			parser.Add("NS", "Red");
			parser.Add("NS", "Green");
			parser.Add("NS", "Blue");
			parser.Add("NS", "White");
			parser.Add("NS", "Black");

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
