using System;
using LogInspect.Models;
using LogInspect.Modules.Test.Mocks;
using LogLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogInspect.Modules.Test
{
	[TestClass]
	public class LibraryModuleUnitTest
	{
		

		[TestMethod]
		public void ShouldLoadFiles()
		{
			MockedLibraryModule module;

			module = new MockedLibraryModule(new MockedDirectoryEnumerator(5),new MockedFileLoader());
			module.LoadDirectory("Path");
			Assert.AreEqual(5, module.Items.Count);
			Assert.AreEqual("Path0", module.Items[0]);
			Assert.AreEqual("Path1", module.Items[1]);
			Assert.AreEqual("Path2", module.Items[2]);
			Assert.AreEqual("Path3", module.Items[3]);
			Assert.AreEqual("Path4", module.Items[4]);
		}

		[TestMethod]
		public void ShouldHandleDirectoryEnumeratorErrors()
		{
			MockedLibraryModule module;

			module = new MockedLibraryModule(new InvalidDirectoryEnumerator(5), new MockedFileLoader());
			module.LoadDirectory("Path");
			Assert.AreEqual(4, module.Items.Count);
			Assert.AreEqual("Path0", module.Items[0]);
			Assert.AreEqual("Path1", module.Items[1]);
			Assert.AreEqual("Path2", module.Items[2]);
			Assert.AreEqual("Path3", module.Items[3]);
		}
		[TestMethod]
		public void ShouldHandleFileLoaderErrors()
		{
			MockedLibraryModule module;

			module = new MockedLibraryModule(new MockedDirectoryEnumerator(5), new InvalidFileLoader());
			module.LoadDirectory("Path");
			Assert.AreEqual(0, module.Items.Count);
		}

	}
}
