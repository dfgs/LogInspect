using System;
using LogInspect.Models;
using LogLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogInspect.Modules.Test
{
	[TestClass]
	public class ColorProviderModuleUnitTest
	{
		[TestMethod]
		public void ShouldHaveCorrectConstructor()
		{
			Assert.ThrowsException<ArgumentNullException>(()=>new ColorProviderModule(NullLogger.Instance, null));
		}

		[TestMethod]
		public void ShouldReturnTransparentIfEventIsNull()
		{
			ColorProviderModule module;
			MemoryLogger logger;

			logger = new MemoryLogger(new DefaultLogFormatter());
			module = new ColorProviderModule(logger, new EventColoringRule[] { });
			Assert.AreEqual("Transparent", module.GetBackground(null));
			Assert.AreNotEqual(0, logger.Count);
		}
	}
}
