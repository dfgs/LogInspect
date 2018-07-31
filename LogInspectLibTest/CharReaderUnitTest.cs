using System;
using System.IO;
using System.Text;
using LogInspectLib;
using LogInspectLib.Readers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogInspectLibTest
{
	[TestClass]
	public class CharReaderUnitTest
	{

		[TestMethod]
		public void ShouldHaveCorrectConstructorParameters()
		{
			Assert.ThrowsException<ArgumentNullException>(() => { new CharReader(null, Encoding.Default, 1); });
			Assert.ThrowsException<ArgumentNullException>(() => { new CharReader(new MemoryStream(), null, 1); });
			Assert.ThrowsException<ArgumentException>(() => { new CharReader(new MemoryStream(), Encoding.Default, 0); });
			Assert.ThrowsException<ArgumentException>(() => { new CharReader(new MemoryStream(), Encoding.Default, -1); });
		}

		[TestMethod]
		public void ShouldRead()
		{
			MemoryStream stream;
			CharReader reader;
			string items = "0123456789";

			stream = new MemoryStream(Encoding.Default.GetBytes(items));
			reader = new CharReader(stream, Encoding.Default, 5);

			foreach (char item in items)
			{ 
				Assert.AreEqual(item, reader.Read());
			}
			Assert.IsTrue(reader.EndOfStream);
			Assert.ThrowsException<EndOfStreamException>(()=> { reader.Read(); });

		}

		[TestMethod]
		public void ShouldSeekWithoutLoad()
		{
			MemoryStream stream;
			CharReader reader;
			string items = "0123456789";

			stream = new MemoryStream(Encoding.Default.GetBytes(items));
			reader = new CharReader(stream, Encoding.Default, 5);


			Assert.AreEqual('0', reader.Read());
			Assert.AreEqual('1', reader.Read());
			Assert.AreEqual('2', reader.Read());
			reader.Seek(1);
			Assert.AreEqual('1', reader.Read());
			Assert.AreEqual('2', reader.Read());
			Assert.AreEqual('3', reader.Read());
			Assert.AreEqual('4', reader.Read());
			Assert.AreEqual('5', reader.Read());
			Assert.AreEqual('6', reader.Read());
			Assert.AreEqual('7', reader.Read());
			Assert.AreEqual('8', reader.Read());
			Assert.AreEqual('9', reader.Read());
			Assert.IsTrue(reader.EndOfStream);
			Assert.ThrowsException<EndOfStreamException>(() => { reader.Read(); });
		}

		[TestMethod]
		public void ShouldSeekWithLoad()
		{
			MemoryStream stream;
			CharReader reader;
			string items = "0123456789";

			stream = new MemoryStream(Encoding.Default.GetBytes(items));
			reader = new CharReader(stream, Encoding.Default, 5);


			Assert.AreEqual('0', reader.Read());
			Assert.AreEqual('1', reader.Read());
			Assert.AreEqual('2', reader.Read());
			Assert.AreEqual('3', reader.Read());
			Assert.AreEqual('4', reader.Read());
			Assert.AreEqual('5', reader.Read());
			Assert.AreEqual('6', reader.Read());
			Assert.AreEqual('7', reader.Read());
			Assert.AreEqual('8', reader.Read());
			reader.Seek(1);
			Assert.AreEqual('1', reader.Read());
			Assert.AreEqual('2', reader.Read());
			Assert.AreEqual('3', reader.Read());
			Assert.AreEqual('4', reader.Read());
			Assert.AreEqual('5', reader.Read());
			Assert.AreEqual('6', reader.Read());
			Assert.AreEqual('7', reader.Read());
			Assert.AreEqual('8', reader.Read());
			Assert.AreEqual('9', reader.Read());
			Assert.IsTrue(reader.EndOfStream);
			Assert.ThrowsException<EndOfStreamException>(() => { reader.Read(); });
		}



	}
}
