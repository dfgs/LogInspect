using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using LogInspect.Models;
using LogInspect.ModelsTest.Mocks;
using LogInspect.Modules.Readers;
using LogLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogInspect.ModelsTest
{
	[TestClass]
	public class LineReaderUnitTest
	{


		[TestMethod]
		public void ShouldHaveCorrectConstructorParameters()
		{
			Assert.ThrowsException<ArgumentNullException>(() => { new LineReader(NullLogger.Instance, null, Encoding.Default,Utils.EmptyStringMatcher); });
			Assert.ThrowsException<ArgumentNullException>(() => { new LineReader(NullLogger.Instance, new MemoryStream(), null, Utils.EmptyStringMatcher); });
			Assert.ThrowsException<ArgumentNullException>(() => { new LineReader(NullLogger.Instance, new MemoryStream(), Encoding.Default, null); });
		}

		[TestMethod]
		public void ShouldReadCompleteLines()
		{
			MemoryStream stream;
			LineReader Reader;
			string[] items = new string[] { "item1", "item2", "item3", "", "item4", "" };
			Line line;
			int index=0;

			using (stream = new MemoryStream(Encoding.Default.GetBytes(String.Join("\r\n", items) + "\r\n")))
			{
				Reader = new LineReader(NullLogger.Instance, stream, Encoding.Default, Utils.EmptyStringMatcher);

				foreach (string item in items)
				{
					Assert.AreEqual(true, Reader.CanRead);
					line = Reader.Read();
					Assert.AreEqual(item,line.Value );
					Assert.AreEqual(index, line.Index);
					index++;
				}
				Assert.AreEqual(false, Reader.CanRead);
				Assert.ThrowsException<EndOfStreamException>(() => Reader.Read());
			}
		}

		[TestMethod]
		public void ShouldReadIncompleteLine()
		{
			MemoryStream stream;
			LineReader Reader;
			string[] items = new string[] { "item1", "item2", "item3", "", "item4", "item5" };
			Line line;
			int index = 0;

			using (stream = new MemoryStream(Encoding.Default.GetBytes(String.Join("\r\n", items))))
			{
				Reader = new LineReader(NullLogger.Instance, stream, Encoding.Default, Utils.EmptyStringMatcher);

				foreach (string item in items)
				{
					Assert.AreEqual(true, Reader.CanRead);
					line = Reader.Read();
					Assert.AreEqual(item, line.Value);
					Assert.AreEqual(index, line.Index);
					index++;
				}
				Assert.AreEqual(false, Reader.CanRead);
				Assert.ThrowsException<EndOfStreamException>(() => Reader.Read());
			}
		}

		[TestMethod]
		public void ShouldDiscardLines()
		{
			MemoryStream stream;
			LineReader Reader;
			IStringMatcher matcher;
			string[] items = new string[] { "item1", "discard", "item2", "discard", "item3", "discard", "item4", "discard", "item5" };
			Line line;

			matcher = new StringMatcher() ;
			matcher.Add( "discard");
			using (stream = new MemoryStream(Encoding.Default.GetBytes(String.Join("\r\n", items))))
			{
				Reader = new LineReader(NullLogger.Instance, stream, Encoding.Default, matcher);

				for (int t = 0; t < 5; t++)
				{
					Assert.AreEqual(true, Reader.CanRead);
					line = Reader.Read();
					Assert.AreEqual(items[t * 2], line.Value);
					Assert.AreEqual(t*2, line.Index);
				}
				Assert.AreEqual(false, Reader.CanRead);
				Assert.ThrowsException<EndOfStreamException>(() => Reader.Read());
			}
		}


	}
}
