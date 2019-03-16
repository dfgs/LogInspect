using System;
using LogInspect.BaseLib;
using LogInspect.BaseLib.Parsers;
using LogInspect.Models;
using LogInspect.Modules.Test.Mocks;
using LogLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogInspect.Modules.Test
{
	[TestClass]
	public class InlineParserFactoryModuleUnitTest
	{
		[TestMethod]
		public void ShouldHaveCorrectConstructor()
		{
			Assert.ThrowsException<ArgumentNullException>(() => new InlineParserFactoryModule(NullLogger.Instance, null, new InlineFormatLibraryModule(NullLogger.Instance, new MockedDirectoryEnumerator(5), new MockedInlineFormatCollectionLoader())));
			Assert.ThrowsException<ArgumentNullException>(() => new InlineParserFactoryModule(NullLogger.Instance, new RegexBuilder(), null));
		}

		[TestMethod]
		public void ShouldCreateInlineParser()
		{
			InlineParserFactoryModule module;


			module=new InlineParserFactoryModule(NullLogger.Instance, new RegexBuilder(), new InlineFormatLibraryModule(NullLogger.Instance, new MockedDirectoryEnumerator(5), new MockedInlineFormatCollectionLoader()));
			Assert.IsNotNull( module.CreateParser("test", new Column()));

		}

		[TestMethod]
		public void ShouldHaveCorrectParameters()
		{
			InlineParserFactoryModule module;


			module = new InlineParserFactoryModule(NullLogger.Instance, new RegexBuilder(), new InlineFormatLibraryModule(NullLogger.Instance, new MockedDirectoryEnumerator(5), new MockedInlineFormatCollectionLoader()));
			Assert.IsNull(module.CreateParser(null, new Column()));
			Assert.IsNull(module.CreateParser("Test", null));

		}





	}
}
