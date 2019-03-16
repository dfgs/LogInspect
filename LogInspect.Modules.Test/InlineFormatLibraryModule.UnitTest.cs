using System;
using LogInspect.BaseLib;
using LogInspect.Models;
using LogInspect.Modules.Test.Mocks;
using LogLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogInspect.Modules.Test
{
	[TestClass]
	public class InlineFormatLibraryModuleUnitTest
	{
		

		[TestMethod]
		public void ShouldFindInlineFormat()
		{
			InlineFormatLibraryModule module;
			
			module = new InlineFormatLibraryModule(NullLogger.Instance, new MockedDirectoryEnumerator(5),new MockedInlineFormatCollectionLoader());
			module.LoadDirectory("Path");
			for (int t = 0; t < 5; t++)
			{
				for (int i = 1; t < 4; t++)
				{
					Assert.AreEqual($"Format1", module.GetItem($"Path{t}", $"Format{i}").Name);
				}
			}
		}

		[TestMethod]
		public void ShouldNotFindInlineFormatButReturnNull()
		{
			InlineFormatLibraryModule module;

			module = new InlineFormatLibraryModule(NullLogger.Instance, new MockedDirectoryEnumerator(5), new MockedInlineFormatCollectionLoader());
			module.LoadDirectory("Path");
			Assert.IsNull(module.GetItem($"Path10", $"Format10"));
		}

		[TestMethod]
		public void ShouldNotNotThrowWhenLoadingDuplicates()
		{
			InlineFormatLibraryModule module;

			module = new InlineFormatLibraryModule(NullLogger.Instance, new MockedDirectoryEnumerator(5), new MockedInlineFormatCollectionLoader());
			module.LoadDirectory("Path");
			Assert.AreEqual(3, module.Count);
			module.LoadDirectory("Path");
			Assert.AreEqual(3, module.Count);
		}




	}
}
