using System;
using System.Collections;
using LogInspectLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogInspectLibTest
{
	[TestClass]
	public class LogParserUnitTest
	{
		private static string log1 = "1|2|3|4";

		[TestMethod]
		public void ShouldRead()
		{
			LogParser parser;
			Rule rule;
			Log log;
			Event? ev;

			rule = new Rule() { Name = "UnitTest" };
			rule.Tokens.Add(new Token() { Name = "C1", Pattern = @"\d" });
			rule.Tokens.Add(new Token() { Name = null, Pattern = @"\|" });
			rule.Tokens.Add(new Token() { Name = "C2", Pattern = @"\d" });
			rule.Tokens.Add(new Token() { Name = null, Pattern = @"\|" });
			rule.Tokens.Add(new Token() { Name = "C3", Pattern = @"\d" });
			rule.Tokens.Add(new Token() { Name = null, Pattern = @"\|" });
			rule.Tokens.Add(new Token() { Name = "C4", Pattern = @"\d$" });
			parser = new LogParser(rule,new SeverityMapping[0]);

			log = new Log(new Line(0, log1));
			ev=parser.Parse(log);

			Assert.IsNotNull(ev.Value.Log);
			Assert.IsNotNull(ev.Value.Rule);
			Assert.AreEqual(4, ev.Value.Properties.Length);
			Assert.AreEqual("C1", ev.Value.Properties[0].Name);
			Assert.AreEqual("1", ev.Value.Properties[0].Value);
			Assert.AreEqual("C2", ev.Value.Properties[1].Name);
			Assert.AreEqual("2", ev.Value.Properties[1].Value);
			Assert.AreEqual("C3", ev.Value.Properties[2].Name);
			Assert.AreEqual("3", ev.Value.Properties[2].Value);
			Assert.AreEqual("C4", ev.Value.Properties[3].Name);
			Assert.AreEqual("4", ev.Value.Properties[3].Value);

		}

	}
}
