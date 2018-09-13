using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LogInspectLib;
using LogInspectLib.Parsers;
using LogInspectLib.Readers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogInspectLibTest
{
	[TestClass]
	public class EventReaderUnitTest
	{
		private static IRegexBuilder regexBuilder = new RegexBuilder();

		private static string log1 = "1|2|3|4";
		private static string log2 = "1234";
		private static string logString1 = log1 + "\r\n" + log2 ;


		[TestMethod]
		public void ShouldHaveCorrectConstructorParameters()
		{
			Assert.ThrowsException<ArgumentNullException>(() => { new EventReader(null,  Enumerable.Empty<ILogParser>()); });
		}




		[TestMethod]
		public void ShouldRead()
		{
			Event ev;
			MemoryStream stream;
			EventReader reader;
			FormatHandler formatHandler;
			Rule rule;

			formatHandler = new FormatHandler();
			formatHandler.Columns.Add(new Column() { Name = "C1" });
			formatHandler.Columns.Add(new Column() { Name = "C2" });
			formatHandler.Columns.Add(new Column() { Name = "C3" });
			formatHandler.Columns.Add(new Column() { Name = "C4" });//*/

			rule = new Rule() { Name = "UnitTest" };
			rule.Tokens.Add(new Token() { Name = "C1", Pattern = @"\d" });
			rule.Tokens.Add(new Token() { Name = null, Pattern = @"\|" });
			rule.Tokens.Add(new Token() { Name = "C2", Pattern = @"\d" });
			rule.Tokens.Add(new Token() { Name = null, Pattern = @"\|" });
			rule.Tokens.Add(new Token() { Name = "C3", Pattern = @"\d" });
			rule.Tokens.Add(new Token() { Name = null, Pattern = @"\|" });
			rule.Tokens.Add(new Token() { Name = "C4", Pattern = @"\d$" });
			formatHandler.Rules.Add(rule);


			stream = new MemoryStream(Encoding.Default.GetBytes(logString1));
			reader = new EventReader(new LogReader(new LineReader(new CharReader( stream, Encoding.Default,10)),regexBuilder,formatHandler.AppendLineToPreviousPatterns,formatHandler.AppendLineToNextPatterns,formatHandler.DiscardLinePatterns), formatHandler.CreateLogParsers(regexBuilder));

			ev = reader.Read();
			Assert.IsNotNull(ev.Log);
			Assert.IsNotNull(ev.Rule);
			Assert.AreEqual(4, ev.Properties.Length);
			Assert.AreEqual("C1", ev.Properties[0].Name);
			Assert.AreEqual("1", ev.Properties[0].Value);
			Assert.AreEqual("C2", ev.Properties[1].Name);
			Assert.AreEqual("2", ev.Properties[1].Value);
			Assert.AreEqual("C3", ev.Properties[2].Name);
			Assert.AreEqual("3", ev.Properties[2].Value);
			Assert.AreEqual("C4", ev.Properties[3].Name);
			Assert.AreEqual("4", ev.Properties[3].Value);
		}




	}
}
