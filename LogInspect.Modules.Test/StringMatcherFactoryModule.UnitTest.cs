using System;
using LogInspect.BaseLib;
using LogInspect.Models;
using LogLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogInspect.Modules.Test
{
	[TestClass]
	public class StringMatcherFactoryModuleUnitTest
	{
		[TestMethod]
		public void ShouldHaveCorrectConstructor()
		{
			Assert.ThrowsException<ArgumentNullException>(()=>new StringMatcherFactoryModule(NullLogger.Instance, null));
		}

		[TestMethod]
		public void ShouldCreateStringMatcher()
		{
			StringMatcherFactoryModule module;
			MemoryLogger logger;
			IRegexBuilder regexBuilder;

			regexBuilder = new RegexBuilder();
			logger = new MemoryLogger(new DefaultLogFormatter());
			module = new StringMatcherFactoryModule(logger,regexBuilder );
			Assert.IsNotNull( module.CreateStringMatcher("default",new string[] { "abc","def"}));
			Assert.AreEqual(0, logger.Count);
		}
		[TestMethod]
		public void ShouldCreateStringMatcherAndLogErrors()
		{
			StringMatcherFactoryModule module;
			MemoryLogger logger;
			IRegexBuilder regexBuilder;

			regexBuilder = new RegexBuilder();
			logger = new MemoryLogger(new DefaultLogFormatter());
			module = new StringMatcherFactoryModule(logger, regexBuilder);
			Assert.IsNotNull(module.CreateStringMatcher(null, new string[] { "[", "def" }));	// invalid regex
			Assert.AreEqual(1, logger.Count);
		}

	}
}
