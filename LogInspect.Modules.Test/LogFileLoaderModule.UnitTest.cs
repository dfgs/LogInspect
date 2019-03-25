using System;
using LogInspect.BaseLib.Builders;
using LogInspect.Models;
using LogInspect.Modules.Test.Mocks;
using LogLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogInspect.Modules.Test
{
	[TestClass]
	public class LogFileLoaderModuleUnitTest
	{
		[TestMethod]
		public void ShouldHaveCorrectConstructor()
		{
			Assert.ThrowsException<ArgumentNullException>(() => new LogFileLoaderModule(NullLogger.Instance, null, new MockedLineBuilder(), new MockedLogBuilder(), new MockedLogParser(), new EventList()));
			Assert.ThrowsException<ArgumentNullException>(() => new LogFileLoaderModule(NullLogger.Instance, new MockedLineReader(5), null, new MockedLogBuilder(), new MockedLogParser(), new EventList()));
			Assert.ThrowsException<ArgumentNullException>(() => new LogFileLoaderModule(NullLogger.Instance, new MockedLineReader(5), new MockedLineBuilder(), null, new MockedLogParser(), new EventList()));
			Assert.ThrowsException<ArgumentNullException>(() => new LogFileLoaderModule(NullLogger.Instance, new MockedLineReader(5), new MockedLineBuilder(), new MockedLogBuilder(), null, new EventList()));
			Assert.ThrowsException<ArgumentNullException>(() => new LogFileLoaderModule(NullLogger.Instance, new MockedLineReader(5), new MockedLineBuilder(), new MockedLogBuilder(), new MockedLogParser(), null));
		}

		[TestMethod]
		public void ShouldLoad()
		{
			LogFileLoaderModule module;
			EventList eventList;

			eventList = new EventList();
			module =new LogFileLoaderModule(NullLogger.Instance, new MockedLineReader(5), new MockedLineBuilder(), new MockedLogBuilder(), new MockedLogParser(), eventList);
			module.Load();
			Assert.AreEqual(5, eventList.Count);
		}



	}
}
