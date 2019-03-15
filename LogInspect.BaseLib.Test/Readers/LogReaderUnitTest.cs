using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LogInspect.BaseLib.Readers;
using LogInspect.BaseLib.Test.Mocks;
using LogInspect.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogInspect.BaseLib.Test
{
	[TestClass]
	public class LogReaderUnitTest
	{
		private static IRegexBuilder regexBuilder = new RegexBuilder();

		[TestMethod]
		public void ShouldHaveCorrectConstructorParameters()
		{
			Assert.ThrowsException<ArgumentNullException>(() => { new LogReader( null,Utils.EmptyStringMatcher,Utils.EmptyStringMatcher); });
			Assert.ThrowsException<ArgumentNullException>(() => { new LogReader( new MockedLineReader(), null, Utils.EmptyStringMatcher); });
			Assert.ThrowsException<ArgumentNullException>(() => { new LogReader( new MockedLineReader(), Utils.EmptyStringMatcher, null); });
		}


		[TestMethod]
		public void ShouldReadWithoutPatterns()
		{
			LogReader reader;

			reader = new LogReader( new MockedLineReader(5),Utils.EmptyStringMatcher,Utils.EmptyStringMatcher);

			for (int t = 0; t < 5; t++)
			{
				Assert.AreEqual(true, reader.CanRead);
				Assert.AreEqual($"Item {t}", reader.Read().ToSingleLine());
			}
			Assert.AreEqual(false, reader.CanRead);
			Assert.ThrowsException<EndOfStreamException>(()=> { reader.Read(); });
			
		}

		
		[TestMethod]
		public void ShouldReadLogWithAppendToPreviousPatterns()
		{
			LogReader reader;
			StringMatcher matcher;

			matcher = new StringMatcher();
			matcher.Add("[0-9]");

			reader = new LogReader( new MockedLineReaderWithAppend(6), matcher, Utils.EmptyStringMatcher);

			for (int t = 0; t < 3; t++)
			{
				Assert.AreEqual(true, reader.CanRead);
				Assert.AreEqual($"Item {t*2+1}", reader.Read().ToSingleLine());
			}
			Assert.AreEqual(false, reader.CanRead);
			Assert.ThrowsException<EndOfStreamException>(() => { reader.Read(); });
		}
		
		[TestMethod]
		public void ShouldReadLogWithAppendToNextPatterns()
		{
			LogReader reader;
			StringMatcher matcher;

			matcher = new StringMatcher();
			matcher.Add(" $");

			reader = new LogReader( new MockedLineReaderWithAppend(6), Utils.EmptyStringMatcher, matcher);

			for (int t = 0; t < 3; t++)
			{
				Assert.AreEqual(true, reader.CanRead);
				Assert.AreEqual($"Item {t*2+1}", reader.Read().ToSingleLine());
			}
			Assert.AreEqual(false, reader.CanRead);
			Assert.ThrowsException<EndOfStreamException>(() => { reader.Read(); });

		}

		
		[TestMethod]
		public void ShouldFailIfCannotAppendToNext()
		{
			LogReader reader;
			StringMatcher matcher;
			MockedLineReaderWithIncompleteAppend lineReader;

			matcher = new StringMatcher();
			matcher.Add(" $");

			lineReader = new MockedLineReaderWithIncompleteAppend();

			reader = new LogReader( lineReader, Utils.EmptyStringMatcher, matcher);

			Assert.AreEqual(true, reader.CanRead);
			Assert.ThrowsException<EndOfStreamException>(() => { reader.Read(); });

		}

		//*/


	}
}
