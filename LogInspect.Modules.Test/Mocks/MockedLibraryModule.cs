using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogInspect.BaseLib;
using LogInspect.BaseLib.FileLoaders;
using LogLib;

namespace LogInspect.Modules.Test.Mocks
{
	public class MockedLibraryModule : LibraryModule<string>
	{
		public List<string> Items;

		public MockedLibraryModule(IDirectoryEnumerator DirectoryEnumerator,IFileLoader<string> FileLoader ) : base(NullLogger.Instance, DirectoryEnumerator,FileLoader)
		{
			Items = new List<string>();
		}

		protected override void OnItemLoaded(string Item)
		{
			Items.Add(Item);
		}

		

	}
}
