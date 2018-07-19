using System;
using LogInspectLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogInspectLibTest
{
	[TestClass]
	public class LineUnitTest
	{
		[TestMethod]
		public void ShouldHaveCorrectConstructorParameters()
		{
			Assert.ThrowsException<ArgumentNullException>(() => { new Line(0, null); });
			Assert.ThrowsException<ArgumentException>(() => { new Line(-1, "test"); });
		}
	}
}
