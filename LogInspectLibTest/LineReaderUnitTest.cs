using System;
using System.IO;
using System.Text;
using LogInspectLib;
using LogInspectLib.Readers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogInspectLibTest
{
	[TestClass]
	public class LineReaderUnitTest
	{
		private static string line1 = "line1";
		private static string line2 = "line2";
		private static string line3 = "line3";
		private static string line4 = "line4";
		private static string line5 = "line5";
		private static string line6 = "line6";
		private static string line = line1 + "\r\n" + line2 + "\r\n" + line3 + "\r\n" + line4 + "\r\n" + line5 + "\r\n" + line6 ;


		[TestMethod]
		public void ShouldHaveCorrectConstructorParameters()
		{
			Assert.ThrowsException<ArgumentNullException>(() => { new LineReader(null, Encoding.Default, 1); });
			Assert.ThrowsException<ArgumentNullException>(() => { new LineReader(new MemoryStream(), null, 1); });
			Assert.ThrowsException<ArgumentException>(() => { new LineReader(new MemoryStream(), Encoding.Default, 0); });
			Assert.ThrowsException<ArgumentException>(() => { new LineReader(new MemoryStream(), Encoding.Default, -1); });
		}

		[TestMethod]
		public void ShouldRead()
		{
			MemoryStream stream;
			LineReader reader;

			stream = new MemoryStream(Encoding.Default.GetBytes(line));
			reader = new LineReader(stream, Encoding.Default, 5);

			Assert.AreEqual(line1, reader.Read().Value);
			Assert.AreEqual(line2, reader.Read().Value);
			Assert.AreEqual(line3, reader.Read().Value);
			Assert.AreEqual(line4, reader.Read().Value);
			Assert.AreEqual(line5, reader.Read().Value);
			Assert.AreEqual(line6, reader.Read().Value);
			Assert.IsTrue(reader.EndOfStream);
			Assert.ThrowsException<EndOfStreamException>(()=> { reader.Read(); });

		}
		[TestMethod]
		public void ShouldSeekWithoutLoad()
		{
			MemoryStream stream;
			LineReader reader;

			stream = new MemoryStream(Encoding.Default.GetBytes(line));
			reader = new LineReader(stream, Encoding.Default, 4096);

			Assert.AreEqual(line1, reader.Read().Value);
			Assert.AreEqual(line2, reader.Read().Value);
			Assert.AreEqual(line3, reader.Read().Value);
			reader.Seek(Encoding.Default.GetByteCount(line1+"\r\n"));
			Assert.AreEqual(line2, reader.Read().Value);
			Assert.AreEqual(line3, reader.Read().Value);
			Assert.AreEqual(line4, reader.Read().Value);
			Assert.AreEqual(line5, reader.Read().Value);
			Assert.AreEqual(line6, reader.Read().Value);
			Assert.IsTrue(reader.EndOfStream);
			Assert.ThrowsException<EndOfStreamException>(() => { reader.Read(); });

		}

		[TestMethod]
		public void ShouldSeekWithLoad()
		{
			MemoryStream stream;
			LineReader reader;

			stream = new MemoryStream(Encoding.Default.GetBytes(line));
			reader = new LineReader(stream, Encoding.Default, 5);

			Assert.AreEqual(line1, reader.Read().Value);
			Assert.AreEqual(line2, reader.Read().Value);
			Assert.AreEqual(line3, reader.Read().Value);
			reader.Seek(Encoding.Default.GetByteCount(line1 + "\r\n"));
			Assert.AreEqual(line2, reader.Read().Value);
			Assert.AreEqual(line3, reader.Read().Value);
			Assert.AreEqual(line4, reader.Read().Value);
			Assert.AreEqual(line5, reader.Read().Value);
			Assert.AreEqual(line6, reader.Read().Value);
			Assert.IsTrue(reader.EndOfStream);
			Assert.ThrowsException<EndOfStreamException>(() => { reader.Read(); });

		}

	}
}
