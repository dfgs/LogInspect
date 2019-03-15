using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.BaseLib.Test
{
	[TestClass]
	public class StringMatcherUnitTest
    {
		[TestMethod]
		public void ShouldMatch()
		{
			StringMatcher matcher;

			matcher = new StringMatcher();
			matcher.Add("abc");
			Assert.AreEqual(true,matcher.Match("abc"));
		}
		[TestMethod]
		public void ShouldNotMatch()
		{
			StringMatcher matcher;

			matcher = new StringMatcher();
			matcher.Add("bcd");
			Assert.AreEqual(false,matcher.Match("abc"));
		}

	}
}
