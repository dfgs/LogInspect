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
			string[] items = new string[] {"item1", "item2", "item3","", "item4","" };
			MemoryStream stream;
			LogReader reader;
			FormatHandler formatHandler;

			formatHandler = new FormatHandler();
			stream = new MemoryStream(Encoding.Default.GetBytes(String.Join("\r\n", items)+ "\r\n"));
			reader = new LogReader(stream, Encoding.Default,10, formatHandler);

			foreach (string item in items)
			{
				Assert.AreEqual(item, reader.Read().ToSingleLine());
			}
			Assert.IsTrue(reader.EndOfStream);
			Assert.ThrowsException<EndOfStreamException>(()=> { reader.Read(); });
		}

		[TestMethod]
		public void ShouldDiscardBasedOnPatterns1()
		{
			string[] items = new string[] { "item1", "item2", "item3", "discard", "item4", "discard" };	// ending with discard
			MemoryStream stream;
			LogReader reader;
			FormatHandler formatHandler;

			formatHandler = new FormatHandler();
			formatHandler.DiscardLinePatterns.Add("discard");
			stream = new MemoryStream(Encoding.Default.GetBytes(String.Join("\r\n", items) + "\r\n"));
			reader = new LogReader(stream, Encoding.Default, 10, formatHandler);

			Assert.AreEqual(items[0], reader.Read().ToSingleLine());
			Assert.AreEqual(items[1], reader.Read().ToSingleLine());
			Assert.AreEqual(items[2], reader.Read().ToSingleLine());
			Assert.AreEqual(items[4], reader.Read().ToSingleLine());

			Assert.IsTrue(reader.EndOfStream);
			Assert.ThrowsException<EndOfStreamException>(() => { reader.Read(); });
		}
		[TestMethod]
		public void ShouldDiscardBasedOnPatterns2()
		{
			string[] items = new string[] {"discard", "item1", "item2", "item3", "discard", "item4" }; // starting with discard
			MemoryStream stream;
			LogReader reader;
			FormatHandler formatHandler;

			formatHandler = new FormatHandler();
			formatHandler.DiscardLinePatterns.Add("discard");
			stream = new MemoryStream(Encoding.Default.GetBytes(String.Join("\r\n", items) + "\r\n"));
			reader = new LogReader(stream, Encoding.Default, 10, formatHandler);

			Assert.AreEqual(items[1], reader.Read().ToSingleLine());
			Assert.AreEqual(items[2], reader.Read().ToSingleLine());
			Assert.AreEqual(items[3], reader.Read().ToSingleLine());
			Assert.AreEqual(items[5], reader.Read().ToSingleLine());

			Assert.IsTrue(reader.EndOfStream);
			Assert.ThrowsException<EndOfStreamException>(() => { reader.Read(); });
		}

		[TestMethod]
		public void ShouldReadLogWithAppendToPreviousPatterns()
		{
			string[] items = new string[] { "item1", "item2", " item3", " item4", "item5", "" };
			MemoryStream stream;
			LogReader reader;
			FormatHandler formatHandler;


			formatHandler = new FormatHandler();
			formatHandler.AppendLineToPreviousPatterns.Add("^ ");
			stream = new MemoryStream(Encoding.Default.GetBytes(String.Join("\r\n", items) + "\r\n"));
			reader = new LogReader(stream, Encoding.Default,10, formatHandler);

			Assert.AreEqual(items[0], reader.Read().ToSingleLine());
			Assert.AreEqual(items[1] + items[2] + items[3], reader.Read().ToSingleLine());
			Assert.AreEqual(items[4], reader.Read().ToSingleLine());
			Assert.AreEqual(items[5], reader.Read().ToSingleLine());

			Assert.IsTrue(reader.EndOfStream);
			Assert.ThrowsException<EndOfStreamException>(() => { reader.Read(); });
		}

		[TestMethod]
		public void ShouldReadLogWithAppendToNextPatterns()
		{
			string[] items = new string[] { "item1", "item2+", "item3+", "item4", "item5", "" };
			MemoryStream stream;
			LogReader reader;
			FormatHandler formatHandler;

			formatHandler = new FormatHandler();
			formatHandler.AppendLineToNextPatterns.Add(@"\+$");
			stream = new MemoryStream(Encoding.Default.GetBytes(String.Join("\r\n", items) + "\r\n"));
			reader = new LogReader(stream, Encoding.Default,10, formatHandler);

			Assert.AreEqual(items[0], reader.Read().ToSingleLine());
			Assert.AreEqual(items[1] + items[2] + items[3], reader.Read().ToSingleLine());
			Assert.AreEqual(items[4], reader.Read().ToSingleLine());
			Assert.AreEqual(items[5], reader.Read().ToSingleLine());

			Assert.IsTrue(reader.EndOfStream);
			Assert.ThrowsException<EndOfStreamException>(() => { reader.Read(); });

		}

		[TestMethod]
		public void ShouldReadLogWithDiscardAndAppendToPreviousPatterns()
		{
			string[] items = new string[] { "item1", "item2", "discard", " item4", "item5", "" };
			MemoryStream stream;
			LogReader reader;
			FormatHandler formatHandler;


			formatHandler = new FormatHandler();
			formatHandler.DiscardLinePatterns.Add("discard");
			formatHandler.AppendLineToPreviousPatterns.Add("^ ");
			stream = new MemoryStream(Encoding.Default.GetBytes(String.Join("\r\n", items) + "\r\n"));
			reader = new LogReader(stream, Encoding.Default, 10, formatHandler);

			Assert.AreEqual(items[0], reader.Read().ToSingleLine());
			Assert.AreEqual(items[1] +  items[3], reader.Read().ToSingleLine());
			Assert.AreEqual(items[4], reader.Read().ToSingleLine());
			Assert.AreEqual(items[5], reader.Read().ToSingleLine());

			Assert.IsTrue(reader.EndOfStream);
			Assert.ThrowsException<EndOfStreamException>(() => { reader.Read(); });
		}

		[TestMethod]
		public void ShouldReadLogWithDiscardAndAppendToNextPatterns()
		{
			string[] items = new string[] { "item1", "item2+", "discard", "item4", "item5", "" };
			MemoryStream stream;
			LogReader reader;
			FormatHandler formatHandler;

			formatHandler = new FormatHandler();
			formatHandler.DiscardLinePatterns.Add("discard");
			formatHandler.AppendLineToNextPatterns.Add(@"\+$");
			stream = new MemoryStream(Encoding.Default.GetBytes(String.Join("\r\n", items) + "\r\n"));
			reader = new LogReader(stream, Encoding.Default, 10, formatHandler);

			Assert.AreEqual(items[0], reader.Read().ToSingleLine());
			Assert.AreEqual(items[1] + items[3], reader.Read().ToSingleLine());
			Assert.AreEqual(items[4], reader.Read().ToSingleLine());
			Assert.AreEqual(items[5], reader.Read().ToSingleLine());

			Assert.IsTrue(reader.EndOfStream);
			Assert.ThrowsException<EndOfStreamException>(() => { reader.Read(); });

		}

		[TestMethod]
		public void ShouldSeekWithoutLoad()
		{
			string[] items = new string[] { "item1", "item2", "item3", "", "item4", "" };
			MemoryStream stream;
			LogReader reader;
			FormatHandler formatHandler;

			formatHandler = new FormatHandler();
			stream = new MemoryStream(Encoding.Default.GetBytes(String.Join("\r\n", items) + "\r\n"));
			reader = new LogReader(stream, Encoding.Default, 4096, formatHandler);

			Assert.AreEqual(items[0], reader.Read().ToSingleLine());
			Assert.AreEqual(items[1], reader.Read().ToSingleLine());
			Assert.AreEqual(items[2], reader.Read().ToSingleLine());
			reader.Seek(Encoding.Default.GetByteCount(items[0]+ "\r\n"));
			Assert.AreEqual(items[1], reader.Read().ToSingleLine());
			Assert.AreEqual(items[2], reader.Read().ToSingleLine());
			Assert.AreEqual(items[3], reader.Read().ToSingleLine());
			Assert.AreEqual(items[4], reader.Read().ToSingleLine());
			Assert.AreEqual(items[5], reader.Read().ToSingleLine());
			Assert.IsTrue(reader.EndOfStream);
			Assert.ThrowsException<EndOfStreamException>(() => { reader.Read(); });

		}

		[TestMethod]
		public void ShouldSeekWithLoad()
		{
			string[] items = new string[] { "item1", "item2", "item3", "", "item4", "" };
			MemoryStream stream;
			LogReader reader;
			FormatHandler formatHandler;

			formatHandler = new FormatHandler();
			stream = new MemoryStream(Encoding.Default.GetBytes(String.Join("\r\n", items) + "\r\n"));
			reader = new LogReader(stream, Encoding.Default, 5, formatHandler);

			Assert.AreEqual(items[0], reader.Read().ToSingleLine());
			Assert.AreEqual(items[1], reader.Read().ToSingleLine());
			Assert.AreEqual(items[2], reader.Read().ToSingleLine());
			reader.Seek(Encoding.Default.GetByteCount(items[0] + "\r\n"));
			Assert.AreEqual(items[1], reader.Read().ToSingleLine());
			Assert.AreEqual(items[2], reader.Read().ToSingleLine());
			Assert.AreEqual(items[3], reader.Read().ToSingleLine());
			Assert.AreEqual(items[4], reader.Read().ToSingleLine());
			Assert.AreEqual(items[5], reader.Read().ToSingleLine());
			Assert.IsTrue(reader.EndOfStream);
			Assert.ThrowsException<EndOfStreamException>(() => { reader.Read(); });

		}


	}
}
