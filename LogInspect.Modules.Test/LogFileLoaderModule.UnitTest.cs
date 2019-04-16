using System;
using System.Collections.Generic;

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
			Assert.ThrowsException<ArgumentNullException>(() => new LogFileLoaderModule(NullLogger.Instance, null, new MockedLogParser()));
			Assert.ThrowsException<ArgumentNullException>(() => new LogFileLoaderModule(NullLogger.Instance, new MockedLogReader(5),null));
		}

		[TestMethod]
		public void ShouldLoad()
		{
			LogFileLoaderModule module;
			List<Event> items;

			items = new List<Event>();
			module = new LogFileLoaderModule(NullLogger.Instance,new MockedLogReader(5), new MockedLogParser()); ;
			items.AddRange(module.Load());
			Assert.AreEqual(5, items.Count);
		}


		


	}
}
