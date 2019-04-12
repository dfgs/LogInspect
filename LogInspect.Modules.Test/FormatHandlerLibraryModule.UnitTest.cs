using System;
using System.Linq;
using LogInspect.BaseLib;
using LogInspect.Models;
using LogInspect.Modules.Test.Mocks;
using LogLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogInspect.Modules.Test
{
	[TestClass]
	public class FormatHandlerLibraryModuleUnitTest
	{
		

		[TestMethod]
		public void ShouldFindFormatHandler()
		{
			FormatHandlerLibraryModule module;

			module = new FormatHandlerLibraryModule(NullLogger.Instance, new MockedDirectoryEnumerator(5),new MockedFormatHandlerLoader(),new RegexBuilder());
			module.LoadDirectory("Path");
			Assert.AreEqual("Path1", module.GetFormatHandler("Path1").Name);
			Assert.AreEqual("Path2", module.GetFormatHandler("Path2").Name);
			Assert.AreEqual("Path3", module.GetFormatHandler("Path3").Name);
			Assert.AreEqual("Path4", module.GetFormatHandler("Path4").Name);
		}

		[TestMethod]
		public void ShouldNotFindFormatHandlerButReturnADefault()
		{
			FormatHandlerLibraryModule module;

			module = new FormatHandlerLibraryModule(NullLogger.Instance, new MockedDirectoryEnumerator(5), new MockedFormatHandlerLoader(), new RegexBuilder());
			module.LoadDirectory("Path");
			Assert.AreEqual("Default handler", module.GetFormatHandler("Path5").Name);
		}

		[TestMethod]
		public void ShouldNotThrowWhenPassingNullFileName()
		{
			FormatHandlerLibraryModule module;

			module = new FormatHandlerLibraryModule(NullLogger.Instance, new MockedDirectoryEnumerator(5), new MockedFormatHandlerLoader(), new RegexBuilder());
			module.LoadDirectory("Path");
			Assert.AreEqual("Default handler", module.GetFormatHandler(null).Name);
		}

		[TestMethod]
		public void ShouldFindFormatHandlers()
		{
			FormatHandlerLibraryModule module;
			FormatHandler[] items;

			module = new FormatHandlerLibraryModule(NullLogger.Instance, new MockedDirectoryEnumerator(5), new MockedFormatHandlerLoader(), new RegexBuilder());
			module.LoadDirectory("Path");

			items = module.GetFormatHandlers("Path1").ToArray();
			Assert.AreEqual(1, items.Length);
			Assert.AreEqual("Path1", items[0].Name);
		}

		[TestMethod]
		public void ShouldNotFindFormatHandlers()
		{
			FormatHandlerLibraryModule module;
			FormatHandler[] items;

			module = new FormatHandlerLibraryModule(NullLogger.Instance, new MockedDirectoryEnumerator(5), new MockedFormatHandlerLoader(), new RegexBuilder());
			module.LoadDirectory("Path");

			items = module.GetFormatHandlers("Default handler").ToArray();
			Assert.AreEqual(0, items.Length);
			items = module.GetFormatHandlers(null).ToArray();
			Assert.AreEqual(0, items.Length);
		}


	}
}
