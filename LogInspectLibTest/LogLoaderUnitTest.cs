using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LogInspectLib;
using LogInspectLib.Loaders;
using LogInspectLibTest.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogInspectLibTest
{
	[TestClass]
	public class LogLoaderUnitTest
	{
		private static IRegexBuilder regexBuilder = new RegexBuilder();

		[TestMethod]
		public void ShouldHaveCorrectConstructorParameters()
		{
			Assert.ThrowsException<ArgumentNullException>(() => { new LogLoader(null,Utils.EmptyStringMatcher,Utils.EmptyStringMatcher); });
			Assert.ThrowsException<ArgumentNullException>(() => { new LogLoader(new MockedLineReader(), null, Utils.EmptyStringMatcher); });
			Assert.ThrowsException<ArgumentNullException>(() => { new LogLoader(new MockedLineReader(), Utils.EmptyStringMatcher, null); });
		}


		[TestMethod]
		public void ShouldReadWithoutPatterns()
		{
			LogLoader loader;
			int index;

			loader = new LogLoader(new MockedLineReader(5),Utils.EmptyStringMatcher,Utils.EmptyStringMatcher);

			for (int t = 0; t < 5; t++)
			{
				loader.Load();
			}
			Assert.AreEqual(5,loader.Count);
			Assert.ThrowsException<EndOfStreamException>(()=> { loader.Load(); });

			index = 0;
			foreach (Log item in  loader.GetBuffer())
			{
				Assert.AreEqual($"Item {index}", item.ToSingleLine());
				index++;
			}
		}


		[TestMethod]
		public void ShouldReadLogWithAppendToPreviousPatterns()
		{
			LogLoader loader;
			IStringMatcher matcher;
			int index;

			matcher = new StringMatcher();
			matcher.Add("[0-9]");

			loader = new LogLoader(new MockedLineReaderWithAppend(6), matcher, Utils.EmptyStringMatcher);

			for (int t = 0; t < 3; t++)
			{
				loader.Load();
			}

			Assert.AreEqual(3, loader.Count);
			Assert.ThrowsException<EndOfStreamException>(() => { loader.Load(); });

			index = 0;
			foreach (Log item in loader.GetBuffer())
			{
				Assert.AreEqual($"Item {index}", item.ToSingleLine());
				index+=2;
			}
		}

		[TestMethod]
		public void ShouldReadLogWithAppendToNextPatterns()
		{
			LogLoader loader;
			IStringMatcher matcher;
			int index;

			matcher = new StringMatcher();
			matcher.Add(" $");

			loader = new LogLoader(new MockedLineReaderWithAppend(6), Utils.EmptyStringMatcher, matcher);

			for (int t = 0; t < 3; t++)
			{
				loader.Load();
			}

			Assert.AreEqual(3, loader.Count);
			Assert.ThrowsException<EndOfStreamException>(() => { loader.Load(); });

			index = 0;
			foreach (Log item in loader.GetBuffer())
			{
				Assert.AreEqual($"Item {index}", item.ToSingleLine());
				index += 2;
			}
		}
		
		[TestMethod]
		public void ShouldFailIfCannotAppendToNext()
		{
			LogLoader loader;
			IStringMatcher matcher;
			MockedLineReaderWithIncompleteAppend lineReader;

			matcher = new StringMatcher();
			matcher.Add(" $");

			lineReader = new MockedLineReaderWithIncompleteAppend();

			loader = new LogLoader(lineReader, Utils.EmptyStringMatcher, matcher);

			Assert.ThrowsException<EndOfStreamException>(() => { loader.Load(); });

		}




	}
}
