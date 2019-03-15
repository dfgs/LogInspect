using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogLib;

namespace LogInspect.Modules.Test.Mocks
{
	public class MockedLibraryModule : LibraryModule<string>
	{
		public List<string> Items;

		public MockedLibraryModule() : base(NullLogger.Instance, new MockedDirectoryEnumerator(5))
		{
			Items = new List<string>();
		}

		protected override void OnItemLoaded(string Item)
		{
			Items.Add(Item);
		}

		protected override string OnLoadFile(string FileName)
		{
			return FileName;
		}

	}
}
