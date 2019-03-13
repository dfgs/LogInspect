using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LogInspect.Models;
using LogInspect.Modules.Readers;
using LogInspect.ModelsTest.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LogLib;

namespace LogInspect.ModelsTest
{
	[TestClass]
	public class LogReaderUnitTest
	{
		private static IRegexBuilder regexBuilder = new RegexBuilder();

		[TestMethod]
		public void ShouldHaveCorrectConstructorParameters()
		{
			Assert.ThrowsException<ArgumentNullException>(() => { new LogReader(NullLogger.Instance, null,Utils.EmptyStringMatcher,Utils.EmptyStringMatcher); });
			Assert.ThrowsException<ArgumentNullException>(() => { new LogReader(NullLogger.Instance, new MockedLineReader(), null, Utils.EmptyStringMatcher); });
			Assert.ThrowsException<ArgumentNullException>(() => { new LogReader(NullLogger.Instance, new MockedLineReader(), Utils.EmptyStringMatcher, null); });
		}


		[TestMethod]
		public void ShouldReadWithoutPatterns()
		{
			LogReader reader;

			reader = new LogReader(NullLogger.Instance, new MockedLineReader(5),Utils.EmptyStringMatcher,Utils.EmptyStringMatcher);

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
			IStringMatcher matcher;

			matcher = new StringMatcher();
			matcher.Add("[0-9]");

			reader = new LogReader(NullLogger.Instance, new MockedLineReaderWithAppend(6), matcher, Utils.EmptyStringMatcher);

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
			IStringMatcher matcher;

			matcher = new StringMatcher();
			matcher.Add(" $");

			reader = new LogReader(NullLogger.Instance, new MockedLineReaderWithAppend(6), Utils.EmptyStringMatcher, matcher);

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
			IStringMatcher matcher;
			MockedLineReaderWithIncompleteAppend lineReader;

			matcher = new StringMatcher();
			matcher.Add(" $");

			lineReader = new MockedLineReaderWithIncompleteAppend();

			reader = new LogReader(NullLogger.Instance, lineReader, Utils.EmptyStringMatcher, matcher);

			Assert.AreEqual(true, reader.CanRead);
			Assert.ThrowsException<EndOfStreamException>(() => { reader.Read(); });

		}

		//*/


	}
}
