using System;
using System.IO;
using System.Text;
using LogInspectLib;
using LogInspectLib.Readers;
using LogInspectLibTest.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogInspectLibTest
{
	[TestClass]
	public class LogReaderUnitTest
	{
		private static IRegexBuilder regexBuilder = new RegexBuilder();

		[TestMethod]
		public void ShouldHaveCorrectConstructorParameters()
		{
			Assert.ThrowsException<ArgumentNullException>(() => { new LogReader(null,new RegexBuilder(),"", null, null, null); });
			Assert.ThrowsException<ArgumentNullException>(() => { new LogReader(new MockedLineReader() ,null, "", null, null, null); });
		}


		[TestMethod]
		public void ShouldReadWithoutPatterns()
		{
			string[] items = new string[] {"item1", "item2", "item3","", "item4","" };
			MemoryStream stream;
			LogReader reader;

			stream = new MemoryStream(Encoding.Default.GetBytes(String.Join("\r\n", items)+ "\r\n"));
			reader = new LogReader(new LineReader(new CharReader( stream, Encoding.Default,10)),regexBuilder, "", null, null, null);

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

			stream = new MemoryStream(Encoding.Default.GetBytes(String.Join("\r\n", items) + "\r\n"));
			reader = new LogReader(new LineReader(new CharReader(stream, Encoding.Default, 10)), regexBuilder, "", null,null,new string[] {"discard" } );

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

			stream = new MemoryStream(Encoding.Default.GetBytes(String.Join("\r\n", items) + "\r\n"));
			reader = new LogReader(new LineReader(new CharReader(stream, Encoding.Default, 10)), regexBuilder, "", null, null, new string[] { "discard" });

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


			stream = new MemoryStream(Encoding.Default.GetBytes(String.Join("\r\n", items) + "\r\n"));
			reader = new LogReader(new LineReader(new CharReader(stream, Encoding.Default,10)), regexBuilder, "", new string[] { "^ " } ,null,null);

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

			stream = new MemoryStream(Encoding.Default.GetBytes(String.Join("\r\n", items) + "\r\n"));
			reader = new LogReader(new LineReader(new CharReader( stream, Encoding.Default,10)), regexBuilder, "", null, new string[] { @"\+$" },null);

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

			stream = new MemoryStream(Encoding.Default.GetBytes(String.Join("\r\n", items) + "\r\n"));
			reader = new LogReader(new LineReader(new CharReader(stream, Encoding.Default, 10)), regexBuilder, "", new string[] { "^ " },null, new string[] { "discard" });

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

			stream = new MemoryStream(Encoding.Default.GetBytes(String.Join("\r\n", items) + "\r\n"));
			reader = new LogReader(new LineReader(new CharReader(stream, Encoding.Default, 10)), regexBuilder, "", null, new string[] { @"\+$" }, new string[] { "discard" });

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

			stream = new MemoryStream(Encoding.Default.GetBytes(String.Join("\r\n", items) + "\r\n"));
			reader = new LogReader(new LineReader(new CharReader(stream, Encoding.Default, 4096)), regexBuilder, "", null, null, null);

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

			stream = new MemoryStream(Encoding.Default.GetBytes(String.Join("\r\n", items) + "\r\n"));
			reader = new LogReader(new LineReader(new CharReader(stream, Encoding.Default, 5)), regexBuilder, "", null, null, null);

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
