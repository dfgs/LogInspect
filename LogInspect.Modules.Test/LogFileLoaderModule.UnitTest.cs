using System;
using System.Collections.Generic;
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
			Assert.ThrowsException<ArgumentNullException>(() => new LogFileLoaderModule(NullLogger.Instance, null, new MockedLineBuilder(), new MockedLogBuilder(), new MockedLogParser()));
			Assert.ThrowsException<ArgumentNullException>(() => new LogFileLoaderModule(NullLogger.Instance, new MockedLineReader(5), null, new MockedLogBuilder(), new MockedLogParser()));
			Assert.ThrowsException<ArgumentNullException>(() => new LogFileLoaderModule(NullLogger.Instance, new MockedLineReader(5), new MockedLineBuilder(), null, new MockedLogParser()));
			Assert.ThrowsException<ArgumentNullException>(() => new LogFileLoaderModule(NullLogger.Instance, new MockedLineReader(5), new MockedLineBuilder(), new MockedLogBuilder(), null));
		}

		[TestMethod]
		public void ShouldLoad()
		{
			LogFileLoaderModule module;
			List<Event> items;

			items = new List<Event>();
			module =new LogFileLoaderModule(NullLogger.Instance, new MockedLineReader(5), new MockedLineBuilder(), new MockedLogBuilder(), new MockedLogParser());
			items.AddRange(module.Load());
			Assert.AreEqual(5, items.Count);
		}
		[TestMethod]
		public void ShouldLoadAndFlush()
		{
			LogFileLoaderModule module;
			List<Event> items;

			items = new List<Event>();
			module = new LogFileLoaderModule(NullLogger.Instance, new MockedLineReader(5), new MockedLineBuilder(), new MockedLogBuilderWithFlush(), new MockedLogParser());
			items.AddRange(module.Load());
			Assert.AreEqual(5, items.Count);

			items = new List<Event>();
			module = new LogFileLoaderModule(NullLogger.Instance, new MockedLineReader(5), new MockedLineBuilderWithFlush(), new MockedLogBuilder(), new MockedLogParser());
			items.AddRange(module.Load());
			Assert.AreEqual(5, items.Count);

			items = new List<Event>();
			module = new LogFileLoaderModule(NullLogger.Instance, new MockedLineReader(5), new MockedLineBuilderWithFlush(), new MockedLogBuilderWithFlush(), new MockedLogParser());
			items.AddRange(module.Load());
			Assert.AreEqual(5, items.Count);
		}


	}
}
