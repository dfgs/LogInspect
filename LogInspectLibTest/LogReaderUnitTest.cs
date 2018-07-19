using System;
using System.IO;
using System.Text;
using LogInspectLib;
using LogInspectLib.Readers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogInspectLibTest
{
	[TestClass]
	public class LogReaderUnitTest
	{
		private static string line1 = "12/12/1980 08:53:22 Line 1#";
		private static string line2 = "12/12/1980 08:53:22 Line 2#";
		private static string line3 = " with comment 1";
		private static string line4 = " with comment 2";
		private static string line5 = " with comment 3";
		private static string line6 = "12/12/1980 08:53:22 +";
		private static string line7 = "Line 3#";
		private static string logString1 = line1 + "\r\n" + line2 + "\r\n" + line3 + "\r\n" + line4 + "\r\n" + line5 + "\r\n" + line6 + "\r\n" + line7;

		private static string log1 = "1|2|3|4";
		private static string log2 = "1234";
		private static string logString2 = log1 + "\r\n" + log2 ;

		[TestMethod]
		public void ShouldReadLine()
		{
			string line;
			MemoryStream stream;
			LogReader reader;
			FormatHandler formatHandler;


			formatHandler = new FormatHandler();
			stream = new MemoryStream(Encoding.Default.GetBytes(logString1));
			reader = new LogReader(stream, Encoding.Default, formatHandler);

			line = reader.ReadLine();
			Assert.AreEqual(line1, line.ToString());
			line = reader.ReadLine();
			Assert.AreEqual(line2, line.ToString());
			line = reader.ReadLine();
			Assert.AreEqual(line3, line.ToString());
			line = reader.ReadLine();
			Assert.AreEqual(line4, line.ToString());
			line = reader.ReadLine();
			Assert.AreEqual(line5, line.ToString());
			line = reader.ReadLine();
			Assert.AreEqual(line6, line.ToString());
			line = reader.ReadLine();
			Assert.AreEqual(line7, line.ToString());
			Assert.ThrowsException<EndOfStreamException>(reader.ReadLine);

		}

		[TestMethod]
		public void ShouldReadLogWithoutPatterns()
		{
			Log log;
			MemoryStream stream;
			LogReader reader;
			FormatHandler formatHandler;


			formatHandler = new FormatHandler();
			stream = new MemoryStream(Encoding.Default.GetBytes(logString1));
			reader = new LogReader(stream, Encoding.Default, formatHandler);

			log = reader.ReadLog();
			Assert.AreEqual(line1, log.ToString());
			log = reader.ReadLog();
			Assert.AreEqual(line2, log.ToString());
			log = reader.ReadLog();
			Assert.AreEqual(line3, log.ToString());
			log = reader.ReadLog();
			Assert.AreEqual(line4, log.ToString());
			log = reader.ReadLog();
			Assert.AreEqual(line5, log.ToString());
			log = reader.ReadLog();
			Assert.AreEqual(line6, log.ToString());
			log = reader.ReadLog();
			Assert.AreEqual(line7, log.ToString());
			Assert.ThrowsException<EndOfStreamException>(reader.ReadLine);

		}


		[TestMethod]
		public void ShouldReadLogWithAppendToPreviousPatterns()
		{
			Log log;
			MemoryStream stream;
			LogReader reader;
			FormatHandler formatHandler;


			formatHandler = new FormatHandler();
			formatHandler.AppendToPreviousPatterns.Add("^ ");
			stream = new MemoryStream(Encoding.Default.GetBytes(logString1));
			reader = new LogReader(stream, Encoding.Default, formatHandler);

			log = reader.ReadLog();
			Assert.AreEqual(line1, log.ToString());
			log = reader.ReadLog();
			Assert.AreEqual(line2+line3+line4+line5, log.ToString());
			log = reader.ReadLog();
			Assert.AreEqual(line6, log.ToString());
			log = reader.ReadLog();
			Assert.AreEqual(line7, log.ToString());
			Assert.ThrowsException<EndOfStreamException>(reader.ReadLine);

		}

		[TestMethod]
		public void ShouldReadLogWithAppendToNextPatterns()
		{
			Log log;
			MemoryStream stream;
			LogReader reader;
			FormatHandler formatHandler;


			formatHandler = new FormatHandler();
			formatHandler.AppendToNextPatterns.Add(@"\+$");
			stream = new MemoryStream(Encoding.Default.GetBytes(logString1));
			reader = new LogReader(stream, Encoding.Default, formatHandler);

			log = reader.ReadLog();
			Assert.AreEqual(line1, log.ToString());
			log = reader.ReadLog();
			Assert.AreEqual(line2, log.ToString());
			log = reader.ReadLog();
			Assert.AreEqual(line3, log.ToString());
			log = reader.ReadLog();
			Assert.AreEqual(line4, log.ToString());
			log = reader.ReadLog();
			Assert.AreEqual(line5, log.ToString());
			log = reader.ReadLog();
			Assert.AreEqual(line6+line7, log.ToString());
			Assert.ThrowsException<EndOfStreamException>(reader.ReadLine);

		}

		[TestMethod]
		public void ShouldReadEvent()
		{
			Event ev;
			MemoryStream stream;
			LogReader reader;
			FormatHandler formatHandler;
			Rule rule;


			formatHandler = new FormatHandler();
			rule = new Rule() { Name="UnitTest" };
			rule.Tokens.Add(new Token() { Name = "C1", Pattern = @"\d" });
			rule.Tokens.Add(new Token() { Name = null, Pattern = @"\|" });
			rule.Tokens.Add(new Token() { Name = "C2", Pattern = @"\d" });
			rule.Tokens.Add(new Token() { Name = null, Pattern = @"\|" });
			rule.Tokens.Add(new Token() { Name = "C3", Pattern = @"\d" });
			rule.Tokens.Add(new Token() { Name = null, Pattern = @"\|" });
			rule.Tokens.Add(new Token() { Name = "C4", Pattern = @"\d$" });
			formatHandler.Rules.Add(rule);

			stream = new MemoryStream(Encoding.Default.GetBytes(logString2));
			reader = new LogReader(stream, Encoding.Default, formatHandler);

			ev= reader.ReadEvent();
			Assert.IsNotNull(ev.Log);
			Assert.IsNotNull(ev.Rule);
			Assert.AreEqual(4, ev.Properties.Count);
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
