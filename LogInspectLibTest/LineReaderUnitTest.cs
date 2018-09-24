using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using LogInspectLib;
using LogInspectLib.Readers;
using LogInspectLibTest.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogInspectLibTest
{
	[TestClass]
	public class LineReaderUnitTest
	{


		[TestMethod]
		public void ShouldHaveCorrectConstructorParameters()
		{
			Assert.ThrowsException<ArgumentNullException>(() => { new LineReader(null, Encoding.Default,Utils.EmptyStringMatcher); });
			Assert.ThrowsException<ArgumentNullException>(() => { new LineReader(new MemoryStream(), null, Utils.EmptyStringMatcher); });
			Assert.ThrowsException<ArgumentNullException>(() => { new LineReader(new MemoryStream(), Encoding.Default, null); });
		}

		[TestMethod]
		public void ShouldReadCompleteLines()
		{
			MemoryStream stream;
			LineReader Reader;
			string[] items = new string[] { "item1", "item2", "item3", "", "item4", "" };

			stream = new MemoryStream(Encoding.Default.GetBytes(String.Join("\r\n", items)+"\r\n"));
			Reader = new LineReader(stream,Encoding.Default,Utils.EmptyStringMatcher);

			foreach (string item in items)
			{
				Assert.AreEqual(item, Reader.Read().Value);
			}
			Assert.ThrowsException<EndOfStreamException>(()=>Reader.Read());
		}

		[TestMethod]
		public void ShouldReadIncompleteLine()
		{
			MemoryStream stream;
			LineReader Reader;
			string[] items = new string[] { "item1", "item2", "item3", "", "item4", "item5" };

			stream = new MemoryStream(Encoding.Default.GetBytes(String.Join("\r\n", items) ));
			Reader = new LineReader(stream, Encoding.Default,Utils.EmptyStringMatcher);

			foreach (string item in items)
			{
				Assert.AreEqual(item, Reader.Read().Value);
			}
			Assert.ThrowsException<EndOfStreamException>(() => Reader.Read());
		}

		[TestMethod]
		public void ShouldDiscardLines()
		{
			MemoryStream stream;
			LineReader Reader;
			IStringMatcher matcher;
			string[] items = new string[] { "item1", "discard", "item2", "discard", "item3", "discard", "item4", "discard", "item5" };

			matcher = Utils.EmptyStringMatcher;
			matcher.Add( "discard");
			stream = new MemoryStream(Encoding.Default.GetBytes(String.Join("\r\n", items)));
			Reader = new LineReader(stream, Encoding.Default, matcher);

			for (int t = 0; t < 5; t++)
			{
				Assert.AreEqual(items[t*2], Reader.Read().Value);
			}
			Assert.ThrowsException<EndOfStreamException>(() => Reader.Read());

		}


	}
}
