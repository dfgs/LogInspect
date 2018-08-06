using System;
using LogInspectLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogInspectLibTest
{
	[TestClass]
	public class EventUnitTest
	{
		[TestMethod]
		public void ShouldHaveCorrectConstructorParameters()
		{
			Assert.ThrowsException<ArgumentNullException>(() => { new Event(new Log(new Line(0,"test")),new Rule(),Severity.Debug,null); });
		}
	}
}
