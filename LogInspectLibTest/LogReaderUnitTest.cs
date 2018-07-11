using System;
using System.IO;
using System.Text;
using LogInspectLib;
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
		private static string logString = line1 + "\r\n" + line2 + "\r\n" + line3 + "\r\n" + line4 + "\r\n" + line5 + "\r\n" + line6 + "\r\n" + line7;

		[TestMethod]
		public void ShouldParseFileWithoutPatterns()
		{
			Log log;
			MemoryStream stream;
			LogReader reader;
			FormatHandler formatHandler;


			formatHandler = new FormatHandler();
			stream = new MemoryStream(Encoding.Default.GetBytes(logString));
			reader = new LogReader(stream,formatHandler);

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
			log = reader.ReadLog();
			Assert.AreEqual(null, log);

		}


		[TestMethod]
		public void ShouldParseFileWithAppendToPreviousPatterns()
		{
			Log log;
			MemoryStream stream;
			LogReader reader;
			FormatHandler formatHandler;


			formatHandler = new FormatHandler();
			formatHandler.AppendToPreviousPatterns.Add("^ ");
			stream = new MemoryStream(Encoding.Default.GetBytes(logString));
			reader = new LogReader(stream, formatHandler);

			log = reader.ReadLog();
			Assert.AreEqual(line1, log.ToString());
			log = reader.ReadLog();
			Assert.AreEqual(line2+line3+line4+line5, log.ToString());
			log = reader.ReadLog();
			Assert.AreEqual(line6, log.ToString());
			log = reader.ReadLog();
			Assert.AreEqual(line7, log.ToString());
			log = reader.ReadLog();
			Assert.AreEqual(null, log);

		}

		[TestMethod]
		public void ShouldParseFileWithAppendToNextPatterns()
		{
			Log log;
			MemoryStream stream;
			LogReader reader;
			FormatHandler formatHandler;


			formatHandler = new FormatHandler();
			formatHandler.AppendToNextPatterns.Add(@"\+$");
			stream = new MemoryStream(Encoding.Default.GetBytes(logString));
			reader = new LogReader(stream, formatHandler);

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
			log = reader.ReadLog();
			Assert.AreEqual(null, log);

		}


	}
}
