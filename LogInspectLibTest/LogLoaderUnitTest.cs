using System;
using System.IO;
using System.Text;
using LogInspectLib;
using LogInspectLib.Loaders;
using LogInspectLibTest.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogInspectLibTest
{
	[TestClass]
	public class LogLoaderUnitTest
	{
		private static IRegexBuilder regexBuilder = new RegexBuilder();

		[TestMethod]
		public void ShouldHaveCorrectConstructorParameters()
		{
			Assert.ThrowsException<ArgumentNullException>(() => { new LogLoader(null,Utils.EmptyStringMatcher,Utils.EmptyStringMatcher); });
			Assert.ThrowsException<ArgumentNullException>(() => { new LogLoader(Utils.EmptyLineLoader, null, Utils.EmptyStringMatcher); });
			Assert.ThrowsException<ArgumentNullException>(() => { new LogLoader(Utils.EmptyLineLoader, Utils.EmptyStringMatcher, null); });
		}


		[TestMethod]
		public void ShouldReadWithoutPatterns()
		{
			ILineLoader lineLoader;
			LogLoader loader;

			lineLoader = new MockedLineLoader();
			for (int t = 0; t < 5; t++) lineLoader.Load();
			
			loader = new LogLoader(lineLoader,Utils.EmptyStringMatcher,Utils.EmptyStringMatcher);

			for (int t = 0; t < 5; t++)
			{
				loader.Load();
				Assert.AreEqual(lineLoader[t].Value, loader[t].ToSingleLine());
			}
			Assert.AreEqual(5,loader.Count);
			Assert.ThrowsException<EndOfStreamException>(()=> { loader.Load(); });
		}


		[TestMethod]
		public void ShouldReadLogWithAppendToPreviousPatterns()
		{
			MockedLineLoader lineLoader;
			LogLoader loader;
			IStringMatcher matcher;

			lineLoader = new MockedLineLoader();
			lineLoader.Load("Item ");
			lineLoader.Load("1");
			lineLoader.Load("Item ");
			lineLoader.Load("2");
			lineLoader.Load("Item ");
			lineLoader.Load("3");
			lineLoader.Load("Item ");
			lineLoader.Load("4");
			lineLoader.Load("Item A");
			lineLoader.Load("Item B");

			matcher = new StringMatcher();
			matcher.Add("[0-9]");

			loader = new LogLoader(lineLoader, matcher, Utils.EmptyStringMatcher);

			for (int t = 0; t < 6; t++) loader.Load();

			Assert.AreEqual("Item 1", loader[0].ToSingleLine());
			Assert.AreEqual("Item 2", loader[1].ToSingleLine());
			Assert.AreEqual("Item 3", loader[2].ToSingleLine());
			Assert.AreEqual("Item 4", loader[3].ToSingleLine());
			Assert.AreEqual("Item A", loader[4].ToSingleLine());
			Assert.AreEqual("Item B", loader[5].ToSingleLine());

			Assert.AreEqual(6, loader.Count);
			Assert.ThrowsException<EndOfStreamException>(() =>  loader.Load() );
		}

		[TestMethod]
		public void ShouldReadLogWithAppendToNextPatterns()
		{
			MockedLineLoader lineLoader;
			LogLoader loader;
			IStringMatcher matcher;

			lineLoader = new MockedLineLoader();
			lineLoader.Load("Item ");
			lineLoader.Load("1");
			lineLoader.Load("Item ");
			lineLoader.Load("2");
			lineLoader.Load("Item ");
			lineLoader.Load("3");
			lineLoader.Load("Item ");
			lineLoader.Load("4");
			lineLoader.Load("Item A");
			lineLoader.Load("Item B");

			matcher = new StringMatcher();
			matcher.Add( " $");

			loader = new LogLoader(lineLoader, Utils.EmptyStringMatcher, matcher);

			for (int t = 0; t < 6; t++) loader.Load();

			Assert.AreEqual("Item 1", loader[0].ToSingleLine());
			Assert.AreEqual("Item 2", loader[1].ToSingleLine());
			Assert.AreEqual("Item 3", loader[2].ToSingleLine());
			Assert.AreEqual("Item 4", loader[3].ToSingleLine());
			Assert.AreEqual("Item A", loader[4].ToSingleLine());
			Assert.AreEqual("Item B", loader[5].ToSingleLine());

			Assert.AreEqual(6, loader.Count);
			Assert.ThrowsException<EndOfStreamException>(() => loader.Load());
		}
		[TestMethod]
		public void ShouldFailIfCannotAppendToNext()
		{
			MockedLineLoader lineLoader;
			LogLoader loader;
			IStringMatcher matcher;

			lineLoader = new MockedLineLoader();
			lineLoader.Load("Item ");
			lineLoader.Load("1");
			lineLoader.Load("Item ");
			lineLoader.Load("2");
			lineLoader.Load("Item ");
			lineLoader.Load("3");
			lineLoader.Load("Item ");

			matcher = new StringMatcher();
			matcher.Add( " $");

			loader = new LogLoader(lineLoader, Utils.EmptyStringMatcher, matcher);

			for (int t = 0; t < 3; t++) loader.Load();

			Assert.AreEqual("Item 1", loader[0].ToSingleLine());
			Assert.AreEqual("Item 2", loader[1].ToSingleLine());
			Assert.AreEqual("Item 3", loader[2].ToSingleLine());

			// cannot load last item because missing EOL
			Assert.AreEqual(3, loader.Count);
			Assert.ThrowsException<EndOfStreamException>(() => loader.Load());

			// can load if we add missing line
			lineLoader.Load("4");
			loader.Load();

			Assert.AreEqual("Item 4", loader[3].ToSingleLine());
			Assert.AreEqual(4, loader.Count);
			Assert.ThrowsException<EndOfStreamException>(() => loader.Load());

		}





	}
}
