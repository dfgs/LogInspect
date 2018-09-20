using System;
using System.IO;
using System.Text;
using LogInspectLib;
using LogInspectLib.Loaders;
using LogInspectLib.Parsers;
using LogInspectLibTest.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogInspectLibTest
{
	[TestClass]
	public class EventLoaderUnitTest
	{
		private static IRegexBuilder regexBuilder = new RegexBuilder();

		[TestMethod]
		public void ShouldHaveCorrectConstructorParameters()
		{
			Assert.ThrowsException<ArgumentNullException>(() => { new EventLoader(Utils.EmptyLogLoader,null); });
			Assert.ThrowsException<ArgumentNullException>(() => { new EventLoader(null,new LogParser() ); });
		}


		[TestMethod]
		public void ShouldRead()
		{
			ILogLoader logLoader;
			EventLoader loader;
			ILogParser parser;
			Column[] columns;

			columns = new Column[] { new Column() { Name = "A" }, new Column() { Name = "B" }, new Column() { Name = "C" } };
			parser = new LogParser();
			parser.Add( @"(?<A>\w\d) \| (?<B>\w\d) \| (?<C>\w\d)");

			logLoader = new MockedLogLoader();

			loader = new EventLoader(logLoader, parser);

			for (int t = 0; t < 5; t++) logLoader.Load();
			
			for (int t = 0; t < 5; t++)
			{
				loader.Load();
				Assert.AreEqual($"A{t}", loader[t]["A"]);
				Assert.AreEqual($"B{t}", loader[t]["B"]);
				Assert.AreEqual($"C{t}", loader[t]["C"]);
				Assert.AreEqual(3, loader[t].Properties.Count);

			}
			Assert.AreEqual(5,loader.Count);
			Assert.ThrowsException<EndOfStreamException>(()=> { loader.Load(); });
		}


		/*
		[TestMethod]
		public void ShouldFailIfCannotAppendToNext()
		{
			MockedLineLoader lineLoader;
			EventLoader loader;
			IStringMatcher matcher;

			lineLoader = new MockedLineLoader();
			lineLoader.Load("Item ");
			lineLoader.Load("1");
			lineLoader.Load("Item ");
			lineLoader.Load("2");
			lineLoader.Load("Item ");
			lineLoader.Load("3");
			lineLoader.Load("Item ");

			matcher = new StringMatcher(new RegexBuilder());
			matcher.Add("", " $");

			loader = new EventLoader(lineLoader, Utils.EmptyStringMatcher, matcher);

			for (int t = 0; t < 3; t++) loader.Load();

			Assert.AreEqual("Item 1", loader[0].ToSingleLine());
			Assert.AreEqual("Item 2", loader[1].ToSingleLine());
			Assert.AreEqual("Item 3", loader[2].ToSingleLine());

			// cannot load last item because missing EOL
			Assert.AreEqual(3, loader.Count);
			Assert.ThrowsException<EndOfStreamException>(() => loader.Load());

			// can load if we add missing line
			lineLoader.Load("4");
			loader.Load();

			Assert.AreEqual("Item 4", loader[3].ToSingleLine());
			Assert.AreEqual(4, loader.Count);
			Assert.ThrowsException<EndOfStreamException>(() => loader.Load());

		}
		//*/




	}
}
