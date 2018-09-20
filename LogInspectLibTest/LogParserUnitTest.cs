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
		public void ShouldRead()
		{
			LogParser parser;
			Rule rule;
			Log log;
			Event ev;
			Column[] columns;

			columns = new Column[] { new Column() { Name = "C1" }, new Column() { Name = "C2" }, new Column() { Name = "C3" }, new Column() { Name = "C4" } };

	
			rule = new Rule() { Name = "UnitTest" };
			rule.Tokens.Add(new Token() { Name = "C1", Pattern = @"\d" });
			rule.Tokens.Add(new Token() { Name = null, Pattern = @"\|" });
			rule.Tokens.Add(new Token() { Name = "C2", Pattern = @"\d" });
			rule.Tokens.Add(new Token() { Name = null, Pattern = @"\|" });
			rule.Tokens.Add(new Token() { Name = "C3", Pattern = @"\d" });
			rule.Tokens.Add(new Token() { Name = null, Pattern = @"\|" });
			rule.Tokens.Add(new Token() { Name = "C4", Pattern = @"\d$" });

			parser = new LogParser(columns.Select(item=>item.Name));
			parser.Add( rule.GetPattern());

			log = new Log();
			log.Lines.Add(new Line() { Value = log1 });
			ev=parser.Parse(log);

			//Assert.IsNotNull(ev.Log);
			//Assert.IsNotNull(ev.Rule);
			Assert.AreEqual(4, ev.Properties.Count);
			Assert.AreEqual("1", ev["C1"]);
			Assert.AreEqual("2", ev["C2"]);
			Assert.AreEqual("3", ev["C3"]);
			Assert.AreEqual("4", ev["C4"]);

		}

	}
}
