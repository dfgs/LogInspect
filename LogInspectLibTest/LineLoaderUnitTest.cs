using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using LogInspectLib;
using LogInspectLib.Loaders;
using LogInspectLibTest.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogInspectLibTest
{
	[TestClass]
	public class LineLoaderUnitTest
	{


		[TestMethod]
		public void ShouldHaveCorrectConstructorParameters()
		{
			Assert.ThrowsException<ArgumentNullException>(() => { new LineLoader(null, Encoding.Default,Utils.EmptyStringMatcher); });
			Assert.ThrowsException<ArgumentNullException>(() => { new LineLoader(new MemoryStream(), null, Utils.EmptyStringMatcher); });
			Assert.ThrowsException<ArgumentNullException>(() => { new LineLoader(new MemoryStream(), Encoding.Default, null); });
		}

		[TestMethod]
		public void ShouldReadCompleteLines()
		{
			MemoryStream stream;
			LineLoader loader;
			string[] items = new string[] { "item1", "item2", "item3", "", "item4", "" };

			stream = new MemoryStream(Encoding.Default.GetBytes(String.Join("\r\n", items)+"\r\n"));
			loader = new LineLoader(stream,Encoding.Default,Utils.EmptyStringMatcher);

			foreach (string item in items)
			{
				loader.Load();
				Assert.AreEqual(item, loader[loader.Count-1].Value);
			}
			Assert.AreEqual(6,loader.Count);
			Assert.ThrowsException<EndOfStreamException>(()=>loader.Load());
		}

		[TestMethod]
		public void ShouldReadIncompleteLine()
		{
			MemoryStream stream;
			LineLoader loader;
			string[] items = new string[] { "item1", "item2", "item3", "", "item4", "item5" };

			stream = new MemoryStream(Encoding.Default.GetBytes(String.Join("\r\n", items) ));
			loader = new LineLoader(stream, Encoding.Default,Utils.EmptyStringMatcher);

			foreach (string item in items)
			{
				loader.Load();
				Assert.AreEqual(item, loader[loader.Count - 1].Value);
			}
			Assert.AreEqual(6, loader.Count);
			Assert.ThrowsException<EndOfStreamException>(() => loader.Load());
		}

		[TestMethod]
		public void ShouldDiscardLines()
		{
			MemoryStream stream;
			LineLoader loader;
			IStringMatcher matcher;
			string[] items = new string[] { "item1", "item2", "item3",  "item4", "item5" };

			matcher = Utils.EmptyStringMatcher;
			matcher.Add( "item1");
			stream = new MemoryStream(Encoding.Default.GetBytes(String.Join("\r\n", items)));
			loader = new LineLoader(stream, Encoding.Default, matcher);

			for(int t= 0;t<4;t++)
			{
				loader.Load();
			}
			Assert.ThrowsException<EndOfStreamException>(() => loader.Load());
			for (int t = 0; t < 4; t++)
			{
				Assert.AreEqual(items[t+1], loader[t].Value);
			}

		}


	}
}
