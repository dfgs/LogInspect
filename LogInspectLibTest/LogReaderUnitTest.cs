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
		public void ShouldHaveCorrectConstructorParameters()
		{
			Assert.ThrowsException<ArgumentNullException>(() => { new LogReader(null, Encoding.Default, 1,new FormatHandler()); });
			Assert.ThrowsException<ArgumentNullException>(() => { new LogReader(new MemoryStream(), null, 1, new FormatHandler()); });
			Assert.ThrowsException<ArgumentException>(() => { new LogReader(new MemoryStream(), Encoding.Default, 0, new FormatHandler()); });
			Assert.ThrowsException<ArgumentException>(() => { new LogReader(new MemoryStream(), Encoding.Default, -1, new FormatHandler()); });
			Assert.ThrowsException<ArgumentException>(() => { new LogReader(new MemoryStream(), Encoding.Default, 0, null); });
		}


	

		[TestMethod]
		public void ShouldReadWithoutPatterns()
		{
			MemoryStream stream;
			LogReader reader;
			FormatHandler formatHandler;


			formatHandler = new FormatHandler();
			stream = new MemoryStream(Encoding.Default.GetBytes(logString1));
			reader = new LogReader(stream, Encoding.Default,10, formatHandler);

			
			Assert.AreEqual(line1, reader.Read().ToSingleLine());
			Assert.AreEqual(line2, reader.Read().ToSingleLine());
			Assert.AreEqual(line3, reader.Read().ToSingleLine());
			Assert.AreEqual(line4, reader.Read().ToSingleLine());
			Assert.AreEqual(line5, reader.Read().ToSingleLine());
			Assert.AreEqual(line6, reader.Read().ToSingleLine());
			Assert.AreEqual(line7, reader.Read().ToSingleLine());
			Assert.IsTrue(reader.EndOfStream);
			Assert.ThrowsException<EndOfStreamException>(()=> { reader.Read(); });

		}


		[TestMethod]
		public void ShouldReadLogWithAppendToPreviousPatterns()
		{
			MemoryStream stream;
			LogReader reader;
			FormatHandler formatHandler;


			formatHandler = new FormatHandler();
			formatHandler.AppendToPreviousPatterns.Add("^ ");
			stream = new MemoryStream(Encoding.Default.GetBytes(logString1));
			reader = new LogReader(stream, Encoding.Default,10, formatHandler);

			Assert.AreEqual(line1, reader.Read().ToSingleLine());
			Assert.AreEqual(line2+line3+line4+line5, reader.Read().ToSingleLine());
			Assert.AreEqual(line6, reader.Read().ToSingleLine());
			Assert.AreEqual(line7, reader.Read().ToSingleLine());
			Assert.IsTrue(reader.EndOfStream);
			Assert.ThrowsException<EndOfStreamException>(() => { reader.Read(); });

		}

		[TestMethod]
		public void ShouldReadLogWithAppendToNextPatterns()
		{
			MemoryStream stream;
			LogReader reader;
			FormatHandler formatHandler;

			formatHandler = new FormatHandler();
			formatHandler.AppendToNextPatterns.Add(@"\+$");
			stream = new MemoryStream(Encoding.Default.GetBytes(logString1));
			reader = new LogReader(stream, Encoding.Default,10, formatHandler);

			Assert.AreEqual(line1, reader.Read().ToSingleLine());
			Assert.AreEqual(line2, reader.Read().ToSingleLine());
			Assert.AreEqual(line3, reader.Read().ToSingleLine());
			Assert.AreEqual(line4, reader.Read().ToSingleLine());
			Assert.AreEqual(line5, reader.Read().ToSingleLine());
			Assert.AreEqual(line6+line7, reader.Read().ToSingleLine());
			Assert.IsTrue(reader.EndOfStream);
			Assert.ThrowsException<EndOfStreamException>(() => { reader.Read(); });

		}

		[TestMethod]
		public void ShouldSeekWithoutLoad()
		{
			MemoryStream stream;
			LogReader reader;
			FormatHandler formatHandler;

			formatHandler = new FormatHandler();
			stream = new MemoryStream(Encoding.Default.GetBytes(logString1));
			reader = new LogReader(stream, Encoding.Default, 4096, formatHandler);

			Assert.AreEqual(line1, reader.Read().ToSingleLine());
			Assert.AreEqual(line2, reader.Read().ToSingleLine());
			Assert.AreEqual(line3, reader.Read().ToSingleLine());
			reader.Seek(Encoding.Default.GetByteCount(line1 + "\r\n"));
			Assert.AreEqual(line2, reader.Read().ToSingleLine());
			Assert.AreEqual(line3, reader.Read().ToSingleLine());
			Assert.AreEqual(line4, reader.Read().ToSingleLine());
			Assert.AreEqual(line5, reader.Read().ToSingleLine());
			Assert.AreEqual(line6, reader.Read().ToSingleLine());
			Assert.AreEqual(line7, reader.Read().ToSingleLine());
			Assert.IsTrue(reader.EndOfStream);
			Assert.ThrowsException<EndOfStreamException>(() => { reader.Read(); });

		}

		[TestMethod]
		public void ShouldSeekWithLoad()
		{
			MemoryStream stream;
			LogReader reader;
			FormatHandler formatHandler;

			formatHandler = new FormatHandler();
			stream = new MemoryStream(Encoding.Default.GetBytes(logString1));
			reader = new LogReader(stream, Encoding.Default, 5, formatHandler);

			Assert.AreEqual(line1, reader.Read().ToSingleLine());
			Assert.AreEqual(line2, reader.Read().ToSingleLine());
			Assert.AreEqual(line3, reader.Read().ToSingleLine());
			reader.Seek(Encoding.Default.GetByteCount(line1 + "\r\n"));
			Assert.AreEqual(line2, reader.Read().ToSingleLine());
			Assert.AreEqual(line3, reader.Read().ToSingleLine());
			Assert.AreEqual(line4, reader.Read().ToSingleLine());
			Assert.AreEqual(line5, reader.Read().ToSingleLine());
			Assert.AreEqual(line6, reader.Read().ToSingleLine());
			Assert.AreEqual(line7, reader.Read().ToSingleLine());
			Assert.IsTrue(reader.EndOfStream);
			Assert.ThrowsException<EndOfStreamException>(() => { reader.Read(); });


		}


	}
}
