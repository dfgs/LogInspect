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
			string[] items = new string[] { "item1", "item2", "item3", "", "item4", "" };

			stream = new MemoryStream(Encoding.Default.GetBytes(String.Join("\r\n", items)+"\r\n"));
			reader = new LineReader(stream, Encoding.Default, 5);

			foreach (string item in items)
			{
				Assert.AreEqual(item, reader.Read().Value);
			}
			Assert.IsTrue(reader.EndOfStream);
			Assert.ThrowsException<EndOfStreamException>(()=> { reader.Read(); });

		}
		[TestMethod]
		public void ShouldSeekWithoutLoad()
		{
			MemoryStream stream;
			LineReader reader;
			string[] items = new string[] { "item1", "item2", "item3", "", "item4", "" };

			stream = new MemoryStream(Encoding.Default.GetBytes(String.Join("\r\n", items) + "\r\n"));
			reader = new LineReader(stream, Encoding.Default, 4096);

			for(int t=0;t<3;t++)
			{
				Assert.AreEqual(items[t], reader.Read().Value);
			}
			reader.Seek(Encoding.Default.GetByteCount(items[0]+"\r\n"));
			for (int t = 1; t < items.Length; t++)
			{
				Assert.AreEqual(items[t], reader.Read().Value);
			}
			Assert.IsTrue(reader.EndOfStream);
			Assert.ThrowsException<EndOfStreamException>(() => { reader.Read(); });

		}

		[TestMethod]
		public void ShouldSeekWithLoad()
		{
			MemoryStream stream;
			LineReader reader;
			string[] items = new string[] { "item1", "item2", "item3", "", "item4", "" };

			stream = new MemoryStream(Encoding.Default.GetBytes(String.Join("\r\n", items) + "\r\n"));
			reader = new LineReader(stream, Encoding.Default, 5);

			for (int t = 0; t < 3; t++)
			{
				Assert.AreEqual(items[t], reader.Read().Value);
			}
			reader.Seek(Encoding.Default.GetByteCount(items[0] + "\r\n"));
			for (int t = 1; t < items.Length; t++)
			{
				Assert.AreEqual(items[t], reader.Read().Value);
			}
			Assert.IsTrue(reader.EndOfStream);
			Assert.ThrowsException<EndOfStreamException>(() => { reader.Read(); });

		}

	}
}
