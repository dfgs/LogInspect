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
	public class LogReaderUnitTest
	{


		[TestMethod]
		public void ShouldHaveCorrectConstructorParameters()
		{
			Assert.ThrowsException<ArgumentNullException>(() => { new LogReader(new MockedLineReader(10), new StringMatcher(), null); });
			Assert.ThrowsException<ArgumentNullException>(() => { new LogReader(new MockedLineReader(10), null, new StringMatcher()); });
			Assert.ThrowsException<ArgumentNullException>(() => { new LogReader(null, new StringMatcher(), new StringMatcher()); });
		}

		[TestMethod]
		public void ShouldReadLogs()
		{
			LogReader reader;
			Log log;
			StringMatcher stringMatcher;

			stringMatcher = new StringMatcher();
			stringMatcher.Add("Line");

			reader = new LogReader(new MockedLineReader(10) ,stringMatcher,Utils.EmptyStringMatcher); 

			for(int t=0;t<10;t++)
			{
				log = reader.Read();
				Assert.AreEqual(t, log.LineIndex);
				Assert.AreEqual($"Line {t}", log.ToSingleLine());
			}
			log = reader.Read();
			Assert.IsNull(log);

		}

		[TestMethod]
		public void ShouldDiscardLogs()
		{
			LogReader reader;
			Log log;
			StringMatcher stringMatcher;
			StringMatcher discardMatcher;

			stringMatcher = new StringMatcher();
			stringMatcher.Add("Line");

			discardMatcher = new StringMatcher();
			discardMatcher.Add("Line 1");
			discardMatcher.Add("Line 3");
			discardMatcher.Add("Line 5");
			discardMatcher.Add("Line 7");
			discardMatcher.Add("Line 9");


			reader = new LogReader(new MockedLineReader(10), stringMatcher, discardMatcher);

			for (int t = 0; t < 5; t++)
			{
				log = reader.Read();
				Assert.AreEqual(t*2, log.LineIndex);
				Assert.AreEqual($"Line {t*2}", log.ToSingleLine());
			}
			log = reader.Read();
			Assert.IsNull(log);

		}







	}
}
