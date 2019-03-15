using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
			Match match;

			matcher = new StringMatcher();
			matcher.Add("abc");
			Assert.AreEqual(true,matcher.Match("abc"));
			match = matcher.GetMatch("abc");
			Assert.AreEqual(true, match.Success);
			Assert.AreEqual("abc", match.Value);
		}
		[TestMethod]
		public void ShouldNotMatch()
		{
			StringMatcher matcher;
			Match match;

			matcher = new StringMatcher();
			matcher.Add("bcd");
			Assert.AreEqual(false,matcher.Match("abc"));
			match = matcher.GetMatch("abc");
			Assert.AreEqual(null, match);
		}

	}
}
