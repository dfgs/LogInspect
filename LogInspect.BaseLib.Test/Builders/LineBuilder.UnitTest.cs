using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using LogInspect.BaseLib.Builders;
using LogInspect.BaseLib.Test.Mocks;
using LogInspect.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogInspect.BaseLib.Test.Builders
{
	[TestClass]
	public class LineBuilderUnitTest
	{


		[TestMethod]
		public void ShouldHaveCorrectConstructorParameters()
		{
			Assert.ThrowsException<ArgumentNullException>(() => { new LineBuilder(null); });
		}

		[TestMethod]
		public void ShouldBuildLines()
		{
			LineBuilder builder;
			string[] items = new string[] { "item1", "item2", "item3", "", "item4", "" };
			Line line;
			int index=0;

			builder = new LineBuilder(Utils.EmptyStringMatcher);

			foreach (string item in items)
			{
				Assert.IsTrue(builder.Push(item, out line));
				Assert.AreEqual(item,line.Value );
				Assert.AreEqual(index, line.Index);
				index++;
			}
				
		}

		[TestMethod]
		public void ShouldDiscardLines()
		{
			StringMatcher matcher;
			LineBuilder builder;
			Line line;

			matcher = new StringMatcher();
			matcher.Add("discard");
			builder = new LineBuilder(matcher);

			for (int t = 0; t < 10; t++)
			{
				Assert.IsFalse(builder.Push("discard", out line));
				Assert.IsNull(line);
			}
		}

		[TestMethod]
		public void ShouldKeepCorrectIndexLines()
		{
			StringMatcher matcher;
			LineBuilder builder;
			Line line;

			matcher = new StringMatcher();
			matcher.Add("discard");
			builder = new LineBuilder(matcher);

			for(int t=0;t<10;t++)
			{
				Assert.IsFalse(builder.Push("discard", out line));
				Assert.IsNull(line);
			}
			Assert.IsTrue(builder.Push("Item1", out line));
			Assert.AreEqual("Item1", line.Value);
			Assert.AreEqual(10, line.Index);
		}
		[TestMethod]
		public void ShouldNotFlush()
		{
			LineBuilder builder;

			builder = new LineBuilder(Utils.EmptyStringMatcher);
			Assert.IsFalse(builder.CanFlush);
			Assert.ThrowsException<InvalidOperationException>(()=>builder.Flush());
		}

	}
}
