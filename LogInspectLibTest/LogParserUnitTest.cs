using System;
using System.Collections;
using System.Linq;
using LogInspectLib;
using LogInspectLib.Parsers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogInspectLibTest
{
	[TestClass]
	public class LogParserUnitTest
	{
		private static IRegexBuilder regexBuilder = new RegexBuilder();

		private static string log1 = "1|2|3|4";

		[TestMethod]
		public void ShouldHaveCorrectConstructorParameters()
		{
			Assert.ThrowsException<ArgumentNullException>(() => { new LogParser(null); });
		}


		[TestMethod]
		public void ShouldParse()
		{
			LogParser parser;
			Log log;
			Event ev;
			Column[] columns;

			columns = new Column[] { new Column() { Name = "C1" }, new Column() { Name = "C2" }, new Column() { Name = "C3" }, new Column() { Name = "C4" } };

			parser = new LogParser(columns.Select(item=>item.Name));
			parser.Add(@"(?<C1>\d)(\|)(?<C2>\d)(\|)(?<C3>\d)(\|)(?<C4>\d$)",false);

			log = new Log();
			log.Lines.Add(new Line() { Value = log1 });
			ev=parser.Parse(log);


			Assert.IsNotNull(ev);
			Assert.AreEqual(4, ev.Properties.Count);
			Assert.AreEqual("1", ev["C1"]);
			Assert.AreEqual("2", ev["C2"]);
			Assert.AreEqual("3", ev["C3"]);
			Assert.AreEqual("4", ev["C4"]);

		}

		[TestMethod]
		public void ShouldParseWhenColumnIsMissingInRegexPattern()
		{
			LogParser parser;
			Log log;
			Event ev;
			Column[] columns;

			columns = new Column[] { new Column() { Name = "C1" }, new Column() { Name = "C2" }, new Column() { Name = "C3" }, new Column() { Name = "C4" } };

			parser = new LogParser(columns.Select(item => item.Name));
			parser.Add(@"(?<C1>\d)(\|)(?<C2>\d)(\|)(?<C3>\d)(\|)(?<C5>\d$)", false); // changed C4 to C5 to simulate missing column

			log = new Log();
			log.Lines.Add(new Line() { Value = log1 });
			ev = parser.Parse(log);


			Assert.IsNotNull(ev);
			Assert.AreEqual(4, ev.Properties.Count);
			Assert.AreEqual("1", ev["C1"]);
			Assert.AreEqual("2", ev["C2"]);
			Assert.AreEqual("3", ev["C3"]);
			Assert.AreEqual("", ev["C4"]);
		}


		[TestMethod]
		public void ShouldDiscard()
		{
			LogParser parser;
			Log log;
			Event ev;
			Column[] columns;

			columns = new Column[] { new Column() { Name = "C1" }, new Column() { Name = "C2" }, new Column() { Name = "C3" }, new Column() { Name = "C4" } };
	
			parser = new LogParser(columns.Select(item => item.Name));
			parser.Add(@"\d\|\d\|\d\|\d$", true);

			log = new Log();
			log.Lines.Add(new Line() { Value = log1 });
			ev = parser.Parse(log);


			Assert.IsNull(ev);
			

		}


	}
}
