using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LogInspect.BaseLib.Parsers;
using LogInspect.BaseLib.Readers;
using LogInspect.BaseLib.Test.Mocks;
using LogInspect.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogInspect.BaseLib.Test
{
	[TestClass]
	public class EventReaderUnitTest
	{
		private static IRegexBuilder regexBuilder = new RegexBuilder();

		[TestMethod]
		public void ShouldHaveCorrectConstructorParameters()
		{
			Assert.ThrowsException<ArgumentNullException>(() => { new EventReader(null, new MockedLogParser()); });
			Assert.ThrowsException<ArgumentNullException>(() => { new EventReader(new MockedLogReader(5), null); });
		}


		[TestMethod]
		public void ShouldReadPatterns()
		{
			EventReader reader;
			Event ev;

			reader = new EventReader( new MockedLogReader(5),new MockedLogParser());

			for (int t = 0; t < 5; t++)
			{
				Assert.IsTrue(reader.CanRead);
				ev = reader.Read();
				Assert.AreEqual(t, ev.LineIndex);
			}
			Assert.AreEqual(false, reader.CanRead);
			Assert.ThrowsException<EndOfStreamException>(()=> { reader.Read(); });
		}




	}
}
