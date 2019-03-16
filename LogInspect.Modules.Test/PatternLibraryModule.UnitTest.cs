using System;
using LogInspect.BaseLib;
using LogInspect.Models;
using LogInspect.Modules.Test.Mocks;
using LogLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogInspect.Modules.Test
{
	[TestClass]
	public class PatternLibraryModuleUnitTest
	{
		

		[TestMethod]
		public void ShouldFindPattern()
		{
			PatternLibraryModule module;
			
			module = new PatternLibraryModule(NullLogger.Instance, new MockedDirectoryEnumerator(5),new MockedPatternCollectionLoader());
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
		public void ShouldNotFindPatternButReturnNull()
		{
			PatternLibraryModule module;

			module = new PatternLibraryModule(NullLogger.Instance, new MockedDirectoryEnumerator(5), new MockedPatternCollectionLoader());
			module.LoadDirectory("Path");
			Assert.IsNull(module.GetItem($"Path10", $"Format10"));


		}

	}
}
