using System;
using LogInspectLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogInspectLibTest
{
	[TestClass]
	public class FormatHandlerUnitTest
	{
		[TestMethod]
		public void ShouldMatchBasicLogFileName()
		{
			FormatHandler formatHandler;
			formatHandler = new FormatHandler();
			formatHandler.FileNamePattern = @"\.log$";
			Assert.IsTrue(formatHandler.MatchFileName("test.log"));
			Assert.IsFalse(formatHandler.MatchFileName("test.log.txt"));
		}
		[TestMethod]
		public void ShouldMatchExtendedLogFileName()
		{
			FormatHandler formatHandler;
			formatHandler = new FormatHandler();
			formatHandler.FileNamePattern = @"^RCM\.log(\.\d+)?$";
			Assert.IsTrue(formatHandler.MatchFileName("RCM.log"));
			Assert.IsTrue(formatHandler.MatchFileName("RCM.log.37176"));
			Assert.IsTrue(formatHandler.MatchFileName("RCM.log.1"));
			Assert.IsFalse(formatHandler.MatchFileName("test.RCM.log.37176"));
			Assert.IsFalse(formatHandler.MatchFileName("test.RCM.log."));
		}

	}
}
