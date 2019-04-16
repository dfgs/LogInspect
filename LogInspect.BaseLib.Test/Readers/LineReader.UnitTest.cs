using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

using LogInspect.BaseLib.Readers;
using LogInspect.BaseLib.Test.Mocks;
using LogInspect.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogInspect.BaseLib.Test.Readers
{
	[TestClass]
	public class LineReaderUnitTest
	{


		[TestMethod]
		public void ShouldHaveCorrectConstructorParameters()
		{
			Assert.ThrowsException<ArgumentNullException>(() => { new LineReader(new MockedStringReader(10),null); });
			Assert.ThrowsException<ArgumentNullException>(() => { new LineReader(null,new StringMatcher()); });
		}

		[TestMethod]
		public void ShouldReadLines()
		{
			LineReader reader;
			Line line;

			reader = new LineReader(new MockedStringReader(10),Utils.EmptyStringMatcher);

			for(int t=0;t<10;t++)
			{
				line = reader.Read();
				Assert.AreEqual(t, line.Index);
				Assert.AreEqual($"Line {t}", line.Value);
			}
			Assert.IsNull(reader.Read());
		}

	

		[TestMethod]
		public void ShouldDiscardLines()
		{
			StringMatcher discardMatcher;
			LineReader reader;
			Line line;

			discardMatcher = new StringMatcher();
			discardMatcher.Add("Line 1");
			discardMatcher.Add("Line 3");
			discardMatcher.Add("Line 5");
			discardMatcher.Add("Line 7");
			reader = new LineReader(new MockedStringReader(10),discardMatcher );

			line = reader.Read();
			Assert.AreEqual(0, line.Index);
			Assert.AreEqual($"Line 0", line.Value);
			line = reader.Read();
			Assert.AreEqual(2, line.Index);
			Assert.AreEqual($"Line 2", line.Value);
			line = reader.Read();
			Assert.AreEqual(4, line.Index);
			Assert.AreEqual($"Line 4", line.Value);
			line = reader.Read();
			Assert.AreEqual(6, line.Index);
			Assert.AreEqual($"Line 6", line.Value);
			line = reader.Read();
			Assert.AreEqual(8, line.Index);
			Assert.AreEqual($"Line 8", line.Value);
		}

		

	}
}
