using System;
using LogInspectLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogInspectLibTest
{
	[TestClass]
	public class LogUnitTest
	{
		[TestMethod]
		public void ShouldHaveCorrectConstructorParameters()
		{
			Assert.ThrowsException<ArgumentNullException>(() => { new Log(null); });
			Assert.ThrowsException<ArgumentNullException>(() => { new Log(new Line[0]); });
		}
	}
}
