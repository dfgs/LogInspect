using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.Modules.Test.Mocks
{
	public class MockedDirectoryEnumerator : IDirectoryEnumerator
	{
		private int count;

		public MockedDirectoryEnumerator(int Count)
		{
			this.count = Count;
		}

		public IEnumerable<string> EnumerateFiles(string Path)
		{
			for (int t = 0; t < count; t++) yield return $"Path{t}";
		}
	}
}
