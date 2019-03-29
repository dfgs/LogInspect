using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LogInspect.BaseLib.Builders;
using LogInspect.BaseLib.Test.Mocks;
using LogInspect.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogInspect.BaseLib.Test.Builders
{
	[TestClass]
	public class LogBuilderUnitTest
	{
		private static IRegexBuilder regexBuilder = new RegexBuilder();

		[TestMethod]
		public void ShouldHaveCorrectConstructorParameters()
		{
			Assert.ThrowsException<ArgumentNullException>(() => { new LogBuilder(null, Utils.EmptyStringMatcher, Utils.EmptyStringMatcher); });
			Assert.ThrowsException<ArgumentNullException>(() => { new LogBuilder(Utils.EmptyStringMatcher, null, Utils.EmptyStringMatcher); });
			Assert.ThrowsException<ArgumentNullException>(() => { new LogBuilder(Utils.EmptyStringMatcher, Utils.EmptyStringMatcher,null); });
		}


	
		[TestMethod]
		public void ShouldNotFlushWhenEmpty()
		{
			LogBuilder Builder;

			Builder = new LogBuilder(Utils.EmptyStringMatcher, Utils.EmptyStringMatcher, Utils.EmptyStringMatcher);

			Assert.IsFalse(Builder.CanFlush);
			Assert.ThrowsException<InvalidOperationException>(Builder.Flush);
		}
		[TestMethod]
		public void ShouldFlushWhenFilled()
		{
			LogBuilder Builder;
			Log log;

			Builder = new LogBuilder(Utils.EmptyStringMatcher, Utils.EmptyStringMatcher, Utils.EmptyStringMatcher);
			Assert.IsFalse(Builder.Push(new Line() { Index = 1, Value = $"Item" }, out log));
			Assert.IsTrue(Builder.CanFlush);
			log = Builder.Flush();
			Assert.AreEqual(1, log.LineIndex);
			Assert.AreEqual($"Item", log.ToSingleLine());
			
			Assert.IsFalse(Builder.CanFlush);
			Assert.ThrowsException<InvalidOperationException>(Builder.Flush);
		}


		[TestMethod]
		public void ShouldBuildWithoutPatterns()
		{
			LogBuilder Builder;
			Log log;

			Builder = new LogBuilder(Utils.EmptyStringMatcher, Utils.EmptyStringMatcher, Utils.EmptyStringMatcher);

			Assert.IsFalse(Builder.Push(new Line() { Index = 0, Value = $"Item{0}" }, out log));
			for (int t = 1; t < 5; t++)
			{
				Assert.IsTrue(Builder.Push(new Line() { Index = t, Value = $"Item{t}" }, out log));
				Assert.AreEqual(t-1, log.LineIndex);
				Assert.AreEqual($"Item{t-1}", log.ToSingleLine());
			}

			Assert.IsTrue(Builder.CanFlush);
			log = Builder.Flush();
			Assert.AreEqual(4, log.LineIndex);
			Assert.AreEqual($"Item4", log.ToSingleLine());

		}
		[TestMethod]
		public void ShouldBuildWithAppendToPreviousPatterns()
		{
			LogBuilder Builder;
			Log log;
			StringMatcher matcher;

			matcher = new StringMatcher();
			matcher.Add("[0-9]");		// we append to previous when line ends with number

			Builder = new LogBuilder(Utils.EmptyStringMatcher, matcher, Utils.EmptyStringMatcher);

			Assert.IsFalse(Builder.Push(new Line() { Index = 0, Value = "0" }, out log));
			for (int t = 1; t < 5; t++)
			{
				Assert.IsFalse(Builder.Push(new Line() { Index = t, Value = $"{t}" }, out log));
			}
			Assert.IsTrue(Builder.Push(new Line() { Index = 5, Value = $"End" }, out log));
			Assert.AreEqual(0, log.LineIndex);
			Assert.AreEqual("01234", log.ToSingleLine());


			Assert.IsTrue(Builder.CanFlush);
			log = Builder.Flush();
			Assert.AreEqual(5, log.LineIndex);
			Assert.AreEqual($"End", log.ToSingleLine());
		}
		[TestMethod]
		public void ShouldBuildWithAppendToNextPatterns()
		{
			LogBuilder Builder;
			Log log;
			StringMatcher matcher;

			matcher = new StringMatcher();
			matcher.Add("[0-9]");       // we append to next when line ends with number

			Builder = new LogBuilder(Utils.EmptyStringMatcher, Utils.EmptyStringMatcher,matcher );

			for (int t = 0; t < 5; t++)
			{
				Assert.IsFalse(Builder.Push(new Line() { Index = t, Value = $"{t}" }, out log));
			}
			Assert.IsFalse(Builder.Push(new Line() { Index = 5, Value = $"End" }, out log));
			Assert.IsTrue(Builder.Push(new Line() { Index = 5, Value = $"End" }, out log));
			Assert.AreEqual(0, log.LineIndex);
			Assert.AreEqual("01234End", log.ToSingleLine());


			Assert.IsTrue(Builder.CanFlush);
			log = Builder.Flush();
			Assert.AreEqual(5, log.LineIndex);
			Assert.AreEqual($"End", log.ToSingleLine());
		}

		[TestMethod]
		public void ShouldBuildWithDiscardPatterns()
		{
			LogBuilder Builder;
			Log log;
			StringMatcher matcher;


			matcher = new StringMatcher();
			matcher.Add("discard");       // to discard

			Builder = new LogBuilder(matcher, Utils.EmptyStringMatcher, Utils.EmptyStringMatcher);

			Assert.IsFalse(Builder.Push(new Line() { Index = 0, Value = $"Item{0}" }, out log));
			for (int t = 1; t < 5; t++)
			{
				Assert.IsFalse(Builder.Push(new Line() { Index = t, Value = "discard" }, out log));
				Assert.IsFalse(Builder.Push(new Line() { Index = t, Value = "discard" }, out log));
				Assert.IsTrue(Builder.Push(new Line() { Index = t, Value = $"Item{t}" }, out log));
				Assert.AreEqual(t - 1, log.LineIndex);
				Assert.AreEqual($"Item{t - 1}", log.ToSingleLine());
			}

			Assert.IsTrue(Builder.CanFlush);
			log = Builder.Flush();
			Assert.AreEqual(4, log.LineIndex);
			Assert.AreEqual($"Item4", log.ToSingleLine());

		}

	}
}
