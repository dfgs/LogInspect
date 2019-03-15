using System;
using LogInspect.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogInspect.BaseLib.Test
{
	[TestClass]
	public class InlineUnitTest
	{
		[TestMethod]
		public void ShouldIntersect()
		{
			Inline a, b;

			a = new Inline() { Index = 0, Length = 5 };
			b = new Inline() { Index = 4, Length = 5 };
			Assert.AreEqual(true, a.Intersect(b));
			Assert.AreEqual(true, b.Intersect(a));

			a = new Inline() { Index = 0, Length = 5 };
			b = new Inline() { Index = 2, Length = 1 };
			Assert.AreEqual(true, a.Intersect(b));
			Assert.AreEqual(true, b.Intersect(a));
		}

		[TestMethod]
		public void ShouldNotIntersect()
		{
			Inline a, b;

			a = new Inline() { Index = 0, Length = 5 };
			b = new Inline() { Index = 5, Length = 5 };
			Assert.AreEqual(false, a.Intersect(b));
			Assert.AreEqual(false, b.Intersect(a));

			
		}


	}
}
